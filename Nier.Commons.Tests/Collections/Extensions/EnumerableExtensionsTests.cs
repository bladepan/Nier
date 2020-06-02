using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections.Extensions;

namespace Nier.Commons.Tests.Collections.Extensions
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void ToReadableString_Null_ReturnsNullEnumerableString()
        {
            string[] strings = null;
            Assert.AreEqual("IEnumerable<String> null", strings.ToReadableString());
        }

        [TestMethod]
        public void ToReadableString_NotNull_ReturnsStringWithAllValues()
        {
            string[] strings = new[] {"1", "2"};
            Assert.AreEqual("String[]<String>[1, 2]", strings.ToReadableString());
        }
    }
}
