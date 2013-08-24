using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    class XmlHelper
    {
        /// <summary>
        /// Gets the unescaped character of an xml escaped character
        /// </summary>
        /// <param name="escaped">The xml escaped character (e.g &apos;)</param>
        /// <returns>The unescaped version of the given escaped character</returns>
        public static string GetUnescapedXmlCharacter(string escaped)
        {
            string unescaped;
            if (escaped == "&nbsp;")
                unescaped = " ";
            else if (escaped == "&quot;")
                unescaped = "\"";
            else if (escaped == "&amp;")
                unescaped = "&";
            else if (escaped == "&ldquo;" || escaped == "&rdquo;")
                unescaped = "\"";
            else if (escaped == "&lsquo;" || escaped == "&rsquo;")
                unescaped = "'";
            else if (escaped == "&mdash;")
                unescaped = "—";
            else if (escaped == "&ndash;")
                unescaped = "–";
            else if (escaped == "&hellip;")
                unescaped = "…";
            else if (escaped == "&apos;")
                unescaped = "'";
            else if (escaped.StartsWith("&#"))
            {
                if (escaped[2] == 'x')
                {
                    string hex = escaped.Substring(3, escaped.Length - 4);
                    int value;
                    if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out value))
                        unescaped = char.ConvertFromUtf32(value);
                    else
                        unescaped = "";
                }
                else
                {
                    string valstr = escaped.Substring(2, escaped.Length - 3);
                    int value;
                    if (int.TryParse(valstr, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out value))
                        unescaped = char.ConvertFromUtf32(value);
                    else
                        unescaped = "";
                }
            }
            else
                unescaped = "";

            return unescaped;
        }
    }
}
