using System;

namespace Nier.Lang.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the String that is nested in between two Strings. Only the first match is returned.
        /// Uses StringComparison.Ordinal for string comparison.
        ///
        /// A null input String returns null.
        /// A null open/close returns null (no match).
        /// An empty ("") open and close returns an empty string.
        ///
        /// <code>
        ///  StringExtensions.SubStringBetween(null, "a", "a") -> null
        /// </code>
        /// </summary>
        /// <param name="str">the String containing the substring, may be null</param>
        /// <param name="open">the String before the substring, may be null</param>
        /// <param name="close">the String after the substring, may be null</param>
        /// <returns>the substring, null if no match</returns>
        public static string SubStringBetween(this string str, string open, string close)
        {
            if (str == null || open == null || close == null)
            {
                return null;
            }

            int start = str.IndexOf(open, StringComparison.Ordinal);
            if (start < 0)
            {
                return null;
            }

            int endStart = start + open.Length;
            int end = str.IndexOf(close, endStart, StringComparison.Ordinal);
            if (end < 0)
            {
                return null;
            }

            int subStrLen = end - endStart;
            return subStrLen >= 0 ? str.Substring(endStart, subStrLen) : null;
        }
    }
}
