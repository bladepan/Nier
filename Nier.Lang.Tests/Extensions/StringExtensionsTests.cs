using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Lang.Extensions;

namespace Nier.Lang.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        [DataRow(null, ".", 0, 1)]
        [DataRow("", ".", 0, 1)]
        [DataRow("abc", ".", 1, 3)]
        public void Abbreviate_DoesNotNeedAbbreviate_ReturnsOriginString(string str, string abbrevMarker, int offset, int maxWidth)
        {
            string result = str.Abbreviate(abbrevMarker, offset, maxWidth);
            Assert.AreEqual(result, str);
        }

        [TestMethod]
        [DataRow("abc", "", 0, 2, "ab")]
        [DataRow("abc", null, 0, 2, "ab")]
        [DataRow("abcd", "..", 1, 3, "a..")]
        public void Abbreviate_AbbreviateOnlyOnRightSide_ReturnsExpectedResult(string str, string abbrevMarker, int offset, int maxWidth, string expected)
        {
            string result = str.Abbreviate(abbrevMarker, offset, maxWidth);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("abcdefghijklmno", "...", 6, 10, "...ghij...")]
        [DataRow("abcdefg", "..", 3, 5, "..d..")]
        public void Abbreviate_AbbreviateOnBothSides_ReturnsExpectedResult(string str, string abbrevMarker, int offset, int maxWidth, string expected)
        {
            string result = str.Abbreviate(abbrevMarker, offset, maxWidth);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("abc", "...", -1, 10)]
        public void Abbreviate_InvalidOffset_ThrowsException(string str, string abbrevMarker, int offset, int maxWidth)
        {
            Assert.ThrowsException<ArgumentException>(() => _ = str.Abbreviate(abbrevMarker, offset, maxWidth));
        }

        [TestMethod]
        [DataRow("abcdef", ".", 6, 3)]
        [DataRow("abcdef", ".", 7, 3)]
        public void Abbreviate_OffsetExceedsLimit_ThrowsException(string str, string abbrevMarker, int offset, int maxWidth)
        {
            Assert.ThrowsException<ArgumentException>(() => _ = str.Abbreviate(abbrevMarker, offset, maxWidth));
        }

        [TestMethod]
        [DataRow("abcdef", ".", 1, 1)]
        [DataRow("abcdef", ".", 1, 0)]
        public void Abbreviate_MaxWidthLessThanRequired_ThrowsException(string str, string abbrevMarker, int offset, int maxWidth)
        {
            Assert.ThrowsException<ArgumentException>(() => _ = str.Abbreviate(abbrevMarker, offset, maxWidth));
        }

        [TestMethod]
        [DataRow("abcd", ".", 3, "a.d")]
        [DataRow("abcde", ".", 4, "ab.e")]
        [DataRow("abcde", null, 4, "abde")]
        public void AbbreviateMiddle_AbbreviateHappens_ReturnsExpectedResult(string str, string abbrevMarker, int maxWidth, string expected)
        {
            string result = str.AbbreviateMiddle(abbrevMarker, maxWidth);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("abc", ".", 4)]
        [DataRow("abcd", ".", 4)]
        [DataRow(null, ".", 4)]
        [DataRow("", ".", 4)]
        public void AbbreviateMiddle_AbbreviateDoesNotHappen_ReturnsOriginalString(string str, string abbrevMarker, int maxWidth)
        {
            string result = str.AbbreviateMiddle(abbrevMarker, maxWidth);
            Assert.AreSame(str, result);
        }

        [TestMethod]
        [DataRow("abc", ".", 2)]
        [DataRow("abc", ".", 1)]
        public void AbbreviateMiddle_InvalidMaxWidth_ThrowsException(string str, string abbrevMarker, int maxWidth)
        {
            Assert.ThrowsException<ArgumentException>(() => _ = str.AbbreviateMiddle(abbrevMarker, maxWidth));
        }

        [TestMethod]
        [DataRow(null, "a", "a")]
        [DataRow("a", null, "a")]
        [DataRow("a", "a", null)]
        public void SubStringBetween_InputIsNull_ReturnsNull(string str, string open, string close)
        {
            Assert.IsNull(str.SubStringBetween(open, close));
        }

        [TestMethod]
        [DataRow("a", "", "")]
        [DataRow("abc", "a", "")]
        public void SubStringBetween_InputIsEmpty_ReturnsEmpty(string str, string open, string close)
        {
            Assert.AreEqual(0, str.SubStringBetween(open, close).Length);
        }

        [TestMethod]
        [DataRow("abc", "d", "d")]
        [DataRow("", "a", "")]
        public void SubStringBetween_OpenNotFound_ReturnsNull(string str, string open, string close)
        {
            Assert.IsNull(str.SubStringBetween(open, close));
        }

        [TestMethod]
        [DataRow("abc", "a", "d")]
        [DataRow("", "", "a")]
        public void SubStringBetween_EndNotFound_ReturnsNull(string str, string open, string close)
        {
            Assert.IsNull(str.SubStringBetween(open, close));
        }

        [TestMethod]
        [DataRow("abc", "", "c", "ab")]
        [DataRow("abc", "a", "c", "b")]
        public void SubStringBetween_FoundSubString_ReturnsSubString(string str, string open, string close, string expectedResult)
        {
            Assert.AreEqual(expectedResult, str.SubStringBetween(open, close));
        }
    }
}
