using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace EpubSpellChecker
{
    /// <summary>
    /// A representation of an epub file
    /// </summary>
    public class Epub : IDisposable
    {
        private Ionic.Zip.ZipFile file;
        public Epub(Ionic.Zip.ZipFile file)
        {
            this.file = file;
        }

        /// <summary>
        /// The title of the book
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The author of the book
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// An entry in the manifest of the epub
        /// </summary>
        public class Entry
        {
            /// <summary>
            /// The relative path in the epub zip file
            /// </summary>
            public string Href { get; set; }
            /// <summary>
            /// The mime type of the file entry
            /// </summary>
            public string MimeType { get; set; }
        }

        /// <summary>
        /// A text entry in the epub file
        /// </summary>
        public class HtmlEntry : Entry
        {
            /// <summary>
            /// The plain html
            /// </summary>
            public string Html { get; set; }
        }

        /// <summary>
        /// All the html entries in the epub file by their href
        /// </summary>
        public Dictionary<string, HtmlEntry> Entries { get; private set; }
        /// <summary>
        /// The sequential order of the html entries
        /// </summary>
        public List<HtmlEntry> EntryOrder { get; private set; }

        /// <summary>
        /// Fully read an epub file to memory and keep the text entries and some general info like Title and Author seperate
        /// </summary>
        /// <param name="path">The path of the epub file</param>
        /// <returns>An epub object read from the given file</returns>
        public static Epub FromFile(string path)
        {
            // read the entire file, and interpret it as a zip file
            var epubBytes = System.IO.File.ReadAllBytes(path);
            var file = Ionic.Zip.ZipFile.Read(epubBytes);

            Epub epub = new Epub(file);

            // read the metadata container xml info
            XmlDocument doc = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                file[@"META-INF\container.xml"].Extract(ms);
                ms.Position = 0;
                doc.Load(ms);
            }

            // determine the href of the content manifest, which is stored in the full-path attribute of the rootfile tag
            var node = doc.ChildNodes.GetAllNodes().Where(n => n.Name == "rootfile").FirstOrDefault();
            if (node != null)
            {
                string contentPath = node.Attributes["full-path"].Value;

                // keep the relative path to the manifest file, because all entries in the manifest will be relative 
                string basePath = System.IO.Path.GetDirectoryName(contentPath);
                using (MemoryStream ms = new MemoryStream())
                {
                    file[contentPath].Extract(ms);
                    ms.Position = 0;
                    doc = new XmlDocument();
                    doc.LoadXml(XDocument.Load(ms).Root.StripNamespaces().ToString());
                }

                // read the title if present
                var titleNode = doc.SelectSingleNode("package/metadata/title");
                if (titleNode != null)
                    epub.Title = titleNode.InnerText;

                // read the author if present
                var authorNode = doc.SelectSingleNode("package/metadata/creator");
                if (authorNode != null)
                    epub.Author = authorNode.InnerText;

                // read all the entries in the manifest
                var items = doc.SelectNodes("package/manifest/item");

                Dictionary<string, HtmlEntry> entries = new Dictionary<string, HtmlEntry>(items.Count);
                var entryOrder = new List<HtmlEntry>(items.Count);

                foreach (var item in items.Cast<XmlNode>())
                {
                    string href = System.IO.Path.Combine(basePath, item.Attributes["href"].Value);
                    string mimeType = item.Attributes["media-type"].Value;

                    // if the entry is a html file
                    if (mimeType == "application/xhtml+xml" || mimeType.Contains("html"))
                    {
                        // extract the file to a a memory stream and read it to a string
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file[href].Extract(ms);
                            ms.Position = 0;
                            StreamReader reader = new StreamReader(ms);
                            string html = reader.ReadToEnd();

                            // store the entry
                            var te = new HtmlEntry()
                            {
                                Href = href,
                                MimeType = mimeType,
                                Html = html
                            };

                            entries.Add(href, te);
                            entryOrder.Add(te);
                        }

                    }
                }
                epub.Entries = entries;
                epub.EntryOrder = entryOrder;
            }
            else
                throw new Exception("No content metadata");



            return epub;
        }

        /// <summary>
        /// Saves the epub to the given path
        /// </summary>
        /// <param name="path">The path to save the epub to</param>
        public void Save(string path)
        {
            // work on a copy
            Ionic.Zip.ZipFile newFile;
            using (MemoryStream ms = new MemoryStream())
            {
                file.Save(ms);
                ms.Position = 0;
                newFile = Ionic.Zip.ZipFile.Read(ms);

                // remove the old entry files and add the modified html entries
                foreach (var e in Entries.Values)
                {
                    newFile.RemoveEntry(e.Href);
                    newFile.AddFileFromString(System.IO.Path.GetFileName(e.Href), System.IO.Path.GetDirectoryName(e.Href), e.Html);
                }
                newFile.Save(path);
            }
        }

       /// <summary>
       /// Makes a complete clone of the epub
       /// </summary>
       /// <returns>A full clone of the epub</returns>
        public Epub Clone()
        {
            // copy the zip file to a memory stream and read it
            Ionic.Zip.ZipFile newFile;
            MemoryStream ms = new MemoryStream();
            file.Save(ms);
            ms.Position = 0;
            newFile = Ionic.Zip.ZipFile.Read(ms);

            Epub clone = new Epub(newFile)
            {
                Author = Author,
                Title = Title,
            };
            clone.Entries = new Dictionary<string, HtmlEntry>();
            clone.EntryOrder = new List<HtmlEntry>();

            // copy all the entries
            foreach (var e in EntryOrder)
            {
                var clonedEntry = new HtmlEntry() { Href = e.Href, Html = e.Html, MimeType = e.MimeType };
                clone.Entries.Add(e.Href, clonedEntry);
                clone.EntryOrder.Add(clonedEntry);
            }

            return clone;
        }

        /// <summary>
        /// Disposes the current epub file by disposing the underlying zip file
        /// </summary>
        public void Dispose()
        {
            file.Dispose();
        }
    }
}
