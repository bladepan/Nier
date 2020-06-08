using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections;
using Nier.Commons.Collections.Extensions;

namespace Nier.Commons.Tests.Collections
{
    [TestClass]
    public class BiDirectionDictionaryTests
    {
        [TestMethod]
        public void Constructor_WithSourceDictionary_RetainsValue()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.IsTrue(dict.IsEquivalentTo(source));
            Assert.IsTrue(
                dict.ReverseDirection.IsEquivalentTo(new Dictionary<string, int> {{"11", 1}, {"12", 2}, {"13", 3}}));
        }

        [TestMethod]
        public void ReverseDirection()
        {
            var origin = new BiDirectionDictionary<int, string>();
            var reverse = origin.ReverseDirection;
            Assert.AreSame(reverse, origin.ReverseDirection);
            Assert.AreSame(origin, reverse.ReverseDirection);
            Assert.AreSame(reverse, reverse.ReverseDirection.ReverseDirection);
        }

        [TestMethod]
        public void Add_DuplicateKeyValue_ContentNotChanged()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.ThrowsException<ArgumentException>(() => dict.Add(new KeyValuePair<int, string>(1, "100")));
            Assert.ThrowsException<ArgumentException>(() => dict.Add(new KeyValuePair<int, string>(100, "11")));

            // assert nothing is changed
            Assert.IsTrue(dict.IsEquivalentTo(source));
            Assert.IsTrue(
                dict.ReverseDirection.IsEquivalentTo(new Dictionary<string, int> {{"11", 1}, {"12", 2}, {"13", 3}}));
        }

        [TestMethod]
        public void Clear()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            dict.Clear();
            Assert.AreEqual(0, dict.Count);
            Assert.AreEqual(0, dict.ReverseDirection.Count);
        }

        [TestMethod]
        public void Contains()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.IsTrue(dict.Contains(new KeyValuePair<int, string>(1, "11")));
            Assert.IsTrue(dict.ReverseDirection.Contains(new KeyValuePair<string, int>("11", 1)));

            Assert.IsFalse(dict.Contains(new KeyValuePair<int, string>(1, "12")));
            Assert.IsFalse(dict.Contains(new KeyValuePair<int, string>(2, "11")));
            Assert.IsFalse(dict.ReverseDirection.Contains(new KeyValuePair<string, int>("11", 2)));
            Assert.IsFalse(dict.ReverseDirection.Contains(new KeyValuePair<string, int>("12", 1)));
        }

        [TestMethod]
        public void CopyTo()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            var dest = new KeyValuePair<int, string>[3];
            dict.CopyTo(dest, 0);
            Assert.IsTrue(dict.IsEquivalentTo(new Dictionary<int, string>(dest)));
        }

        [TestMethod]
        public void Remove()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.IsFalse(dict.Remove(new KeyValuePair<int, string>(1, "12")));
            Assert.IsTrue(dict.Remove(new KeyValuePair<int, string>(1, "11")));

            Assert.IsTrue(dict.IsEquivalentTo(new Dictionary<int, string> {{2, "12"}, {3, "13"}}));
            Assert.IsTrue(dict.ReverseDirection.IsEquivalentTo(new Dictionary<string, int> {{"12", 2}, {"13", 3}}));

            // remove by key
            Assert.IsFalse(dict.Remove(4));
            Assert.IsTrue(dict.Remove(2));

            Assert.IsTrue(dict.IsEquivalentTo(new Dictionary<int, string> {{3, "13"}}));
            Assert.IsTrue(dict.ReverseDirection.IsEquivalentTo(new Dictionary<string, int> {{"13", 3}}));
        }

        [TestMethod]
        public void IsReadOnly()
        {
            var dict = new BiDirectionDictionary<int, string>();
            Assert.IsFalse(dict.IsReadOnly);
            Assert.IsFalse(dict.ReverseDirection.IsReadOnly);
        }

        [TestMethod]
        public void ContainsKey()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.IsTrue(dict.ContainsKey(1));
            Assert.IsTrue(dict.ReverseDirection.ContainsKey("11"));

            Assert.IsFalse(dict.ContainsKey(4));
            Assert.IsFalse(dict.ReverseDirection.ContainsKey("1"));
        }

        [TestMethod]
        public void TryGetValue()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);

            Assert.IsFalse(dict.TryGetValue(4, out _));
            Assert.IsTrue(dict.TryGetValue(1, out string value));
            Assert.AreEqual("11", value);
        }

        [TestMethod]
        public void GetIndex()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.AreEqual("11", dict[1]);
            Assert.ThrowsException<KeyNotFoundException>(() => dict[4]);
        }

        [TestMethod]
        public void SetIndex()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            dict[1] = "14";
            Assert.IsTrue(dict.IsEquivalentTo(new Dictionary<int, string> {{1, "14"}, {2, "12"}, {3, "13"}}));

            dict[4] = "15";
            Assert.IsTrue(
                dict.IsEquivalentTo(new Dictionary<int, string> {{1, "14"}, {2, "12"}, {3, "13"}, {4, "15"}}));

            dict[1] = "15";
            Assert.IsTrue(dict.IsEquivalentTo(new Dictionary<int, string> {{1, "15"}, {2, "12"}, {3, "13"}}));
        }

        [TestMethod]
        public void Keys()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            CollectionAssert.AreEquivalent(new[] {1, 2, 3}, dict.Keys.ToArray());
        }

        [TestMethod]
        public void Values()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            CollectionAssert.AreEquivalent(new[] {"11", "12", "13"}, dict.Values.ToArray());
        }

        [TestMethod]
        public void ToString_NoneEmpty_ReturnsStringsWithKeyValues()
        {
            var source = new Dictionary<int, string> {{1, "11"}, {2, "12"}, {3, "13"}};
            var dict = new BiDirectionDictionary<int, string>(source);
            Assert.AreEqual("BiDirectionDictionary<Int32,String>{1=11, 2=12, 3=13}", dict.ToString());
        }
    }
}
