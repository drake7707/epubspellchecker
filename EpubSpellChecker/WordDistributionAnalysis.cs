﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EpubSpellChecker
{
    partial class WordDistributionAnalysis : Form
    {

        /// <summary>
        /// Represents a word entry that can be checked
        /// </summary>
        class CheckableWordEntry : WordEntry
        {
            public CheckableWordEntry(WordEntry we)
                : base(we.Occurrences)
            {
                this.Check = false;
                this.UnknownType = we.UnknownType;
            }

            private bool check;
            /// <summary>
            /// Determines if the word is checked or not
            /// </summary>
            public bool Check
            {
                get { return check; }
                set { if (check != value) { check = value; NotifyPropertyChanged("Check"); } }
            }
        }

        private List<WordEntry> wordEntries;
        private Epub epub;
        public WordDistributionAnalysis(Epub epub, SortableBindingList<WordEntry> dataSource)
        {
            InitializeComponent();

            this.epub = epub;
            // make a copy
            this.wordEntries = dataSource.OriginalList.ToList();
            grid.DataSource = new SortableBindingList<CheckableWordEntry>(wordEntries.Select(we => new CheckableWordEntry(we)).ToList());
        }

        /// <summary>
        /// Occurs when the generate button is clicked.
        /// Use all checked word entries and insert the required json data
        /// in the word chart template html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void btnGenerate_Click(object sender, EventArgs ev)
        {
            var lst = grid.DataSource as SortableBindingList<CheckableWordEntry>;

            List<WordEntry> wordEntries = new List<WordEntry>();
            foreach (var we in lst)
            {
                if (we.Check)
                {
                    if (we != null)
                        wordEntries.Add(we);
                }
            }

            var entryOrder = epub.EntryOrder.Select((e, idx) => new KeyValuePair<string, int>(e.Href, idx))
                           .ToDictionary(p => p.Key, p => p.Value);

            var words = wordEntries.SelectMany(we => we.Occurrences).OrderBy(occ => entryOrder[occ.Href]).ThenBy(occ => occ.CharOffset).ToArray();

            Dictionary<string, Dictionary<int, int>> wordCountByWord = new Dictionary<string, Dictionary<int, int>>();

            // break down words into 40 classes max
            int classCount = 40;

            float groupSize = words.Length / (float)classCount;
            for (int i = 0; i < words.Length; i++)
            {
                int group = (int)(i / groupSize);
                string word = words[i].Text.TrimSuffix().ToLower();

                Dictionary<int, int> groupWords;
                if (!wordCountByWord.TryGetValue(word, out groupWords))
                    wordCountByWord[word] = groupWords = new Dictionary<int, int>();


                int wordCount;
                if (!groupWords.TryGetValue(group, out wordCount))
                    wordCount = 0;
                groupWords[group] = wordCount + 1;
            }


            StringBuilder series = new StringBuilder();
            series.Append("series: [");

            List<string> serieText = new List<string>();
            foreach (var pair in wordCountByWord)
            {
                string serie = "{";

                serie += "name: " + "'" + pair.Key.ProperCase() + "'," + Environment.NewLine;
                serie += "data: [";

                List<int> counts = new List<int>();
                for (int i = 0; i < classCount; i++)
                {
                    int wordCountOfGroup;
                    if (!pair.Value.TryGetValue(i, out wordCountOfGroup))
                        wordCountOfGroup = 0;
                    counts.Add(wordCountOfGroup);
                }
                serie += string.Join(",", counts);
                serie += "]";
                serie += "}";
                serieText.Add(serie);
            }

            series.Append(string.Join(",", serieText));
            series.Append("]");

            string pieData = "data: [" + string.Join(",", wordEntries.Select(we => "['" + we.Text.ProperCase() + "', " + we.Count + "]")) + "]";

            string html;

            // checks if there is a report template available in the program folder
            // if there isn't default to the one as embedded resource
            if (System.IO.File.Exists("wordDistributionReportTemplate.html"))
            {
                html = System.IO.File.ReadAllText("wordDistributionReportTemplate.html");
            }
            else
            {
                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("EpubSpellChecker.wordDistributionReportTemplate.html"))
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                    html = reader.ReadToEnd();
                }
            }

            html = html.Replace("%series%", series.ToString());
            html = html.Replace("%pieData%", pieData);


            // write to a temp file
            string tempFilename = System.IO.Path.GetTempFileName() + ".html";
            System.IO.File.WriteAllText(tempFilename, html);

            // open the html file
            System.Diagnostics.Process.Start(tempFilename);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            tmrFilter.Enabled = false;
            tmrFilter.Enabled = true;
        }

        private void tmrFilter_Tick(object sender, EventArgs e)
        {
            tmrFilter.Enabled = false;
            var lst = grid.DataSource as SortableBindingList<CheckableWordEntry>;
            if (lst != null)
            {
                if (string.IsNullOrEmpty(txtFilter.Text))
                    lst.Filter = "";
                else
                    lst.Filter = "Text %LIKE% '" + txtFilter.Text.Replace("'", "\'") + "'";
            }
        }

    }
}
