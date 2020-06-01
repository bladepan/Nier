using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Collections.Extensions;

namespace Nier.Collections.Tests.Extensions
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
        public void AreEquivalent_BothAreEmpty_ReturnsTrue(IDictionary<string, string> dict1,
            IDictionary<string, string> dict2)
        {
            Assert.IsTrue(dict1.AreEquivalent(dict2));
        }

        private static IEnumerable<object[]> DictionariesOfDifferentLengthTestData =>
            new[]
            {
                new[] {new Dictionary<string, string>(), new Dictionary<string, string> {{"key1", "val1"}}},
                new[] {null, new Dictionary<string, string> {{"key1", "val1"}}}
            };

        [DataTestMethod]
        [DynamicData(nameof(DictionariesOfDifferentLengthTestData))]
        public void AreEquivalent_DictionariesAreOfDifferentLength_ReturnsFalse(IDictionary<string, string> dict1,
            IDictionary<string, string> dict2)
        {
            Assert.IsFalse(dict1.AreEquivalent(dict2));
        }

        [TestMethod]
        public void AreEquivalent_DictionariesWithDifferentKeys_ReturnsFalse()
        {
            var dict1 = new Dictionary<string, string> {{"key1", "val1"}};
            var dict2 = new Dictionary<string, string> {{"key2", "val1"}};
            Assert.IsFalse(dict1.AreEquivalent(dict2));
        }

        [TestMethod]
        public void AreEquivalent_DictionariesWithDifferentValues_ReturnsFalse()
        {
            var dict1 = new Dictionary<string, string> {{"key1", "val1"}};
            var dict2 = new Dictionary<string, string> {{"key1", "val2"}};
            Assert.IsFalse(dict1.AreEquivalent(dict2));
        }

        [TestMethod]
        public void AreEquivalent_DictionariesSameKeyValues_ReturnsTrue()
        {
            var dict1 = new Dictionary<string, string> {{"key1", "val1"}};
            var dict2 = new Dictionary<string, string> {{"key1", "val1"}};
            Assert.IsTrue(dict1.AreEquivalent(dict2));
        }
    }
}
