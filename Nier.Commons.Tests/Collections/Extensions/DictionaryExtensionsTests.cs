using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections.Extensions;

namespace Nier.Commons.Tests.Collections.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        private static IEnumerable<object[]> EmptyDictionaryTestData =>
            new[]
            {
                new[] {new Dictionary<string, string>(), null}, new[] {null, new Dictionary<string, string>()},
                new[] {new Dictionary<string, string>(), new Dictionary<string, string>()}
            };

        [DataTestMethod]
        [DynamicData(nameof(EmptyDictionaryTestData))]
        public void IsEquivalentTo_BothAreEmpty_ReturnsTrue(IDictionary<string, string> dict1,
            IDictionary<string, string> dict2)
        {
            Assert.IsTrue(dict1.IsEquivalentTo(dict2));
        }

        private static IEnumerable<object[]> DictionariesOfDifferentLengthTestData =>
            new[]
            {
                new[] {new Dictionary<string, string>(), new Dictionary<string, string> {{"key1", "val1"}}},
                new[] {null, new Dictionary<string, string> {{"key1", "val1"}}}
            };

        [DataTestMethod]
        [DynamicData(nameof(DictionariesOfDifferentLengthTestData))]
        public void IsEquivalentTo_DictionariesAreOfDifferentLength_ReturnsFalse(IDictionary<string, string> dict1,
            IDictionary<string, string> dict2)
        {
            Assert.IsFalse(dict1.IsEquivalentTo(dict2));
        }

        [TestMethod]
        public void IsEquivalentTo_DictionariesWithDifferentKeys_ReturnsFalse()
        {
            var dict1 = new Dictionary<string, string> {{"key1", "val1"}};
            var dict2 = new Dictionary<string, string> {{"key2", "val1"}};
            Assert.IsFalse(dict1.IsEquivalentTo(dict2));
        }

        [TestMethod]
        public void IsEquivalentTo_DictionariesWithDifferentValues_ReturnsFalse()
        {
            var dict1 = new Dictionary<string, string> {{"key1", "val1"}};
            var dict2 = new Dictionary<string, string> {{"key1", "val2"}};
            Assert.IsFalse(dict1.IsEquivalentTo(dict2));
        }

        [TestMethod]
        public void IsEquivalentTo_DictionariesSameKeyValues_ReturnsTrue()
        {
            var dict1 = new Dictionary<string, string> {{"key1", "val1"}};
            var dict2 = new Dictionary<string, string> {{"key1", "val1"}};
            Assert.IsTrue(dict1.IsEquivalentTo(dict2));
        }

        [TestMethod]
        public void ToReadableString_NullValue_ReturnsNullDictionaryString()
        {
            Dictionary<string, int> dict = null;
            Assert.AreEqual("IDictionary<String,Int32> null", dict.ToReadableString());
        }

        [TestMethod]
        public void ToReadableString_NoneEmptyDictionary_ReturnsStringWithKeyValues()
        {
            Dictionary<string, int> dict = new Dictionary<string, int> {{"key1", 1}, {"key2", 2}};
            Assert.AreEqual("Dictionary<String,Int32>{key1=1, key2=2}", dict.ToReadableString());
        }
    }
}
