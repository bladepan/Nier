using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Collections;

namespace Nier.Commons.Tests.Collections
{
    [TestClass]
    public class MultiSetTests
    {
        [TestMethod]
        public void Add()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            Assert.AreEqual(0, multiSet.Add(null, 3));
            Assert.AreEqual(0, multiSet.Add(string.Empty, 4));
            multiSet.Add("ab");
            Assert.AreEqual(1, multiSet.Add("ab", 2));

            Assert.AreEqual(3, multiSet.Count(i => i == null));
            Assert.AreEqual(4, multiSet.Count(i => i == string.Empty));
            Assert.AreEqual(3, multiSet.Count(i => i == "ab"));
        }

        [TestMethod]
        public void GetItemCount()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add(null, 3);
            multiSet.Add("ab");

            Assert.AreEqual(3, multiSet.GetItemCount(null));
            Assert.AreEqual(1, multiSet.GetItemCount("ab"));
            Assert.AreEqual(0, multiSet.GetItemCount("abc"));
        }

        [TestMethod]
        public void SetItemCount()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("itemToIncrease", 3);
            multiSet.Add("itemToDecrease", 3);
            multiSet.Add("itemToDelete", 3);
            multiSet.Add("itemNotChanged", 3);

            Assert.AreEqual(3, multiSet.SetItemCount("itemToIncrease", 4));
            Assert.AreEqual(3, multiSet.SetItemCount("itemToDecrease", 2));
            Assert.AreEqual(3, multiSet.SetItemCount("itemToDelete", 0));
            Assert.AreEqual(3, multiSet.SetItemCount("itemNotChanged", 3));
            Assert.AreEqual(0, multiSet.SetItemCount("itemToAdd", 3));

            Assert.AreEqual(4, multiSet.Count(i => i == "itemToIncrease"));
            Assert.AreEqual(2, multiSet.Count(i => i == "itemToDecrease"));
            Assert.IsFalse(multiSet.Any(i => i == "itemToDelete"));
            Assert.AreEqual(3, multiSet.Count(i => i == "itemToAdd"));
            Assert.AreEqual(3, multiSet.Count(i => i == "itemNotChanged"));
        }

        [TestMethod]
        public void SetItemCount_ExpectedCountMisMatch_DoesNotUpdate()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("item", 3);

            Assert.IsFalse(multiSet.SetItemCount("item", 4, 5));
            Assert.IsFalse(multiSet.SetItemCount("item", 0, 5));

            Assert.IsFalse(multiSet.SetItemCount("itemNotExist", 1, 5));
            Assert.IsFalse(multiSet.SetItemCount("itemNotExist", 5, 5));

            Assert.AreEqual(3, multiSet.Count(i => i == "item"));
            Assert.IsFalse(multiSet.Any(i => i == "itemNotExist"));
        }

        [TestMethod]
        public void Remove()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("itemToDecrease", 3);
            multiSet.Add("itemToDelete", 3);
            multiSet.Add("itemToDelete_negative", 3);
            multiSet.Add("itemNotChanged", 3);

            Assert.AreEqual(3, multiSet.Remove("itemToDecrease", 1));
            Assert.IsTrue(multiSet.Remove("itemToDecrease"));
            Assert.AreEqual(3, multiSet.Remove("itemToDelete", 3));
            Assert.IsFalse(multiSet.Remove("itemToDelete"));
            Assert.AreEqual(3, multiSet.Remove("itemToDelete_negative", 4));
            Assert.AreEqual(3, multiSet.Remove("itemNotChanged", 0));
            Assert.AreEqual(0, multiSet.Remove("itemNotExist", 1));
            Assert.IsFalse(multiSet.Remove("itemNotExist"));

            Assert.AreEqual(1, multiSet.Count(i => i == "itemToDecrease"));
            Assert.AreEqual(3, multiSet.Count(i => i == "itemNotChanged"));
            Assert.IsFalse(multiSet.Any(i => i == "itemToDelete"));
            Assert.IsFalse(multiSet.Any(i => i == "itemToDelete_negative"));
            Assert.IsFalse(multiSet.Any(i => i == "itemNotExist"));
        }

        [TestMethod]
        public void ItemSet()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("item1", 3);
            multiSet.Add("item2", 3);
            multiSet.Add(null, 3);

            var itemSet = multiSet.ItemSet.ToArray();
            Assert.AreEqual(3, itemSet.Length);
            Assert.IsTrue(itemSet.Contains("item1"));
            Assert.IsTrue(itemSet.Contains("item2"));
            Assert.IsTrue(itemSet.Contains(null));
        }

        [TestMethod]
        public void EntrySet()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("item1", 3);
            multiSet.Add("item2", 3);
            multiSet.Add(null, 3);

            var itemSet = multiSet.EntrySet.ToArray();
            Assert.AreEqual(3, itemSet.Length);
            Assert.IsTrue(itemSet.Contains(new KeyValuePair<string, int>("item1", 3)));
            Assert.IsTrue(itemSet.Contains(new KeyValuePair<string, int>("item2", 3)));
            Assert.IsTrue(itemSet.Contains(new KeyValuePair<string, int>(null, 3)));
        }

        [TestMethod]
        public void Clear()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("item1", 3);
            multiSet.Add("item2", 3);
            multiSet.Add(null, 3);

            Assert.AreEqual(9, multiSet.Count);
            multiSet.Clear();
            Assert.AreEqual(0, multiSet.Count);
            Assert.AreEqual(0, multiSet.ToArray().Length);

            // still usable after clear
            multiSet.Add("item1", 3);
            Assert.AreEqual(3, multiSet.Count);
            Assert.AreEqual(3, multiSet.Count(i => i == "item1"));
        }

        [TestMethod]
        public void Contains()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("item1", 3);
            multiSet.Add("item2", 1);
            multiSet.Add(null, 3);

            Assert.IsTrue(multiSet.Contains("item1"));
            Assert.IsTrue(multiSet.Contains("item2"));
            Assert.IsTrue(multiSet.Contains(null));
            Assert.IsFalse(multiSet.Contains("itemNotExist"));
        }

        [TestMethod]
        public void ToString_ReturnsReadableString()
        {
            IMultiSet<string> multiSet = new MultiSet<string>();
            multiSet.Add("item1", 3);
            Assert.AreEqual("MultiSet<String>{Entries={item1=3}}", multiSet.ToString());
        }
    }
}
