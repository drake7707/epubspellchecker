using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EpubSpellChecker
{
    [Description("Checks if the given word is a plural form of a word, or an inflection (-s, -ing).")]
    [DisplayName("Suffixes")]
    class SuffixTest : ITest
    {

        private static Dictionary<string, string> suffixes = new Dictionary<string, string>()
        {
            { "s", "plural" },
            { "ing", "inflection" }
        };
        /// <summary>
        /// Checks if the given word is a plural form of a word, or an inflection (-s, -ing)
        /// </summary>
        /// <param name="we">The word entry to test</param>
        /// <param name="fullDictionary">The full dictionary available</param>
        public static void Test(WordEntry we, HashSet<string> fullDictionary)
        {
            if (we.IsUnknownWord && !we.Ignore)
            {
                foreach (var pair in suffixes)
                {
                    string suffix = pair.Key;
                    if (we.Text.Length > suffix.Length && we.Text.ToLower().EndsWith(suffix) && fullDictionary.Contains(we.Text.Substring(0, we.Text.Length - suffix.Length).ToLower()))
                    {
                        we.UnknownType = "Possible " +  pair.Value + "?";
                        we.Ignore = true;
                        return;
                    }
                }
            }
        }


    }
}
