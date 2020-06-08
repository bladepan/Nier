using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Extensions;

namespace Nier.Commons.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void ToReadableString()
        {
            Assert.AreEqual("Dictionary<DateTimeOffset,IReadOnlyDictionary<String,Int32>>",
                (new Dictionary<DateTimeOffset, IReadOnlyDictionary<string, int>>()).GetType().ToReadableString());
            Assert.AreEqual("Int32[]",
                (new int[5]).GetType().ToReadableString());
            Assert.AreEqual("Dictionary<DateTimeOffset,IReadOnlyDictionary<String,Int32>>[,]",
                (new Dictionary<DateTimeOffset, IReadOnlyDictionary<string, int>>[3, 3]).GetType().ToReadableString());
            Assert.AreEqual("Tuple<Object,String,Dictionary<Int32,Int32>>",
                (new Tuple<object, string, Dictionary<int, int>>(null, null, null).GetType().ToReadableString()));
        }
    }
}
