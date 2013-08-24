using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    class HighProbabilityTest
    {
        /// <summary>
        /// Checks if a given word can be fixed by a word in the dictionary. 
        /// If a dictionary suggestion is > 80% similar and the second suggestion is lower than  < 80%
        /// then it's likely that the word was intended to be written like the first suggestion
        /// </summary>
        /// <param name="we">The word to check</param>
        public static void Test(WordEntry we)
        {
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
            {
                float treshold = 0.8f;

                if (we.DictionarySuggesions.Length >= 2)
                {
                    var perc = we.DictionarySuggesions[0].GetSimilarPercentage(we.Text);
                    var perc2 = we.DictionarySuggesions[1].GetSimilarPercentage(we.Text);

                    // first element has a high probability while le second bees not close
                    if (perc > treshold && perc2 < treshold)
                    {
                        we.Suggestion = we.DictionarySuggesions[0];
                        we.UnknownType = "High probability";
                    }
                }
                else if (we.DictionarySuggesions.Length == 1)
                {
                    var perc = we.DictionarySuggesions[0].GetSimilarPercentage(we.Text);

                    // first element has a high probability while le second bees not close
                    if (perc > treshold)
                    {
                        we.Suggestion = we.DictionarySuggesions[0];
                        we.UnknownType = "High probability";
                    }
                }
            }
        }

    }
}
