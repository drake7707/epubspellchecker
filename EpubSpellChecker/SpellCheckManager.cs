using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EpubSpellChecker
{
    class SpellCheckManager
    {
        private Dictionary<string, List<string>> ocrPatterns = new Dictionary<string, List<string>>();
        private HashSet<string> fullDictionary = new HashSet<string>();

        /// <summary>
        /// The full dictionary (dictionary and custom dictionary)
        /// </summary>
        public HashSet<string> FullDictionary
        {
            get { return fullDictionary; }
        }

        private HashSet<string> customDictionary = new HashSet<string>();


        public SpellCheckManager(out List<string> warnings)
        {
            List<string> dicWarnings;

            // loads the dictionary from the dictionary.txt
            LoadDictionaries(out dicWarnings);

            // do a few tests to see if things work (used in debugging)
            if (System.Diagnostics.Debugger.IsAttached)
                DoTests();

            List<string> ocrWarnings;
            // loads the OCR patterns
            LoadOCRPatterns(out ocrWarnings);

            // combine the warnings from both loading operations
            warnings = dicWarnings.Concat(ocrWarnings).ToList();
        }

        private void DoTests()
        {
            string testHtml = "<h2>Blind<em>&#201; seer</em. <em>Fire</em>keep<br/er is&nbsp;a god. Amongst men</h2>";
            //string testHtml = " <p>Marshall in this regard makes his own thought<sup>1</sup> entirely clear:</p>";

            var words = GetWords("", testHtml).GroupBy(w => w.Text.ToLower())
                                              .ToDictionary(g => g.Key, g => g.ToArray());

            List<KeyValuePair<WordEntry, Word>> pairs = new List<KeyValuePair<WordEntry, Word>>();
            foreach (var w in words)
            {
                var we = new WordEntry(w.Value);
                if (we.Text == "BlindÉ")
                {
                    we.FixedText = "ble'eper";
                    we.IsUnknownWord = true;
                    we.UnknownType = "Test";
                }

                foreach (var occ in w.Value)
                    pairs.Add(new KeyValuePair<WordEntry, Word>(we, occ));
            }

            string replacedHtml = GetReplacedHtml(testHtml, pairs.ToArray());


            string text = "he stood upright. he stood with his back to the wall. be stood right on top of it.";
            var wordList = GetWords("", text);
            Dictionary<string, List<Word>> wordsOccurences = new Dictionary<string, List<Word>>();
            // append the words to the occurence dictionary
            foreach (var w in wordList)
            {
                List<Word> occurences;
                if (!wordsOccurences.TryGetValue(w.Text.ToLower(), out occurences))
                    wordsOccurences[w.Text.ToLower()] = occurences = new List<Word>();

                occurences.Add(w);
            }
            var wordEntries = CreateWordEntriesFromOccurrences(wordsOccurences);

            var wordEntry = wordEntries["be"];
            HighProbabilityTest.Test(wordEntry, wordEntries);
        }

        /// <summary>
        /// Load the OCR patterns from the ocrpatterns.txt file
        /// </summary>
        /// <param name="warnings">A list of warnings that are generated during the loading operation</param>
        private void LoadOCRPatterns(out List<string> warnings)
        {
            warnings = new List<string>();

            var ocrPatternsFilePath = GetOCRPatternsFilePath();
            if (System.IO.File.Exists(ocrPatternsFilePath))
            {
                // read all lines that are not empty and don't start with # (comment)
                foreach (var l in System.IO.File.ReadAllLines(ocrPatternsFilePath).Where(l => !string.IsNullOrEmpty(l.Trim()) && !l.Trim().StartsWith("#")))
                {
                    // if the line contains <->, the pattern needs to be applied in both ways
                    if (l.Contains("<->"))
                    {
                        var parts = Regex.Split(l, Regex.Escape("<->"));
                        if (parts.Length == 2)
                        {
                            // parse both parts and add them in the pattern dictionary
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();
                            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                            {
                                List<string> lst;
                                if (!ocrPatterns.TryGetValue(key, out lst))
                                    ocrPatterns[key] = lst = new List<string>();
                                lst.Add(value);

                                if (!ocrPatterns.TryGetValue(value, out lst))
                                    ocrPatterns[value] = lst = new List<string>();
                                lst.Add(key);
                            }
                        }
                    }
                    // if the line contains -> the pattern only needs to be applied in a forward direction
                    else if (l.Contains("->"))
                    {
                        var parts = Regex.Split(l, Regex.Escape("->"));
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();
                            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                            {
                                List<string> lst;
                                if (!ocrPatterns.TryGetValue(key, out lst))
                                    ocrPatterns[key] = lst = new List<string>();
                                lst.Add(value);
                            }
                        }
                    }
                }
            }
            else
                warnings.Add("No ocrpatterns.txt available, no attempt at automatically suggesting spell check fixes will be done");
        }

        private static string GetOCRPatternsFilePath()
        {
            var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "ocrpatterns.txt");
            if (System.IO.File.Exists(path))
                return path;

            return "ocrpatterns.txt";
        }

        /// <summary>
        /// Loads the dictionary from the dictionary.txt
        /// </summary>
        /// <param name="warnings">A list of warnings that are generated during the loading operation</param>
        private void LoadDictionaries(out List<string> warnings)
        {
            warnings = new List<string>();

            var dictionaryPath = GetDictionaryFilePath();
            if (!System.IO.File.Exists(dictionaryPath))
            {
                warnings.Add("No dictionary.txt available, add a word list to have spell checking");
            }
            else
            {
                foreach (var l in System.IO.File.ReadAllLines(dictionaryPath).Where(l => !string.IsNullOrEmpty(l.Trim())))
                    fullDictionary.Add(l.ToLower());
            }

            var customDictionaryPath = GetCustomDictionaryPath();
            if (System.IO.File.Exists(customDictionaryPath))
            {
                foreach (var l in System.IO.File.ReadAllLines(customDictionaryPath).Where(l => !string.IsNullOrEmpty(l.Trim())))
                {
                    fullDictionary.Add(l.ToLower());
                    customDictionary.Add(l.ToLower());
                }
            }
        }

        private static string GetCustomDictionaryPath()
        {
            var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "custom_dictionary.txt");
            if (System.IO.File.Exists(path))
                return path;

            return "custom_dictionary.txt";
        }

        private static string GetDictionaryFilePath()
        {
            var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "dictionary.txt");
            if (System.IO.File.Exists(path))
                return path;

            return "dictionary.txt";
        }

        /// <summary>
        /// Analyses the epub by determining all the word entries
        /// </summary>
        /// <param name="epub">The epub file</param>
        /// <returns>A collection of word entries from the entire book</returns>
        public Dictionary<string, WordEntry> AnalyseEpub(Epub epub)
        {
            // get a list of all words by their lower case string value
            Dictionary<string, List<Word>> wordsOccurences = GetWordsByText(epub);
            var wordEntries = CreateWordEntriesFromOccurrences(wordsOccurences);
            return wordEntries;
        }

        private Dictionary<string, WordEntry> CreateWordEntriesFromOccurrences(Dictionary<string, List<Word>> wordsOccurences)
        {
            var wordEntries = new Dictionary<string, WordEntry>();
            foreach (var pair in wordsOccurences)
            {
                // create a word entry from the list of words
                List<Word> words = pair.Value;
                WordEntry we = new WordEntry(words);
                // check if the word is recognized (if it has a length > 1). If it's not try trimming the suffix (e.g 's) and see if it matches then
                we.IsUnknownWord = we.Text.Length > 1 && !(fullDictionary.Contains(we.Text.ToLower()) || fullDictionary.Contains(we.Text.ToLower().TrimSuffix()));
                wordEntries.Add(we.Text.ToLower(), we);
            }

            // complete the word entries with its neighbour data
            foreach (var we in wordEntries.Values)
            {
                FillNeighbours(we, wordEntries);
            }
            return wordEntries;
        }

        /// <summary>
        /// Gets all the words present and group by their lower string value
        /// </summary>
        /// <param name="epub">The epub file</param>
        /// <returns>A list of words grouped by their lower string value</returns>
        private Dictionary<string, List<Word>> GetWordsByText(Epub epub)
        {
            Dictionary<string, List<Word>> wordsOccurences = new Dictionary<string, List<Word>>();

            foreach (var entry in epub.Entries.Values.Where(e => e is Epub.HtmlEntry).Cast<Epub.HtmlEntry>())
            {
                // get all the words from the current text file
                var words = GetWords(entry.Href, entry.Html);

                // append the words to the occurence dictionary
                foreach (var w in words)
                {
                    List<Word> occurences;
                    if (!wordsOccurences.TryGetValue(w.Text.ToLower(), out occurences))
                        wordsOccurences[w.Text.ToLower()] = occurences = new List<Word>();

                    occurences.Add(w);
                }
            }
            return wordsOccurences;
        }

        /// <summary>
        /// Determines all the words from the input string
        /// </summary>
        /// <param name="href">The entry reference</param>
        /// <param name="input">The input string to get words from</param>
        /// <returns>An array of words that are present in the input string</returns>
        private static Word[] GetWords(string href, string input)
        {
            // the stringbuilder that will only contain the plain text, without any tags or escaped characters
            StringBuilder str = new StringBuilder();
            // a list of character offsets that match the stringbuilder's text
            List<int> originalOffsets = new List<int>();
            // the current tag depth (++ each time < is encountered not between quotes, -- when >)
            int tagDepth = 0;
            // flags if the current position is between quotes
            bool inSingleQuotes = false;
            bool inDoubleQuotes = false;

            // the current position
            int idx = 0;
            // flags if the current position is between <style> ... </style>. This is necessary to prevent tagDepth changes when css with > is encountered
            bool inStyleBlock = false;
            while (idx < input.Length)
            {
                if (idx >= "<style".Length && input.Substring(idx - "<style".Length, "<style".Length).ToLower() == "<style")
                    // a <style was encountered, flag inside style block
                    inStyleBlock = true;
                else if (idx >= "</style>".Length && input.Substring(idx - "</style>".Length, "</style>".Length).ToLower() == "</style>")
                    // the end of the style tag was encountered, flag inside style block
                    inStyleBlock = false;
                else if ((idx >= "<br".Length && input.Substring(idx - "<br".Length, "<br".Length).ToLower() == "<br") ||
                         (idx >= "<p".Length && input.Substring(idx - "<p".Length, "<p".Length).ToLower() == "<p") ||
                         (idx >= "<blockquote".Length && input.Substring(idx - "<blockquote".Length, "<blockquote".Length).ToLower() == "<blockquote"))
                {
                    // an explicit (br) or implicit new line was encountered, add a newline to the string, but add the character offsets as -1
                    str.Append('\r');
                    originalOffsets.Add(-1);
                    str.Append('\n');
                    originalOffsets.Add(-1);
                }
                else if ((idx >= "<sup".Length && input.Substring(idx - "<sup".Length, "<sup".Length).ToLower() == "<sup") ||
                         (idx >= "<sub".Length && input.Substring(idx - "<sub".Length, "<sub".Length).ToLower() == "<sub"))
                {
                    // superscript or subscript, ignore by adding a space
                    str.Append(' ');
                    originalOffsets.Add(-1);
                }

                if (input[idx] == '<')
                {
                    if (!inSingleQuotes && !inDoubleQuotes) // if not encountered in quotes, it's the start of a tag
                        tagDepth++;
                    else
                    {
                        if (tagDepth == 0 && !inStyleBlock) // when not encountered in the inside of a tag and not in a style block, append it
                        {
                            str.Append(input[idx]);
                            originalOffsets.Add(idx);
                        }
                    }
                }
                else if (input[idx] == '>')
                {
                    if (!inSingleQuotes && !inDoubleQuotes) // if not encountered in quotes, it's the end of a tag
                    {
                        if (tagDepth > 0)
                            tagDepth--;
                    }
                    else
                    {
                        if (tagDepth == 0 && !inStyleBlock) // when not encountered in the inside of a tag and not in a style block, append it
                        {
                            str.Append(input[idx]);
                            originalOffsets.Add(idx);
                        }
                    }
                }
                else if (input[idx] == '\"')
                {
                    if (tagDepth > 0) // only detect quotes when the current position is inside a tag
                    {
                        if (!inSingleQuotes) // when not surrounded by single quotes
                            inDoubleQuotes = !inDoubleQuotes;
                    }
                    else
                    {
                        // otherwise append it, it's part of the text
                        str.Append(input[idx]);
                        originalOffsets.Add(idx);
                    }
                }
                else if (input[idx] == '\'')
                {
                    if (tagDepth > 0) // only detect quotes when the current position is inside a tag
                    {
                        if (!inDoubleQuotes) // when not surrounded by double quotes
                            inSingleQuotes = !inSingleQuotes;
                    }
                    else
                    {
                        // otherwise append it, it's part of the text
                        str.Append(input[idx]);
                        originalOffsets.Add(idx);
                    }
                }
                else if (input[idx] == '&')
                {
                    if (!inStyleBlock && !inSingleQuotes && !inDoubleQuotes && tagDepth == 0)
                    {
                        // read until encountering ';' because this is a xml escaped word
                        StringBuilder escapedWord = new StringBuilder("");

                        int startAmp = idx;
                        while (idx < input.Length && input[idx] != ';')
                        {
                            if (input[idx] != ' ') // some books have spaces in the xml, ignore any spaces between & and ;
                                escapedWord.Append(input[idx]);
                            idx++;
                        }
                        escapedWord.Append(";");

                        // get the unescaped character
                        string unescaped = XmlHelper.GetUnescapedXmlCharacter(escapedWord.ToString());

                        // append the character
                        foreach (var ch in unescaped)
                        {
                            str.Append(ch);
                            originalOffsets.Add(startAmp);
                        }
                    }
                }
                else
                {
                    if (tagDepth == 0 && !inStyleBlock) // when not in a tag and not in the style block, append the char
                    {
                        str.Append(input[idx]);
                        originalOffsets.Add(idx);
                    }
                }
                idx++;
            }

            string plainText = str.ToString();
            var orgOffsets = originalOffsets.ToArray();

            // build regex matches for all words and all sentence endings ('.', '!', '?', '...')
            MatchCollection matches = Regex.Matches(plainText, @"(\b(&?[\w'-]*;?)\b)");
            MatchCollection sentenceEndMatches = Regex.Matches(plainText, @"([\.][^\d]?)|[\:!…\?]");


            List<Word> words = new List<Word>();
            // determines if the next word is the first word of a sentence
            bool nextWordIsFirstWordOfSentence = true;
            // the previous word that was encountered
            Word previousWord = null;

            // combine the matches with the sentence matches and iterate over them sorted by their index
            foreach (var m in matches.Cast<Match>()
                                     .Concat(sentenceEndMatches.Cast<Match>()).OrderBy(m => m.Index))
            {
                if (m.Value.StartsWith(".") || m.Value.StartsWith("?") || m.Value.StartsWith("!") || m.Value.StartsWith("…"))
                {
                    // the match is sentence ending match, flag that the next word will be the start of a sentence
                    nextWordIsFirstWordOfSentence = true;
                }
                else if (!string.IsNullOrEmpty(m.Value) && !(m.Value.StartsWith("&") && m.Value.EndsWith(";")))
                {
                    // gets the word of the match, and the character offsets matching that part
                    Word w = new Word()
                    {
                        Text = (m.Value),
                        Href = href,
                        OriginalCharPositions = orgOffsets.SubArray(m.Index, m.Length).ToArray(),
                        IsStartOfSentence = nextWordIsFirstWordOfSentence,
                    };

                    // if the word is not the start of a sentence, link the previous and current word together (if previous word is present)
                    if (!nextWordIsFirstWordOfSentence)
                    {
                        w.Previous = previousWord;
                        if (previousWord != null)
                            previousWord.Next = w;
                    }

                    words.Add(w);
                    previousWord = w;
                    nextWordIsFirstWordOfSentence = false;
                }
            }

            return words.ToArray();
        }

        /// <summary>
        /// Fills the neighbours of the word entry, along with how many times the neighbour occurred over the word occurrences
        /// </summary>
        /// <param name="we">The word entry to fill the neighbours for</param>
        /// <param name="wordEntries">A collection of all the word entries</param>
        public void FillNeighbours(WordEntry we, Dictionary<string, WordEntry> wordEntries)
        {
            var neighbour = new WordEntry.NeighbourWords();
            neighbour.PreviousWords = new Dictionary<WordEntry, int>();
            neighbour.NextWords = new Dictionary<WordEntry, int>();

            foreach (var w in we.Occurrences)
            {
                if (!w.IsStartOfSentence && w.Previous != null)
                {
                    WordEntry entryOfW;
                    if (wordEntries.TryGetValue(w.Previous.Text.ToLower(), out entryOfW))
                    {
                        int count;
                        if (!neighbour.PreviousWords.TryGetValue(entryOfW, out count))
                            neighbour.PreviousWords[entryOfW] = count = 1;
                        else
                            neighbour.PreviousWords[entryOfW] = ++count;
                    }
                }

                if (w.Next != null && !w.Next.IsStartOfSentence)
                {
                    WordEntry entryOfW;
                    if (wordEntries.TryGetValue(w.Next.Text.ToLower(), out entryOfW))
                    {
                        int count;
                        if (!neighbour.NextWords.TryGetValue(entryOfW, out count))
                            neighbour.NextWords[entryOfW] = count = 1;
                        else
                            neighbour.NextWords[entryOfW] = ++count;
                    }
                }
            }
            we.Neighbours = neighbour;
        }

        /// <summary>
        /// Fills the suggestion of a word entry
        /// </summary>
        /// <param name="we">The word entry to fill the suggestion for</param>
        /// <param name="wordEntries">A collection of all the word entries</param>
        /// <param name="ocrPatternsAppliedCount">A collection that keeps track of which ocr patterns have been applied and how many times</param>
        public void FillSuggestion(WordEntry we, Dictionary<string, WordEntry> wordEntries, Dictionary<string, Dictionary<string, int>> ocrPatternsAppliedCount, HashSet<string> enabledTests)
        {
            if (!we.IsUnknownWord)
            {
                // it's a word that is known, ignore and don't fill a suggestion

                we.Ignore = true;
            }
            else
            {
                // build the dictionary suggestions
                // note: this takes a long time!
                if (we.IsUnknownWord && !we.Ignore && string.IsNullOrEmpty(we.Suggestion))
                    we.DictionarySuggesions = fullDictionary
                                                    .Where(s => (char.ToLower(s[0]) == char.ToLower(we.Text[0]) || s.Last() == char.ToLower(we.Text.Last())) && Math.Abs(s.Length - we.Text.Length) <= 2) // only take the words that have a max 2 char length deviation
                                                    .OrderBy(s => s.GetDistance(we.Text.ToLower())).Take(10).ToArray();


                // test for numbers
                if (enabledTests.Contains(typeof(NumberTest).Name))
                    NumberTest.Test(we);

                // test for OCR errors
                if (enabledTests.Contains(typeof(OCRErrorTest).Name))
                {
                    OCRErrorTest.OCRResult result;
                    OCRErrorTest.Test(we, ocrPatterns, fullDictionary, out result);

                    // if the OCR pattern was succesfully applied, append it to the dictionary that keeps track of how many times a pattern is applied
                    if (result != null && result.IsFixed)
                    {
                        Dictionary<string, int> patternMatches;
                        // make sure to lock the dictionary, as this is executed in parallel
                        lock (ocrPatternsAppliedCount)
                        {
                            // add the pattern if it's not present
                            if (!ocrPatternsAppliedCount.TryGetValue(result.PatternSource, out patternMatches))
                                ocrPatternsAppliedCount[result.PatternSource] = patternMatches = new Dictionary<string, int>();
                        }

                        // lock and increase the count of the pattern or add it if it wasn't present yet
                        lock (patternMatches)
                        {
                            int ocrCount;
                            if (patternMatches.TryGetValue(result.PatternTarget, out ocrCount))
                                patternMatches[result.PatternTarget] = ocrCount + 1;
                            else
                                patternMatches[result.PatternTarget] = 1;
                        }
                    }
                }

                // test for name
                if (enabledTests.Contains(typeof(NameTest).Name))
                    NameTest.Test(we);

                // test for suffixes
                if (enabledTests.Contains(typeof(SuffixTest).Name))
                    SuffixTest.Test(we, fullDictionary);

                // test for unnecessary hyphens
                if (enabledTests.Contains(typeof(UnnecessaryHyphenTest).Name))
                    UnnecessaryHyphenTest.Test(we, fullDictionary);

                // test for unnecessary diacritics
                if (enabledTests.Contains(typeof(UnnecessaryDiacriticsTest).Name))
                    UnnecessaryDiacriticsTest.Test(we, fullDictionary);

                // test for high probability
                if (enabledTests.Contains(typeof(HighProbabilityTest).Name))
                    HighProbabilityTest.Test(we, wordEntries);

                // test for probability on neighbours
                if (enabledTests.Contains(typeof(HighProbabilityOnNeighboursTest).Name))
                    HighProbabilityOnNeighboursTest.Test(we, false);

                // test for missing spaces
                if (enabledTests.Contains(typeof(MissingSpacesTest).Name))
                    MissingSpacesTest.Test(we, wordEntries, fullDictionary);
            }
        }

        /// <summary>
        /// Checks if the word entry could be misinterpreted as a valid word, based on the the occurrence of previously applied OCR patterns
        /// </summary>
        /// <param name="we">The word entry to cheeck</param>
        /// <param name="wordEntries">A collection of all word entries</param>
        /// <param name="ocrPatternsAppliedCount">A collection that keeps track of which ocr patterns have been applied and how many times</param>
        public void FillWarnings(WordEntry we, Dictionary<string, WordEntry> wordEntries, Dictionary<string, Dictionary<string, int>> ocrPatternsAppliedCount)
        {
            //if (true || !we.IsUnknownWord)
            //{

            var settings = SettingsManager.GetSettings();
            if (settings.OCRWarnings)
            {
                foreach (var patternPair in ocrPatterns)
                {
                    foreach (var patternValue in patternPair.Value)
                    {
                        int nrTimesApplied;
                        Dictionary<string, int> appliedPatternValue;
                        if (ocrPatternsAppliedCount.TryGetValue(patternPair.Key, out appliedPatternValue) && appliedPatternValue.TryGetValue(patternValue, out nrTimesApplied))
                        {
                        }
                        else
                            nrTimesApplied = 0;

                        bool needToWarn = !settings.OnlyUseAppliedOCRPatternsForWarnings || (settings.OnlyUseAppliedOCRPatternsForWarnings && nrTimesApplied > 0);

                        if (needToWarn)
                        {
                            // check if the pattern matches the current word entry
                            var matches = Regex.Matches(we.Text, patternPair.Key);
                            foreach (var m in matches.Cast<Match>())
                            {
                                // for all matches, determine the new word and check if it is present in the dictionary
                                var newWord = we.Text.Substring(0, m.Index) + patternValue + we.Text.Substring(m.Index + m.Length);
                                if (fullDictionary.Contains(newWord.ToLower()))
                                {
                                    // the pattern applied on the word also exists (e.g rale -> rule)
                                    // check if rule exists as well in the word entries
                                    WordEntry targetWordEntry;
                                    if (wordEntries.TryGetValue(newWord.ToLower(), out targetWordEntry) && targetWordEntry.Occurrences.Length > 0)
                                    {
                                        // the new word also exists in the book, flag the word as a warning
                                        we.IsWarning = true;

                                        if (nrTimesApplied > 0)
                                            we.UnknownType = "Probable OCR error";
                                        else
                                            we.UnknownType = "Possible OCR error";

                                        we.Suggestion = newWord;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}

            //HighProbabilityOnNeighboursTest.Test(we, true);
        }

        /// <summary>
        /// Applies the fixed text of the word entries on the given epub file
        /// </summary>
        /// <param name="epub">The epub file to change</param>
        /// <param name="wordEntries">The word entry collection</param>
        public void Apply(Epub epub, IEnumerable<WordEntry> wordEntries)
        {
            // in order to preserve the character offsets of all the words, the words have to be replaced
            // in descending order per href entry.
            // group all the word entries by the href, and then sort them by character offset in descending order.
            var wordOccurencesByHrefInDescOrder = wordEntries.SelectMany(we => we.Occurrences.Select(occ => new KeyValuePair<WordEntry, Word>(we, occ)))
                                                    .GroupBy(pair => pair.Value.Href)
                                                    .ToDictionary(g => g.Key, g => g.OrderByDescending(pair => pair.Value.CharOffset).ToArray());

            foreach (var pair in wordOccurencesByHrefInDescOrder)
            {
                var href = pair.Key;
                var wordEntryOccurrencePairs = pair.Value;

                var te = (Epub.HtmlEntry)epub.Entries[href];
                string html = te.Html;

                // replace the words in the html of the epub entry
                string replacedHtml = GetReplacedHtml(html, wordEntryOccurrencePairs);
                te.Html = replacedHtml;
            }
        }

        /// <summary>
        /// Replaces all the given words in the given html
        /// </summary>
        /// <param name="html">The html to replace the words in</param>
        /// <param name="wordEntryOccurrencePairs">A collection of word entry-word pairs</param>
        /// <returns>The html string where all fixed text is applied</returns>
        private string GetReplacedHtml(string html, KeyValuePair<WordEntry, Word>[] wordEntryOccurrencePairs)
        {
            string replacedHtml = html;

            foreach (var weOcc in wordEntryOccurrencePairs)
            {
                var we = weOcc.Key;
                var occ = weOcc.Value;

                // if there is a fixed text available and both the word entry and word occurence is not ignored
                if (!we.Ignore && !occ.Ignore && !string.IsNullOrEmpty(we.FixedText))
                {
                    // determine the text before the word
                    string partBeforeWord = replacedHtml.Substring(0, occ.CharOffset);

                    // determine the part to replace
                    string partToReplace;

                    int lastCharPosition = occ.OriginalCharPositions.Last();
                    // if the last char position of the word occurence is a &, that means the last character was an escaped xml character
                    // determine the actual last character position that includes the escaped character fully
                    if (replacedHtml[lastCharPosition] == '&')
                    {
                        // an escaped xml character, include it fullly
                        while (replacedHtml[lastCharPosition] != ';')
                            lastCharPosition++;
                    }
                    partToReplace = replacedHtml.Substring(occ.CharOffset, lastCharPosition - occ.CharOffset + 1);

                    // determine the part after the word
                    string partAfterWord = replacedHtml.Substring(lastCharPosition + 1);


                    // unescape the entire part between tags.
                    // e.g "This is an ampersand (&amp;)<br/>Some other text" becomes This is an ampersand (&)<br/>Some other text"
                    // this change results in new character positions and is necessary to prevent breaking the escaped xml keywords
                    // into fragments
                    int[] newCharPositions;
                    string newPart = UnescapeBetweenTags(partToReplace, occ, out newCharPositions);

                    // make a stringbuilder to easily replace characters
                    StringBuilder partToReplaceStr = new StringBuilder(newPart);

                    // determine the proper casing of the fixed text.
                    // if the source word is all upper case or lower case, match it fully
                    // otherwise check for the start of a sentence and match accordingly
                    string fixedText = we.FixedText;
                    if (occ.Text.All(c => char.IsUpper(c)))
                        fixedText = fixedText.ToUpper();
                    else if (occ.Text.All(c => char.IsLower(c)))
                        fixedText = fixedText.ToLower();
                    else if (occ.IsStartOfSentence)
                        fixedText = fixedText.ProperCase();


                    // replace characters until either the source or fixed text length is met
                    int idx = 0;
                    var min = Math.Min(we.Text.Length, fixedText.Length);
                    while (idx < min)
                    {
                        var orgIdx = newCharPositions[idx];
                        partToReplaceStr[orgIdx - occ.CharOffset] = fixedText[idx];
                        idx++;
                    }

                    // if the source word was longer than the fixed text
                    if (newCharPositions.Length > fixedText.Length)
                    {
                        // remove the remaining characters
                        for (int i = newCharPositions.Length - 1; i >= idx; i--)
                        {
                            var orgIdx = newCharPositions[i];
                            partToReplaceStr.Remove(orgIdx - occ.CharOffset, 1);
                        }
                    }
                    // else if the source word was shorter than the fixed text
                    else if (newCharPositions.Length < fixedText.Length)
                    {
                        // append all the remaining characters of the fixed text
                        var orgIdx = newCharPositions.Last();
                        for (int i = fixedText.Length - 1; i >= idx; i--)
                        {
                            partToReplaceStr.Insert(orgIdx - occ.CharOffset + 1, fixedText[i]);
                        }
                    }

                    // now reescape the resulting text between tags
                    string escapedResult = EscapeBetweenTags(partToReplaceStr.ToString());

                    // and combine the entire html together
                    // note: this could be faster if a stringbuilder was used on the entire replacedHtml (//TODO?)
                    replacedHtml = partBeforeWord + escapedResult + partAfterWord;
                }
            }

            return replacedHtml;
        }

        /// <summary>
        /// Escapes xml characters in text between tags.
        /// e.g "This is an ampersand (&amp;)<br/>Some other text" becomes This is an ampersand (&)<br/>Some other text"
        /// </summary>
        /// <param name="partToReplace">The input string to replace the text in</param>
        /// <returns>An escaped xml string (without replacing tags that were present)</returns>
        private string EscapeBetweenTags(string partToReplace)
        {
            StringBuilder str = new StringBuilder();
            int inTagCount = 0;
            bool inQuotes = false;
            for (int idx = 0; idx < partToReplace.Length; idx++)
            {
                if (partToReplace[idx] == '<')
                {
                    if (!inQuotes) // not in quotes -> start of tag
                    {
                        inTagCount++;
                        str.Append(partToReplace[idx]);
                    }
                    else
                    {
                        if (inTagCount == 0) // not in a tag, escape
                            str.Append(System.Security.SecurityElement.Escape(partToReplace[idx] + ""));
                        else
                            str.Append(partToReplace[idx]);
                    }
                }
                else if (partToReplace[idx] == '>')
                {
                    if (!inQuotes) // not in quotes -> end of tag
                    {
                        if (inTagCount > 0)
                        {
                            str.Append(partToReplace[idx]);
                            inTagCount--;
                        }
                        else // not in a tag, escape
                            str.Append(System.Security.SecurityElement.Escape(partToReplace[idx] + ""));
                    }
                    else
                    {
                        if (inTagCount == 0) // in quotes, escape
                            str.Append(System.Security.SecurityElement.Escape(partToReplace[idx] + ""));
                        else
                            str.Append(partToReplace[idx]);
                    }
                }
                else if (partToReplace[idx] == '\"' || partToReplace[idx] == '\'')
                {
                    if (inTagCount > 0) // quote encountered in a tag, toggle quotes
                    {
                        inQuotes = !inQuotes;
                        str.Append(partToReplace[idx]);
                    }
                    else // not in a tag, escape
                        str.Append(System.Security.SecurityElement.Escape(partToReplace[idx] + ""));
                }
                else
                {

                    if (inTagCount == 0) // not in a tag, escape
                        str.Append(System.Security.SecurityElement.Escape(partToReplace[idx] + ""));
                    else
                        str.Append(partToReplace[idx]);
                }
            }
            // replace &apos; because it's not supported in html/xhtml huzzah
            return str.ToString().Replace("&apos;", "&#39;");
        }

        /// <summary>
        /// Unescapes the text between tags
        /// </summary>
        /// <param name="partToReplace">The text to unescape the text in</param>
        /// <param name="w">The word that has the character offsets</param>
        /// <param name="newCharPositions">The new character positions</param>
        /// <returns>The unescaped text</returns>
        private string UnescapeBetweenTags(string partToReplace, Word w, out int[] newCharPositions)
        {
            StringBuilder str = new StringBuilder();
            List<int> newCharIndices = new List<int>();

            // keep track of the total indices shifted
            int modifier = 0;
            int inTagCount = 0;
            bool inQuotes = false;
            for (int idx = 0; idx < partToReplace.Length; idx++)
            {
                if (partToReplace[idx] == '<')
                {
                    if (!inQuotes)
                        inTagCount++;
                    else
                    {
                        if (inTagCount == 0) // not in a tag, add the new char index
                            newCharIndices.Add(w.CharOffset + idx - modifier);
                    }

                    str.Append(partToReplace[idx]);
                }
                else if (partToReplace[idx] == '>')
                {
                    if (!inQuotes)
                    {
                        if (inTagCount > 0)
                            inTagCount--;
                    }
                    else
                    {
                        if (inTagCount == 0) // not in a tag, add the new char index
                            newCharIndices.Add(w.CharOffset + idx - modifier);
                    }

                    str.Append(partToReplace[idx]);

                }
                else if (partToReplace[idx] == '\"' || partToReplace[idx] == '\'')
                {
                    if (inTagCount > 0)
                        inQuotes = !inQuotes;
                    else
                    {
                        // not in a tag, add the new char index
                        newCharIndices.Add(w.CharOffset + idx - modifier);
                    }
                    str.Append(partToReplace[idx]);
                }
                else if (partToReplace[idx] == '&' && inTagCount == 0)
                {
                    if (!inQuotes)
                    {
                        int startOfAmp = idx;
                        // read until encountering ';' because this is a xml escaped word
                        StringBuilder escapedWord = new StringBuilder("");
                        while (idx < partToReplace.Length && partToReplace[idx] != ';')
                        {
                            if (partToReplace[idx] != ' ')
                                escapedWord.Append(partToReplace[idx]);
                            idx++;
                        }
                        escapedWord.Append(";");

                        string unescaped = XmlHelper.GetUnescapedXmlCharacter(escapedWord.ToString());

                        for (int ch = 0; ch < unescaped.Length; ch++)
                        {
                            str.Append(unescaped[ch]);
                            newCharIndices.Add(w.CharOffset + startOfAmp + ch - modifier);
                        }
                        // change the modifier by the differnce of length in the char positions
                        modifier += (escapedWord.Length - unescaped.Length);
                    }
                }
                else
                {
                    str.Append(partToReplace[idx]);
                    if (inTagCount == 0)
                        newCharIndices.Add(w.CharOffset + idx - modifier);
                }
            }

            newCharPositions = newCharIndices.ToArray();
            return str.ToString();
        }

        /// <summary>
        /// Adds a word the the custom dictionary
        /// </summary>
        /// <param name="item">The item to add to the dictionary</param>
        public void AddToDictionary(string item)
        {
            customDictionary.Add(item.ToLower());
            fullDictionary.Add(item.ToLower());
        }

        /// <summary>
        /// Saves the custom dictionary
        /// </summary>
        public void SaveCustomDictionary()
        {
            var customDictionaryPath = GetCustomDictionaryPath();
            System.IO.File.WriteAllText(customDictionaryPath, string.Join(Environment.NewLine, customDictionary));
        }

        /// <summary>
        /// Reloads the OCR patterns and apply them to the given word entries
        /// </summary>
        /// <param name="entries">The word entries to retest the OCR pattern check</param>
        public void ReloadOCRPatterns(IEnumerable<WordEntry> entries)
        {
            List<string> warnings;
            LoadOCRPatterns(out warnings);

            foreach (var we in entries)
            {
                if (we.IsUnknownWord)
                {
                    OCRErrorTest.OCRResult ocrResult;
                    OCRErrorTest.Test(we, ocrPatterns, fullDictionary, out ocrResult);
                }
            }
        }
    }
}
