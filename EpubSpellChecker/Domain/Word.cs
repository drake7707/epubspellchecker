using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    /// <summary>
    /// A word occurence in the epub
    /// </summary>
    class Word
    {
        /// <summary>
        /// The word in string form
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The href of the entry where the word is located in
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// The character offset in the entry where the word begins
        /// </summary>
        public int CharOffset { get { return OriginalCharPositions[0]; } }

        /// <summary>
        /// The source character positions for each character in the word (this can not always be consecutive, e.g when formatting tags are splitting the word in half)
        /// </summary>
        public int[] OriginalCharPositions { get; set; }

        /// <summary>
        /// Flags if the word is the start of a sentence
        /// </summary>
        public bool IsStartOfSentence { get; set; }

        /// <summary>
        /// Flags if the word should be ignored when replacing all the words with their fixed versions
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// The previous word in the text, if the word is the start of a sentence, the previous word will be null
        /// </summary>
        public Word Previous { get; set; }

        /// <summary>
        /// The next word in the text, if the next word will be the start of a new sentence, this will be null
        /// </summary>
        public Word Next { get; set; }
    }
}
