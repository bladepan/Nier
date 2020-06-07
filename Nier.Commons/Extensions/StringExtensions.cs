using System;
using System.Text;

namespace Nier.Commons.Extensions
{
    /// <summary>
    /// Utility methods of <see cref="string"/> type.
    /// </summary>
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
                throw new ArgumentException($"Offset {offset} should be less than string length {strLen}.",
                    nameof(offset));
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
        /// Abbreviates a string to the length passed, replacing the middle characters with the supplied replacement string.
        ///
        /// The abbreviation happens when the string length is greater than the maxWidth.
        /// Otherwise, the original string is returned.
        /// </summary>
        /// <param name="str">the string to be abbreviated, can be null</param>
        /// <param name="abbrevMarker">the string to replace the middle characters with</param>
        /// <param name="maxWidth">maximum length of result string</param>
        /// <returns>the abbreviated string if abbreviation is necessary, or the original string supplied for abbreviation</returns>
        public static string AbbreviateMiddle(this string str, string abbrevMarker, int maxWidth)
        {
            int strLen = str?.Length ?? 0;
            int abbrevMarkerLength = abbrevMarker?.Length ?? 0;
            if (strLen <= maxWidth)
            {
                return str;
            }

            int minMaxLength = abbrevMarkerLength + 2;
            if (maxWidth < minMaxLength)
            {
                throw new ArgumentException($"Insufficient maxWidth {maxWidth}. Require at least {minMaxLength}",
                    nameof(maxWidth));
            }

            // from here maxWidth is positive. str is not null
            int characterLength = maxWidth - abbrevMarkerLength;
            int rightLength = characterLength / 2;
            int leftLength = characterLength - rightLength;
            return str.Substring(0, leftLength) + abbrevMarker + str.Substring(strLen - rightLength, rightLength);
        }

        /// <summary>
        /// Appends the suffix to the end of the string if the string does not
        /// already end with the suffix.
        /// </summary>
        /// <param name="str">The string, can be null</param>
        /// <param name="suffix">The suffix to append to the end of the string.</param>
        /// <param name="stringComparison">String comparison used to find suffix</param>
        /// <returns></returns>
        public static string AppendIfMissing(this string str, string suffix, StringComparison stringComparison)
        {
            if (string.IsNullOrEmpty(suffix) || (str != null && str.EndsWith(suffix, stringComparison)))
            {
                return str;
            }

            return str + suffix;
        }

        /// <summary>
        /// Centers a string in a larger string of size size. Uses a supplied character as the value to pad the string with.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="size"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        public static string Center(this string str, int size, char padChar = ' ')
        {
            int strLen = str?.Length ?? 0;
            int padSize = size - strLen;
            if (padSize <= 0)
            {
                return str;
            }

            int rightPadSize = padSize / 2;
            int leftPadSize = padSize - rightPadSize;
            return Pad(str, leftPadSize, rightPadSize, padChar);
        }

        /// <summary>
        /// Left pad a string with a specified character.
        /// The string is padded to the size of size.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="size"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        public static string PadStart(this string str, int size, char padChar = ' ')
        {
            int strLen = str?.Length ?? 0;
            int padSize = size - strLen;
            return padSize <= 0 ? str : Pad(str, padSize, 0, padChar);
        }

        /// <summary>
        /// Right pad a string with a specified character.
        /// The string is padded to the size of size.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="size"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        public static string PadEnd(this string str, int size, char padChar = ' ')
        {
            int strLen = str?.Length ?? 0;
            int padSize = size - strLen;
            return padSize <= 0 ? str : Pad(str, 0, padSize, padChar);
        }

        /// <summary>
        /// Pad string withs supplied character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startPadSize"></param>
        /// <param name="endPadSize"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string Pad(string str, int startPadSize, int endPadSize, char padChar)
        {
            if (startPadSize < 0)
            {
                throw new ArgumentException($"Invalid pad size {startPadSize}.", nameof(startPadSize));
            }

            if (endPadSize < 0)
            {
                throw new ArgumentException($"Invalid pad size {endPadSize}.", nameof(endPadSize));
            }

            if (startPadSize == 0 && endPadSize == 0)
            {
                return str;
            }

            int strLen = str?.Length ?? 0;
            StringBuilder sb = new StringBuilder(startPadSize + endPadSize + strLen);
            for (int i = 0; i < startPadSize; i++)
            {
                sb.Append(padChar);
            }

            if (strLen > 0)
            {
                sb.Append(str);
            }

            for (int i = 0; i < endPadSize; i++)
            {
                sb.Append(padChar);
            }

            return sb.ToString();
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
