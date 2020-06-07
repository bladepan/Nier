using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections.Extensions;

namespace Nier.Commons.Tests.Collections.Extensions
{
    [TestClass]
    public class DictionaryDifferenceExtensionsTests
    {
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
}
