using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections;
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

        [TestMethod]
        public void GetDifference()
        {
            var left = new Dictionary<string, string>
            {
                {"left1", "leftVal1"},
                {"left2", "leftVal2"},
                {"common1", "commonVal1"},
                {"common2", "commonVal2"},
                {"diff1", "diffLeftVal1"},
                {"diff2", "diffLeftVal2"}
            };
            var right = new Dictionary<string, string>
            {
                {"right1", "rightVal1"},
                {"right2", "rightVal2"},
                {"common1", "commonVal1"},
                {"common2", "commonVal2"},
                {"diff1", "diffRightVal1"},
                {"diff2", "diffRightVal2"}
            };
            var diff = left.GetDifference(right);

            var expectedLeftOnly = new Dictionary<string, string> {{"left1", "leftVal1"}, {"left2", "leftVal2"}};
            var expectedRightOnly = new Dictionary<string, string> {{"right1", "rightVal1"}, {"right2", "rightVal2"}};
            var expectedCommon = new Dictionary<string, string> {{"common1", "commonVal1"}, {"common2", "commonVal2"}};
            var expectedDiffering =
                new Dictionary<string, IDictionaryValueDifference<string>>
                {
                    {"diff1", new ExpectedDictionaryValueDifference<string>("diffLeftVal1", "diffRightVal1")},
                    {"diff2", new ExpectedDictionaryValueDifference<string>("diffLeftVal2", "diffRightVal2")}
                };
            Assert.IsTrue(diff.EntriesOnlyOnLeft.IsEquivalentTo(expectedLeftOnly));
            Assert.IsTrue(diff.EntriesOnlyOnRight.IsEquivalentTo(expectedRightOnly));
            Assert.IsTrue(diff.EntriesInCommon.IsEquivalentTo(expectedCommon));
            Assert.IsTrue(diff.EntriesDiffering.IsEquivalentTo(expectedDiffering));
        }

        [TestMethod]
        public void GetDifference_LeftIsEmpty_EntriesOnlyOnRightIsSet()
        {
            IDictionary<string, string> left = null;
            IDictionary<string, string> right = new Dictionary<string, string>
            {
                {"right1", "rightVal1"}, {"right2", "rightVal2"}
            };

            var diff = left.GetDifference(right);

            Assert.IsTrue(right.IsEquivalentTo(diff.EntriesOnlyOnRight));
            Assert.AreEqual(0, diff.EntriesOnlyOnLeft.Count);
            Assert.AreEqual(0, diff.EntriesInCommon.Count);
            Assert.AreEqual(0, diff.EntriesDiffering.Count);
        }

        [TestMethod]
        public void GetDifference_RightIsEmpty_EntriesOnlyOnLeftIsSet()
        {
            IDictionary<string, string> left = new Dictionary<string, string>
            {
                {"right1", "rightVal1"}, {"right2", "rightVal2"}
            };
            IDictionary<string, string> right = null;

            var diff = left.GetDifference(right);

            Assert.IsTrue(left.IsEquivalentTo(diff.EntriesOnlyOnLeft));
            Assert.AreEqual(0, diff.EntriesOnlyOnRight.Count);
            Assert.AreEqual(0, diff.EntriesInCommon.Count);
            Assert.AreEqual(0, diff.EntriesDiffering.Count);
        }

        [TestMethod]
        public void GetDifference_BothAreEmpty_ReturnEmptyResult()
        {
            IDictionary<string, string> left = new Dictionary<string, string>();
            IDictionary<string, string> right = new Dictionary<string, string>();

            var diff = left.GetDifference(right);

            Assert.AreEqual(0, diff.EntriesOnlyOnLeft.Count);
            Assert.AreEqual(0, diff.EntriesOnlyOnRight.Count);
            Assert.AreEqual(0, diff.EntriesInCommon.Count);
            Assert.AreEqual(0, diff.EntriesDiffering.Count);
        }

        private static IEnumerable<object[]> EmptyDictionaryDiffTestData =>
            new[]
            {
                new[] {new Dictionary<string, string>(), new Dictionary<string, string>()},
                new[]
                {
                    new Dictionary<string, string> {{"common1", "commonVal1"}},
                    new Dictionary<string, string> {{"common1", "commonVal1"}}
                }
            };

        [DataTestMethod]
        [DynamicData(nameof(EmptyDictionaryDiffTestData))]
        public void IsEmpty_EmptyDictionaryDiff_ReturnTrue(IDictionary<string, string> left,
            IDictionary<string, string> right)
        {
            var diff = left.GetDifference(right);
            Assert.IsTrue(diff.IsEmpty());
        }

        private static IEnumerable<object[]> NonEmptyDictionaryDiffTestData =>
            new[]
            {
                new[] {new Dictionary<string, string> {{"left1", "leftVal1"}}, new Dictionary<string, string>()},
                new[] {new Dictionary<string, string>(), new Dictionary<string, string> {{"right1", "rightVal1"}}},
                new[]
                {
                    new Dictionary<string, string> {{"diff1", "diffLeftVal1"}},
                    new Dictionary<string, string> {{"diff1", "diffRightVal1"}}
                }
            };

        [DataTestMethod]
        [DynamicData(nameof(NonEmptyDictionaryDiffTestData))]
        public void IsEmpty_NoneEmptyDictionaryDiff_ReturnFalse(IDictionary<string, string> left,
            IDictionary<string, string> right)
        {
            var diff = left.GetDifference(right);

            Assert.IsFalse(diff.IsEmpty());
        }
    }

    internal class ExpectedDictionaryValueDifference<TVal> : IDictionaryValueDifference<TVal>
    {
        public TVal LeftValue { get; }
        public TVal RightValue { get; }

        public ExpectedDictionaryValueDifference(TVal leftValue, TVal rightValue)
        {
            LeftValue = leftValue;
            RightValue = rightValue;
        }
    }
}
