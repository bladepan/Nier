using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Lang.Extensions;

namespace Nier.Lang.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
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
