using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    class HighProbabilityOnNeighboursTest
    {
        /// <summary>
        /// Checks if the given word is occurring less beween its previous and/or next word than other words with the same neighbours.
        /// If so, it's possible that that word is preferable
        /// </summary>
        /// <param name="we">The given word to check</param>
        /// <param name="flagAsWarning">If true, also flag the word entry as IsWarning</param>
        public static void Test(WordEntry we, bool flagAsWarning)
        {
            if (we.IsUnknownWord && string.IsNullOrEmpty(we.Suggestion))
            {
                // check if there is another word that is very similar but occurs more, e.g
                // [he] stood upright
                // [he] stood still
                // [be] stood at the edge
                // -> The 'he' occurs more, so it's possible that 'be' is an error and should be 'he'

                // the next is the 'stood' in the example above
                foreach (var next in we.Neighbours.NextWords)
                {
                    // look if there are similar words
                    var similar = next.Key.Neighbours.PreviousWords.Where(pair => pair.Key != we)
                                                                       .OrderBy(p => p.Key.Text.GetDistance(we.Text))
                                                                       .Take(5)
                                                                       .ToArray();

                    if (similar.Length > 0)
                    {
                        var mostSimilar = similar.First();
                        var nrOfDiff = mostSimilar.Key.Text.ToLower().GetDistance(we.Text.ToLower());
                        if (nrOfDiff < we.Text.Length / 2f)
                        {
                            // find the entry of we
                            var thisEntry = next.Key.Neighbours.PreviousWords.Where(pair => pair.Key == we).First();

                            // the other entry is similar enough, see if it occurs more 
                            if (IsHigherProbabilityThan(ref mostSimilar, ref thisEntry))
                            {
                                if (flagAsWarning)
                                    we.IsWarning = true;
                                we.Suggestion = mostSimilar.Key.Text;
                                we.UnknownType = "Possibility based on pattern";
                                return;
                            }
                        }
                    }
                }

                foreach (var prev in we.Neighbours.PreviousWords)
                {
                    // look if there are similar words
                    var similar = prev.Key.Neighbours.NextWords.Where(pair => pair.Key != we)
                                                                       .OrderBy(p => p.Key.Text.GetDistance(we.Text))
                                                                       .Take(5)
                                                                       .ToArray();

                    if (similar.Length > 0)
                    {
                        var mostSimilar = similar.First();
                        var nrOfDiff = mostSimilar.Key.Text.ToLower().GetDistance(we.Text.ToLower());
                        if (nrOfDiff < we.Text.Length / 2f)
                        {
                            // find the entry of we
                            var thisEntry = prev.Key.Neighbours.NextWords.Where(pair => pair.Key == we).First();

                            // the other entry is similar enough, see if it occurs more 
                            if (IsHigherProbabilityThan(ref mostSimilar, ref thisEntry))
                            {
                                if (flagAsWarning)
                                    we.IsWarning = true;

                                we.Suggestion = mostSimilar.Key.Text;
                                we.UnknownType = "Possibility based on pattern";
                                return;
                            }
                        }
                    }   
                }
            }
        }

        private static bool IsHigherProbabilityThan(ref KeyValuePair<WordEntry, int> mostSimilar, ref KeyValuePair<WordEntry, int> thisEntry)
        {
            // if it's a unknown word, just higher occurrence will do, but if it is a valid word from the dictionary, only suggest
            // once it occurs 10 times more in the book overall and 10 times more in the same pattern (otherwise there would be way too many false
            // positives)
            if (!thisEntry.Key.IsUnknownWord)
                return mostSimilar.Key.Count >= 10 * thisEntry.Key.Count &&
                                                mostSimilar.Value >= 10 * thisEntry.Value;
            else
                return mostSimilar.Value > thisEntry.Value;
        }
    }
}
