using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    class MissingSpacesTest
    {
        /// <summary>
        /// Checks if the word can be broken into seperate words
        /// </summary>
        /// <param name="we">The word to check</param>
        /// <param name="wordsOccurences">A collection of all the words present in the epub</param>
        /// <param name="fullDictionary">The dictionary of all valid words</param>
        public static void Test(WordEntry we, Dictionary<string, WordEntry> wordsOccurences, HashSet<string> fullDictionary)
        {
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
            {
                string fixedText;
                fixedText = GetFixedMissingSpaces2(we.Text, wordsOccurences, fullDictionary);
                if (!string.IsNullOrEmpty(fixedText))
                {
                    we.Suggestion = fixedText;
                    we.UnknownType = "Missing spaces";
                }

            }
        }

        /// <summary>
        /// Represents a word node in a tree of all the combinations the original word can be split into
        /// </summary>
        private class WordSequence
        {
            /// <summary>
            /// The parent node
            /// </summary>
            public WordSequence Parent;

            public string Word;

            /// <summary>
            /// All the child nodes
            /// </summary>
            public List<WordSequence> Sequences;

            /// <summary>
            /// The score of the current node, determining the likelyhood that this is the intended piece of the word
            /// </summary>
            public float Score { get; set; }

            public override string ToString()
            {
                return Word;
            }

            /// <summary>
            /// Gets all the child nodes recursively (depth first)
            /// </summary>
            /// <returns>A collection of word nodes</returns>
            public IEnumerable<WordSequence> GetAllNodes()
            {
                yield return this;

                if (Sequences == null)
                    yield break;

                foreach (var subn in Sequences)
                {
                    foreach (var subsubn in subn.GetAllNodes())
                        yield return subsubn;
                }
            }

            /// <summary>
            /// Moves up in the tree to build a path of words from the nodes encountered until the root node
            /// </summary>
            /// <returns>A list of words and their score </returns>
            public List<KeyValuePair<string, float>> GetWordPath()
            {
                Stack<KeyValuePair<string, float>> lst = new Stack<KeyValuePair<string, float>>();
                WordSequence ws = this;
                while (ws != null)
                {
                    lst.Push(new KeyValuePair<string, float>(ws.Word, ws.Score));
                    ws = ws.Parent;
                }
                return lst.ToList();
            }
        }

        /// <summary>
        /// Returns the possible intended sentence from the word
        /// </summary>
        /// <param name="text">The word to split into spaces</param>
        /// <param name="wordsOccurences">All the words in the epub</param>
        /// <param name="fullDictionary">The dictionary that contains valid words</param>
        /// <returns>A suggestion that was most likely the intended seperation of words</returns>
        private static string GetFixedMissingSpaces2(string text, Dictionary<string, WordEntry> wordsOccurences, HashSet<string> fullDictionary)
        {
            // gets all the possibilities in a tree
            List<WordSequence> wordSequences = GetWordSequences(default(WordSequence), text, wordsOccurences, fullDictionary);

            // get all the leaves of the tree
            var leaves = wordSequences.SelectMany(ws => ws.GetAllNodes()).Where(ws => ws.Sequences == null);

            // select all the paths to the root to get all the possible splits of the word
            List<List<KeyValuePair<string, float>>> wordChains = leaves.Select(l => l.GetWordPath()).ToList();

            // determine the best word path
            float maxsumScore = float.MinValue;
            int maxLength = int.MaxValue;
            List<KeyValuePair<string, float>> max = null;
            foreach (var wc in wordChains)
            {
                // determine the total sum of the score
                var sumScore = wc.Sum(l => l.Value);
                var length = wc.Count;

                bool hasOnlyPartsThatAre2CharOrLess = wc.All(l => l.Key.Length <= 2);
                if (hasOnlyPartsThatAre2CharOrLess)
                {
                    // ignore, it gives too many false positives
                }
                else if (wc.Any(w => w.Key.Length >= 2 && IsOnlyConsonants(w.Key) || IsOnlyVowels(w.Key)))
                {
                    // ignore, this will remove some splitting to abbreviations
                }
                else
                {
                    // less parts -> better
                    if (length < maxLength || (length >= maxLength && sumScore > maxsumScore))
                    {
                        max = wc;
                        maxLength = length;
                        maxsumScore = sumScore;
                    }
                }
            }
            if (max == null)
                return "";

            // returns the word path in a string with spaces between each piece
            return string.Join(" ", max.Select(l => l.Key));
        }

        private static HashSet<char> VOWELS = new HashSet<char>(new char[] { 'a', 'e', 'i','o', 'u', 'y' });

        /// <summary>
        /// Checks if  a string only contains vowels
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>True if all the characters are vowels</returns>
        private static bool IsOnlyVowels(string str)
        {
            return !str.Any(ch => !VOWELS.Contains(ch));
        }

        /// <summary>
        /// Checks if  a string only contains consonants
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>True if all the characters are consonants</returns>
        private static bool IsOnlyConsonants(string str)
        {
            return !str.Any(ch => VOWELS.Contains(ch));
        }

        /// <summary>
        /// Gets the score of a word
        /// </summary>
        /// <param name="word">The word to rate</param>
        /// <param name="wordsOccurences">All the words in the epub</param>
        /// <returns>A score that determines how likely the word was the intended word</returns>
        private static float GetScore(string word, Dictionary<string, WordEntry> wordsOccurences)
        {
            int count = 0;
            WordEntry occurences;
            if (wordsOccurences.TryGetValue(word.ToLower(), out occurences))
                count = word.Length * word.Length; //occurences.Count;
            else
                count = 0;

            float wordscore = count; // (20 - (float)word.Length);
            return wordscore;
        }

        /// <summary>
        /// Builds a tree of all possible pieces of the words recursively
        /// </summary>
        /// <param name="parent">The parent node</param>
        /// <param name="text">The current text that needs to be broken into pieces</param>
        /// <param name="wordsOccurences">All the words in the epub</param>
        /// <param name="fullDictionary">The dictionary that contains valid words</param>
        /// <returns>A collection of word nodes that represent all the possible pieces</returns>
        private static List<WordSequence> GetWordSequences(WordSequence parent, string text, Dictionary<string, WordEntry> wordsOccurences, HashSet<string> fullDictionary)
        {
            // there is nothing more to split, the current node is a leaf
            if (text == "")
                return null;

            List<WordSequence> wordSequences = new List<WordSequence>();
            int idx = 0;
            while (idx < text.Length)
            {
                // read until a part of the word is valid in the dictionary, then take that part as a word node and continue splitting 
                // the rest as child nodes
                string part = text.Substring(0, idx + 1);
                if (part.Length > 1 && fullDictionary.Contains(part.ToLower()))
                {
                    // create the word node of the valid part of the word
                    WordSequence ws = new WordSequence() { Word = part, Score = GetScore(part, wordsOccurences) };
                    // continue splitting up the remainder and add them as child nodes
                    ws.Sequences = GetWordSequences(ws, text.Substring(idx + 1), wordsOccurences, fullDictionary);
                    ws.Parent = parent;

                    wordSequences.Add(ws);
                }

                idx++;
            }
            return wordSequences;
        }

        //private static string GetFixedMissingSpaces(string text, HashSet<string> fullDictionary)
        //{
        //    string fixedText;

        //    List<string> seperateWords = new List<string>();

        //    string word = text;
        //    int idx = 0;
        //    int lastWordOffset = 0;
        //    while (idx < text.Length)
        //    {
        //        string str = GetLongestMatchingWord(word.Substring(lastWordOffset), fullDictionary);
        //        if (str.Length <= 2) // do not find a match if parts are only <= 2 in length
        //            str = "";

        //        if (!string.IsNullOrEmpty(str))
        //        {
        //            seperateWords.Add(str);
        //            idx = idx + str.Length - 1;
        //            lastWordOffset = idx + 1;
        //        }
        //        else
        //        {
        //            // prepend all previous words to see if it makes a word then
        //            if (seperateWords.Count > 0)
        //            {
        //                string combinedstr = word.Substring(lastWordOffset);

        //                bool foundMatch = false;
        //                for (int i = seperateWords.Count - 1; i >= 0; i--)
        //                {
        //                    combinedstr = seperateWords[i] + combinedstr;
        //                    if (fullDictionary.Contains(combinedstr))
        //                    {
        //                        seperateWords = new List<string>(seperateWords.Take(i));
        //                        seperateWords.Add(combinedstr);
        //                        lastWordOffset = idx + 1;
        //                        foundMatch = true;
        //                        break;
        //                    }
        //                }
        //                if (!foundMatch)
        //                {
        //                    // can't find match,unless it we need to break previous parts up, TODO ?
        //                    idx = text.Length;
        //                }
        //            }

        //        }
        //        idx++;
        //    }

        //    if (lastWordOffset == text.Length)
        //    {
        //        fixedText = string.Join(" ", seperateWords);
        //    }
        //    else
        //        fixedText = "";
        //    return fixedText;
        //}

        //private static string GetLongestMatchingWord(string word, HashSet<string> fullDictionary)
        //{
        //    for (int i = word.Length - 1; i >= 0; i--)
        //    {
        //        var str = word.Substring(0, i + 1);
        //        if (fullDictionary.Contains(str))
        //            return str;
        //    }
        //    return "";
        //}


    }
}
