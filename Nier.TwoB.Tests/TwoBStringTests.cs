using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nier.TwoB.Tests
{
    [TestClass]
    public class TwoBStringTests
    {
        private const int CachedNumberCount = 128;
        private readonly Random _random = new Random();

        [TestMethod]
        public void FromValue()
        {
            Assert.AreSame(TwoBString.Empty, TwoBString.FromValue(null));
            Assert.AreSame(TwoBString.Empty, TwoBString.FromValue(string.Empty));

            string notCachedvalue = "abc";
            Assert.AreNotSame(TwoBString.FromValue(notCachedvalue), TwoBString.FromValue(notCachedvalue));
            Assert.AreEqual(notCachedvalue, TwoBString.FromValue(notCachedvalue).ToString());

            string cachedValue = GenerateString(1024);
            Assert.AreSame(TwoBString.FromValue(cachedValue), TwoBString.FromValue(cachedValue));
            Assert.AreEqual(cachedValue, TwoBString.FromValue(cachedValue).ToString());

            Assert.AreNotSame(TwoBString.FromValue(cachedValue), TwoBString.FromValue(cachedValue, true));
        }

        [TestMethod]
        public void FromValue_Bool()
        {
            Assert.AreSame(TwoBString.True, TwoBString.FromValue(true));
            Assert.AreSame(TwoBString.False, TwoBString.FromValue(false));
        }

        [TestMethod]
        public void FromValue_Int()
        {
            for (int i = 0; i < CachedNumberCount; i++)
            {
                Assert.AreSame(TwoBString.FromValue(i), TwoBString.FromValue(i));
            }

            for (int i = CachedNumberCount; i < CachedNumberCount + 100; i++)
            {
                Assert.AreNotSame(TwoBString.FromValue(i), TwoBString.FromValue(i));
            }
        }

        [TestMethod]
        public void Equals_DifferentStringValue_ReturnsFalse()
        {
            string val1 = GenerateString(1020) + "abcd";
            string val2 = GenerateString(1020) + "1234";
            Assert.AreNotEqual(TwoBString.FromValue(val1), TwoBString.FromValue(val2));
        }

        private string GenerateString(int length)
        {
            byte[] bytes = new byte[length];
            _random.NextBytes(bytes);
            string str = Convert.ToBase64String(bytes);
            return str.Substring(0, length);
        }
    }
}
