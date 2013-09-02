using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace EpubSpellChecker
{
    [Description("Checks if there are OCR errors in the word based on the OCR patterns file")]
    [DisplayName("OCR Errors")]
    class OCRErrorTest : ITest 
    {
        /// <summary>
        /// Represents the result of the OCR test
        /// </summary>
        public class OCRResult
        {
            /// <summary>
            /// Flags if the result resulted in a suggestion
            /// </summary>
            public bool IsFixed { get; set; }
            /// <summary>
            /// The suggestion of the OCR
            /// </summary>
            public string FixedWord { get; set; }

            /// <summary>
            /// The source pattern of the OCR applied (e.g the 'di' in 'di -> th')
            /// </summary>
            public string PatternSource { get; set; }
            /// <summary>
            /// The target pattern of the OCR applied (e.g the 'th' in 'di -> th')
            /// </summary>
            public string PatternTarget { get; set; }
        }

        /// <summary>
        /// Checks if there are OCR errors in the word and suggest a fix if there are
        /// </summary>
        /// <param name="we">The word to check</param>
        /// <param name="ocrPatterns">The available OCR patterns</param>
        /// <param name="fullDictionary">The dictionary that contains valid words</param>
        /// <param name="result">The result of the OCR test</param>
        public static void Test(WordEntry we, Dictionary<string, List<string>> ocrPatterns,  HashSet<string> fullDictionary, out OCRResult result)
        {
            if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
            {
                // get the result
                result = GetFixedTextFromOCRPattern(we.Text, ocrPatterns, fullDictionary);
                if (result.IsFixed)
                {
                    we.Suggestion = result.FixedWord;
                    we.UnknownType = "OCR";
                }
            }
            else
                result = null;
        }

        /// <summary>
        /// Checks if the word can be fixed by applying OCR replacements and return the result
        /// </summary>
        /// <param name="word">The word to check</param>
        /// <param name="ocrPatterns">The available OCR patterns</param>
        /// <param name="fullDictionary">The dictionary that contains valid words</param>
        /// <returns>The result of the OCR test</returns>
        private static OCRResult GetFixedTextFromOCRPattern(string word, Dictionary<string, List<string>> ocrPatterns, HashSet<string> fullDictionary)
        {
            var result = new OCRResult();

            // todo test more combinations, now only 1 match at a time will be replaced

            var input = word;
            // check each pattern in the available OCR patterns
            foreach (var pattern in ocrPatterns)
            {
                // get all the matching characters in the word for the current pattern source
                var matches = Regex.Matches(input, pattern.Key);
                foreach (var m in matches.Cast<Match>())
                {
                    // check for each pattern target until the word exists in the dictionary
                    foreach (var value in pattern.Value)
                    {
                        // build the new word where the source pattern is replaced with the target pattern
                        var newWord = input.Substring(0, m.Index) + value + input.Substring(m.Index + m.Length);

                        // if the dictionary contains the new word, then the OCR was successful
                        if (fullDictionary.Contains(newWord.ToLower()))
                        {
                            result.IsFixed = true;
                            result.FixedWord = newWord;
                            result.PatternSource = pattern.Key;
                            result.PatternTarget = value;
                            return result;
                        }
                    }

                }
            }
            result.IsFixed = false;
            return result;
        }

    }
}
