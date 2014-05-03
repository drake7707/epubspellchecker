using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EpubSpellChecker
{
    [DisplayName("Unnecessary diacritics")]
    [Description("Tests if the word can be written without diacritics.")]
    class UnnecessaryDiacriticsTest : ITest
    {
        /// <summary>
        /// Tests if the word exists in the dictionary without accents.
        /// If it is, it'll probably mean that the OCR introduced those accents from artifacts
        /// </summary>
        /// <param name="we">The word entry to test</param>
        /// <param name="fullDictionary">The full dictionary available</param>
        public static void Test(WordEntry we, HashSet<string> fullDictionary)
        {
            // check if the word contains a hyphen
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
            {
                string withoutAccents = we.Text.RemoveDiacritics();

                if (we.Text != withoutAccents && fullDictionary.Contains(withoutAccents))
                {
                    we.Suggestion = withoutAccents;
                    we.UnknownType = "Unnecessary diacritics";
                }
            }
        }


    }
}
