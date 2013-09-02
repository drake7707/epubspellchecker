using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EpubSpellChecker
{
    [Description("Checks if a given word can be fixed by a word in the dictionary. ")]
    [DisplayName("High probability")]
    class HighProbabilityTest : ITest
    {
        /// <summary>
        /// Checks if a given word can be fixed by a word in the dictionary. 
        /// If a dictionary suggestion is &gt; 80% similar and the second suggestion is lower than &lt; 80%
        /// then it's likely that the word was intended to be written like the first suggestion
        /// </summary>
        /// <param name="we">The word to check</param>
        public static void Test(WordEntry we)
        {
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
            {
                float treshold = 4f;

                if (we.DictionarySuggesions.Length >= 2)
                {
                    var nrOfDiff = we.DictionarySuggesions[0].GetDistance(we.Text);
                    var nrOfDiff2 = we.DictionarySuggesions[1].GetDistance(we.Text);

                    // first element has a high probability while le second is not close
                    if (nrOfDiff < treshold && nrOfDiff2 > treshold)
                    {
                        we.Suggestion = we.DictionarySuggesions[0];
                        we.UnknownType = "High probability";
                    }
                }
                else if (we.DictionarySuggesions.Length == 1)
                {
                    var nrOfDiff = we.DictionarySuggesions[0].GetDistance(we.Text);

                    // first element has a high probability
                    if (nrOfDiff < treshold)
                    {
                        we.Suggestion = we.DictionarySuggesions[0];
                        we.UnknownType = "High probability";
                    }
                }
            }
        }

    }
}
