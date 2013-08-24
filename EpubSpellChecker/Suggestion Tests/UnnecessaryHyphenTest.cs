using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    class UnnecessaryHyphenTest
    {
        /// <summary>
        /// Tests if the word can be written without hyphens. Unnecessary hyphens occurs in text when
        /// the word is too long for the line in the book and needs to be word wrapped by syllable.
        /// </summary>
        /// <param name="we">The word entry to test</param>
        /// <param name="fullDictionary">The full dictionary available</param>
        public static void Test(WordEntry we, HashSet<string> fullDictionary)
        {
            // check if the word contains a hyphen
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion) && we.Text.Contains('-'))
            {
                var parts = we.Text.Split('-');

                // if any of the parts is only 1 character long, it's highly likely not a word that was split on multiple newlines
                if (parts.Any(part => part.Length <= 1))
                {
                    // not a split word
                }
                else
                {
                    // check if the word also exists by removing the hyphens 
                    var testWord = we.Text.Replace("-", "");
                    if (fullDictionary.Contains(testWord.ToLower()))
                    {
                        // it exists in the dictionary
                        we.Suggestion = testWord;
                        we.UnknownType = "Unneeded hyphen";
                    }
                    else
                    {
                        // check if all of the parts are seperate words that exist in the dictionary
                        // this is sometimes used to link words in a sentence together
                        var partsAreSeperateRecognizedWords = we.Text.Split('-').All(part => fullDictionary.Contains(part.Trim().ToLower()));
                        if (partsAreSeperateRecognizedWords)
                        {
                            we.UnknownType = "";
                            we.IsUnknownWord = false;
                        }
                    }
                }
            }
        }

    
    }
}
