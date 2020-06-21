using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections;

namespace Nier.Commons.Tests.Collections
{
    [TestClass]
    public class ListMultiDictionaryTests
    {
        [TestMethod]
        public void Add()
        {
            var dict = new ListMultiDictionary<int, int>();
            dict.Add(3, new[] {3, 4, 5});
            Assert.IsTrue(dict[3].SequenceEqual(new[] {3, 4, 5}));
        }

        [TestMethod]
        public void Add_DuplicateKey_ThrowsException()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};
            Assert.ThrowsException<ArgumentException>(() => dict.Add(3, new[] {3, 4, 5}));
        }

        [TestMethod]
        public void Clear()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};
            dict.Clear();
            Assert.AreEqual(0, dict.Count);
            Assert.IsFalse(dict.ContainsKey(3));

            // verify we can operate after Clear
            dict[3] = new[] {3, 4, 5};
            Assert.IsTrue(dict[3].SequenceEqual(new[] {3, 4, 5}));
        }

        [TestMethod]
        public void Contains()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};

            Assert.IsFalse(dict.Contains(new KeyValuePair<int, IReadOnlyCollection<int>>(4, new[] {1, 2})));
            Assert.IsFalse(dict.Contains(new KeyValuePair<int, IReadOnlyCollection<int>>(3, null)));
            Assert.IsFalse(dict.Contains(new KeyValuePair<int, IReadOnlyCollection<int>>(3, new[] {1, 2})));
            Assert.IsTrue(dict.Contains(new KeyValuePair<int, IReadOnlyCollection<int>>(3, new[] {1, 2, 3})));
        }

        [TestMethod]
        public void CopyTo_InvalidArgument_ThrowsException()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};

            Assert.ThrowsException<ArgumentNullException>(() => dict.CopyTo(null, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                dict.CopyTo(new KeyValuePair<int, IReadOnlyCollection<int>>[1], -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                dict.CopyTo(new KeyValuePair<int, IReadOnlyCollection<int>>[1], 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                dict.CopyTo(new KeyValuePair<int, IReadOnlyCollection<int>>[1], 1));
        }

        [TestMethod]
        public void CopyTo()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};
            var arr = new KeyValuePair<int, IReadOnlyCollection<int>>[1];
            dict.CopyTo(arr, 0);
            var kv = arr[0];
            Assert.AreEqual(3, kv.Key);
            Assert.IsTrue(kv.Value.SequenceEqual(new[] {1, 2, 3}));
        }

        [TestMethod]
        public void Remove()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};
            Assert.IsFalse(dict.Remove(new KeyValuePair<int, IReadOnlyCollection<int>>(3, new[] {1, 2})));
            Assert.IsFalse(dict.Remove(new KeyValuePair<int, IReadOnlyCollection<int>>(4, new[] {1, 2})));
            Assert.IsTrue(dict.ContainsKey(3));

            Assert.IsTrue(dict.Remove(new KeyValuePair<int, IReadOnlyCollection<int>>(3, new[] {1, 2, 3})));
            Assert.IsFalse(dict.ContainsKey(3));
        }

        [TestMethod]
        public void IsReadOnly()
        {
            var dict = new ListMultiDictionary<int, int>();
            Assert.IsFalse(dict.IsReadOnly);
        }

        [TestMethod]
        public void TryGetValue()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};
            Assert.IsFalse(dict.TryGetValue(4, out _));
            Assert.IsTrue(dict.TryGetValue(3, out var val));
            Assert.IsTrue(val.SequenceEqual(new[] {1, 2, 3}));
        }

        [TestMethod]
        public void Set()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}, [4] = new[] {1}};
            dict[3] = null;
            dict[4] = new int[0];
            Assert.IsFalse(dict.ContainsKey(3));
            Assert.IsFalse(dict.ContainsKey(4));
        }

        [TestMethod]
        public void Keys()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}, [4] = new[] {1}};
            var keys = dict.Keys;
            Assert.IsTrue(keys.Contains(3));
            Assert.IsTrue(keys.Contains(4));
            Assert.AreEqual(2, keys.Count);
        }

        [TestMethod]
        public void Values()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}, [4] = new[] {1}};
            var vals = dict.Values;
            Assert.IsTrue(vals.Contains(new[] {1, 2, 3}, Enumerables.SequenceEqualityComparer<int>()));
            Assert.IsTrue(vals.Contains(new[] {1}, Enumerables.SequenceEqualityComparer<int>()));
            Assert.AreEqual(2, vals.Count);
        }

        [TestMethod]
        public void Add_ValidInput_Returns1()
        {
            var dict = new ListMultiDictionary<int, int>();
            Assert.IsTrue(dict.Add(3, 1));
            Assert.IsTrue(dict.Add(3, 2));

            Assert.IsTrue(dict[3].SequenceEqual(new[] {1, 2}));
        }

        [TestMethod]
        public void AddAll()
        {
            var dict = new ListMultiDictionary<int, int>();
            Assert.IsTrue(dict.AddAll(3, new[] {1, 2}));
            Assert.IsFalse(dict.AddAll(3, new int[0]));
            Assert.IsTrue(dict.AddAll(3, new[] {3, 4}));

            Assert.IsTrue(dict[3].SequenceEqual(new[] {1, 2, 3, 4}));
        }

        [TestMethod]
        public void Remove_ValidInput_ReturnsDataChanged()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3}};
            Assert.IsTrue(dict.Remove(3, 1));
            Assert.IsFalse(dict.Remove(3, 4));
            Assert.IsFalse(dict.Remove(4, 2));

            Assert.IsTrue(dict[3].SequenceEqual(new[] {2, 3}));

            Assert.IsTrue(dict.Remove(3, 2));
            Assert.IsTrue(dict.Remove(3, 3));
            Assert.IsFalse(dict.ContainsKey(3));
        }

        [TestMethod]
        public void RemoveAll()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3, 4}};
            Assert.AreEqual(2, dict.RemoveAll(3, new[] {1, 2, 5}));
            Assert.IsTrue(dict[3].SequenceEqual(new[] {3, 4}));
            Assert.AreEqual(0, dict.RemoveAll(3, new[] {1, 2, 5}));
            Assert.AreEqual(2, dict.RemoveAll(3, new[] {3, 4}));

            Assert.IsFalse(dict.ContainsKey(3));
            Assert.AreEqual(0, dict.RemoveAll(3, new[] {1}));
        }

        [TestMethod]
        public void GetKeyValueEnumerator()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3, 4}, [4] = new[] {5}};
            var kvs = dict.GetKeyValueEnumerator().ToList();
            Assert.AreEqual(5, kvs.Count);
            Assert.IsTrue(kvs.Contains(new KeyValuePair<int, int>(3, 1)));
            Assert.IsTrue(kvs.Contains(new KeyValuePair<int, int>(4, 5)));
        }

        [TestMethod]
        public void Equals()
        {
            var dict1 = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3, 4}, [4] = new[] {5}};
            var dict2 = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3, 4}, [4] = new[] {5}};
            Assert.IsTrue(dict1.Equals(dict2));
            dict1.Remove(3, 4);
            Assert.IsFalse(dict1.Equals(dict2));
            dict1.Add(3, 4);
            Assert.IsTrue(dict1.Equals(dict2));
        }

        [TestMethod]
        public void ToString_ReturnsReadableString()
        {
            var dict = new ListMultiDictionary<int, int> {[3] = new[] {1, 2, 3, 4}, [4] = new[] {5}};
            Assert.AreEqual("ListMultiDictionary<Int32,Int32>{3=[1, 2, 3, 4], 4=[5]}", dict.ToString());
        }
    }
}
