using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EpubSpellChecker
{
    [Description("Checks if a given word can be fixed by a word in the dictionary. The suggested word also has to be present in the book to narrow it down.")]
    [DisplayName("High probability")]
    class HighProbabilityTest : ITest
    {
        /// <summary>
        /// Checks if a given word can be fixed by a word in the dictionary. The suggested word also has to be present in the book to narrow it down.
        /// </summary>
        /// <param name="we">The word to check</param>
        public static void Test(WordEntry we, Dictionary<string, WordEntry> wordsOccurences)
        {
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
            {
                float treshold = 3f;

                var suggestions = we.DictionarySuggesions.TakeWhile(sugg => sugg.GetDistance(we.Text) < treshold)
                                                         .Where(sugg => wordsOccurences.ContainsKey(sugg.ToLower()))
                                                         .ToArray();


                if (suggestions.Length > 0)
                {
                    we.Suggestion = suggestions.First();
                    we.UnknownType = "High probability";
                }

                //if (suggestions.Length >= 2)
                //{

                //    var nrOfDiff = we.DictionarySuggesions[0].GetDistance(we.Text);
                //    var nrOfDiff2 = we.DictionarySuggesions[1].GetDistance(we.Text);

                //    // first element has a high probability while le second is not close
                //    if (nrOfDiff < treshold)// && nrOfDiff2 > treshold)
                //    {
                //        we.Suggestion = we.DictionarySuggesions[0];
                //        we.UnknownType = "High probability";
                //    }
                //}
                //else if (suggestions.Length == 1)
                //{
                //    var nrOfDiff = we.DictionarySuggesions[0].GetDistance(we.Text);

                //    // first element has a high probability
                //    if (nrOfDiff < treshold)
                //    {
                //        we.Suggestion = we.DictionarySuggesions[0];
                //        we.UnknownType = "High probability";
                //    }
                //}
            }
        }

    }
}
