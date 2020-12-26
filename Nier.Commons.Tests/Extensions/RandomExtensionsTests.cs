using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Commons.Extensions;

namespace Nier.Commons.Tests.Extensions
{
    [TestClass]
    public class RandomExtensionsTests
    {
        private static readonly char[] s_chars = {'a', 'b', 'c'};
        private readonly IRandom _random = RNGCryptoRandom.Instance;

        [TestMethod]
        public void RandomString()
        {
            Assert.AreSame(string.Empty, _random.RandomString(s_chars, 0));

            string s = _random.RandomString(s_chars, 16);
            Assert.AreEqual(16, s.Length);
            Assert.IsTrue(s.All(c => s_chars.Contains(c)), "string content {0}", s);
        }

        [TestMethod]
        public void RandomString_InvalidArgument_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RandomExtensions.RandomString(null, s_chars, 2));
            Assert.ThrowsException<ArgumentNullException>(() => _random.RandomString(null, 4));
            Assert.ThrowsException<ArgumentException>(() => _random.RandomString(Array.Empty<char>(), 4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _random.RandomString(s_chars, -1));
        }
    }
}
