using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EpubSpellChecker
{
    /// <summary>
    /// A word entry in the text. A word entry has multiple occurences
    /// </summary>
    class WordEntry : INotifyPropertyChanged
    {
    
        public WordEntry(IEnumerable<Word> words)
        {
            if (words.Count() > 1)
            {
                // take the text that occurs the most
                Text = words.GroupBy(w => w.Text).OrderByDescending(g => g.Count()).First().Key;
            }
            else
            {
                var word = words.First();
                Text = word.Text;
            }

            Suggestion = "";
            FixedText = "";
            Occurrences = words.ToArray();
        }

        /// <summary>
        /// The string version of the word
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The number of occurences of the word
        /// </summary>
        public int Count { get { return Occurrences.Length; } }

        /// <summary>
        /// A list of the occurences of the word in the entire epub
        /// </summary>
        public Word[] Occurrences { get; private set; }

        /// <summary>
        /// Contains how much the same previous and next words are encountered over all the occurrences
        /// </summary>
        public NeighbourWords Neighbours { get; set; }

        public class NeighbourWords
        {
            public Dictionary<WordEntry, int> PreviousWords { get; set; }
            public Dictionary<WordEntry, int> NextWords { get; set; }
        }

        private string fixedText;
        /// <summary>
        /// The fixed text the user has specified for the word, this will be used to replace the word occurences in the html entries of the epub
        /// </summary>
        public string FixedText
        {
            get { return fixedText; }
            set
            {
                if (fixedText != value) { fixedText = value; NotifyPropertyChanged("FixedText"); }
            }
        }

        private string suggestion;
        /// <summary>
        /// A suggested fix for the word, to make it known
        /// </summary>
        public string Suggestion
        {
            get { return suggestion; }
            set { if (suggestion != value) { suggestion = value; NotifyPropertyChanged("Suggestion"); } }
        }

        
        private string unknownType;
        /// <summary>
        /// Contains information of what exactly is wrong with the word and how the suggestion was built
        /// </summary>
        public string UnknownType
        {
            get { return unknownType; }
            set { if (unknownType != value) { unknownType = value; NotifyPropertyChanged("UnknownType"); } }
        }

        /// <summary>
        /// Flags if the word is recognized by the dictionary, true if it isn't
        /// </summary>
        public bool IsUnknownWord { get; set; }

        private bool ignore;
        /// <summary>
        /// Flags if all the occurences should be ignored
        /// </summary>
        public bool Ignore
        {
            get { return ignore; }
            set { if (ignore != value) { ignore = value; NotifyPropertyChanged("Ignore"); } }
        }

        /// <summary>
        /// An array of suggestions that to fix the word error
        /// </summary>
        public string[] DictionarySuggesions { get; set; }

        /// <summary>
        /// Flags if the word exists in the dictionary, but some OCR patterns have been applied on other words that can also be applied
        /// on this word, which means it could contain an OCR error
        /// </summary>
        public bool IsWarning { get; set; }


        /// <summary>
        /// Flags if the word entry needs to be checked by the user
        /// </summary>
        public bool NeedsWork
        {
            get
            {
                return IsUnknownWord || IsWarning;
            }
        }


        public override string ToString()
        {
            return "Entry: " + Text + " [" + Count + "]";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies any listeners that a given property has changed
        /// </summary>
        /// <param name="info">The property name</param>
        protected void NotifyPropertyChanged(String info)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
            {
                try
                {
                    propChanged(this, new PropertyChangedEventArgs(info));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error notifying change of property '" + info + "' of word entry:" + ex.GetType().FullName + " " + ex.Message);
                    // ignore errors on notify
                }
            }
        }

    }
}
