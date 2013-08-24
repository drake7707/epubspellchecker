using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    class NumberTest
    {
        /// <summary>
        /// Checks if the given word is a number
        /// A number can be either a decimal or an integer
        /// </summary>
        /// <param name="we">The word to test</param>
        public static void Test(WordEntry we)
        {
            if (we.IsUnknownWord && !we.Ignore)
            {
                int val;
                double dval;

                if (int.TryParse(we.Text, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out val))
                {
                    // the word can be interpreted as an integer
                    we.IsUnknownWord = false;
                    we.UnknownType = "Number";
                }
                else if (double.TryParse(we.Text, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out dval))
                {
                    // the word can be interpreted as a decimal
                    we.IsUnknownWord = false;
                    we.UnknownType = "Number";
                }
            }
        }

        
    }
}
