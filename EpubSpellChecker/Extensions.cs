using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace EpubSpellChecker
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a percentage of how similar 2 strings are, based on the levenshtein edit distance
        /// </summary>
        /// <param name="str">The first string</param>
        /// <param name="str2">The second string to compare to the first</param>
        /// <returns>A percentage [0-1] that determines the similarity</returns>
        public static float GetSimilarPercentage(this string str, string str2)
        {
            var dist = EditDistance(str, str2);
            if (str.Length > str2.Length)
                return (str.Length - dist) / (float)str.Length;
            else
                return (str2.Length - dist) / (float)str2.Length;
        }

        /// <SUMMARY>
        ///    Computes the Levenshtein Edit Distance between two enumerables.
        ///     http://blogs.msdn.com/b/toub/archive/2006/05/05/590814.aspx
        ///     
        /// </SUMMARY>
        /// <TYPEPARAM name="T">The type of the items in the enumerables.</TYPEPARAM>
        /// <PARAM name="x">The first enumerable.</PARAM>
        /// <PARAM name="y">The second enumerable.</PARAM>
        /// <RETURNS>The edit distance.</RETURNS>
        private static int EditDistance<T>(IEnumerable<T> x, IEnumerable<T> y)
            where T : IEquatable<T>
        {
            // Validate parameters
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");

            // Convert the parameters into IList instances
            // in order to obtain indexing capabilities
            IList<T> first = x as IList<T> ?? new List<T>(x);
            IList<T> second = y as IList<T> ?? new List<T>(y);

            // Get the length of both.  If either is 0, return
            // the length of the other, since that number of insertions
            // would be required.
            int n = first.Count, m = second.Count;
            if (n == 0) return m;
            if (m == 0) return n;

            // Rather than maintain an entire matrix (which would require O(n*m) space),
            // just store the current row and the next row, each of which has a length m+1,
            // so just O(m) space. Initialize the current row.
            int curRow = 0, nextRow = 1;
            int[][] rows = new int[][] { new int[m + 1], new int[m + 1] };
            for (int j = 0; j <= m; ++j) rows[curRow][j] = j;

            // For each virtual row (since we only have physical storage for two)
            for (int i = 1; i <= n; ++i)
            {
                // Fill in the values in the row
                rows[nextRow][0] = i;
                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] +
                        (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            // Return the computed edit distance
            return rows[curRow][m];
        }

        /// <summary>
        /// Checks if a string starts with a capital
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>True if the string starts with a capital, otherwise false</returns>
        public static bool StartsWithCapital(this string str)
        {
            return str.Length > 0 && char.IsUpper(str[0]);
        }

        /// <summary>
        /// Gets the proper case of a word (first letter capital, the rest lower)
        /// </summary>
        /// <param name="str">The string to make proper case</param>
        /// <returns>A version of the string that is properly cased</returns>
        public static string ProperCase(this string str)
        {
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.ToLower().Substring(1);
            else
                return str;
        }

        /// <summary>
        /// Returns a part of the array, similar to substring
        /// </summary>
        /// <typeparam name="T">The type of the objects of the array</typeparam>
        /// <param name="data">The array to take a segment out of</param>
        /// <param name="index">The start position of the segment</param>
        /// <param name="length">The length of the segment</param>
        /// <returns>A segment of the given array</returns>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Sets the value of double buffered property of the given datagridview
        /// </summary>
        /// <param name="dgv">The datagridview to change the doublebuffered property of</param>
        /// <param name="setting">The new value of the double buffered property</param>
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        /// <summary>
        /// Sets the value of double buffered property of the given listbox
        /// </summary>
        /// <param name="lst">The listbox to change the doublebuffered property of</param>
        /// <param name="setting">The new value of the double buffered property</param>
        public static void DoubleBuffered(this ListBox lst, bool setting)
        {
            Type dgvType = lst.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(lst, setting, null);
        }

        /// <summary>
        /// Returns all the nodes of an xml node list recursively (depth first)
        /// </summary>
        /// <param name="nl">The node list to fully iterate</param>
        /// <returns>A collection of nodes</returns>
        public static IEnumerable<XmlNode> GetAllNodes(this XmlNodeList nl)
        {
            foreach (var subn in nl.Cast<XmlNode>())
            {
                yield return subn;
                foreach (var subsubn in GetAllNodes(subn.ChildNodes))
                    yield return subsubn;
            }
        }

         /// <summary>
        /// Removes the namespaces from the xml, so it can be read with an XmlDocument without having to worry about included namespace references
        /// </summary>
        /// <param name="root">The root xml node</param>
        /// <returns>The xml node without any of the namespace references</returns>
        public static XElement StripNamespaces(this XElement root)
        {
            XElement res = new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => StripNamespaces(el)) :
                    (object)root.Value
            );

            res.ReplaceAttributes(
                root.Attributes().Where(attr => (!attr.IsNamespaceDeclaration)));

            return res;
        }

        /// <summary>
        /// Trims the suffix of a word (based on an apostrophe)
        /// </summary>
        /// <param name="word">The string to trim</param>
        /// <returns>A trimmed version of the string</returns>
        public  static string TrimSuffix(this string word)
        {
            int apostrapheLocation = word.IndexOf('\'');
            if (apostrapheLocation != -1)
            {
                word = word.Substring(0, apostrapheLocation);
            }

            return word;
        }

    }

}
