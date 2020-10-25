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

            string value = GenerateString(1024);
            Assert.AreSame(TwoBString.FromValue(value), TwoBString.FromValue(value));
            Assert.AreEqual(value, TwoBString.FromValue(value).ToString());

            Assert.AreNotSame(TwoBString.FromValue(value), TwoBString.FromValue(value, true));
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
            for (int i = 0; i < CachedNumberCount + 5; i++)
            {
                Assert.AreSame(TwoBString.FromValue(i), TwoBString.FromValue(i));
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
