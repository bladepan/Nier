using System;

namespace Nier.Lang.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Abbreviates a String using ellipses. This will turn
        /// "Now is the time for all good men" into "Now is the time for..."
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static string Abbreviate(this string str, int maxWidth)
        {
            return Abbreviate(str, "...", 0, maxWidth);
        }

        /// <summary>
        /// Abbreviates a String using another given String as replacement marker. This will turn
        /// "Now is the time for all good men" into "Now is the time for..." if "..." was defined
        /// as the replacement marker.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="abbrevMarker"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static string Abbreviate(this string str, string abbrevMarker, int maxWidth)
        {
            return Abbreviate(str, abbrevMarker, 0, maxWidth);
        }

        /// <summary>
        /// Abbreviates a String using a given replacement marker. This will turn
        /// Now is the time for all good men" into "...is the time for..." if "..." was defined
        /// as the replacement marker.
        /// <code>
        ///  StringExtensions.Abbreviate(null, ".", 0, 3) -> null
        ///  StringExtensions.Abbreviate("", ".", 0, 3) -> ""
        ///  StringExtensions.Abbreviate("abc", ".", 0, 3) -> "abc"
        ///  StringExtensions.Abbreviate("abc", ".", 0, 2) -> "ab."
        ///  StringExtensions.Abbreviate("abcdefg", "..", 3, 5) -> "..d.."
        ///
        ///  StringExtensions.Abbreviate("abcdefg", "..", 3, 2) -> ArgumentException
        ///  StringExtensions.Abbreviate("abcdefg", "..", -1, 5) -> ArgumentException
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to check, can be null</param>
        /// <param name="abbrevMarker">the string used as replacement marker</param>
        /// <param name="offset">the character on the offset is guaranteed to be kept in the result string. if offset is greater than abbrevMarker length, the characters
        /// on the left of offset will be abbreviated in result.</param>
        /// <param name="maxWidth">maximum length of the result string</param>
        /// <returns>Abbreviated string</returns>
        /// <exception cref="ArgumentException">offset less than 0, maxWidth less or equal to abbrevMarker length</exception>
        public static string Abbreviate(this string str, string abbrevMarker, int offset, int maxWidth)
        {
            if (offset < 0)
            {
                throw new ArgumentException($"Invalid offset value {offset}. Should not be negative.", nameof(offset));
            }

            int strLen = str?.Length ?? 0;

            if (maxWidth >= strLen)
            {
                return str;
            }

            int abbrevMarkerLength = abbrevMarker?.Length ?? 0;
            int minAbbrevWidth = abbrevMarkerLength + 1;
            if (maxWidth < minAbbrevWidth)
            {
                throw new ArgumentException(
                    $"Insufficient width for abbreviation. Require at least {minAbbrevWidth}, provided {maxWidth}.",
                    nameof(maxWidth));
            }

            if (offset >= strLen)
            {
                throw new ArgumentException($"Offset {offset} should be less than string length {strLen}.", nameof(offset));
            }

            // from this point on, str can't be null.
            // do not have enough space to abbrev from the left. keep all chars on the left side
            if (offset <= abbrevMarkerLength)
            {
                return str.Substring(0, maxWidth - abbrevMarkerLength) + abbrevMarker;
            }

            // replace chars before offset with abbrevMarker.
            // do not have space to abbrev from the right. keep all chars on the right side
            if (offset + maxWidth - abbrevMarkerLength > strLen)
            {
                return abbrevMarker + str.Substring(offset, strLen - offset);
            }

            // need to chop off characters on the right side too
            return abbrevMarker + str.Substring(offset, maxWidth - 2 * abbrevMarkerLength) + abbrevMarker;
        }

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
