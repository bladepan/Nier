using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nier.TwoB.Tests
{
    [TestClass]
    public class CharSequenceTests
    {
        private const int CachedNumberCount = 128;
        private readonly Random _random = new Random();

        [TestMethod]
        public void FromValue()
        {
            Assert.AreSame(CharSequence.Empty, CharSequence.FromValue(null));
            Assert.AreSame(CharSequence.Empty, CharSequence.FromValue(string.Empty));

            string value = GenerateString(1024);
            Assert.AreSame(CharSequence.FromValue(value), CharSequence.FromValue(value));
            Assert.AreEqual(value, CharSequence.FromValue(value).ToString());

            Assert.AreNotSame(CharSequence.FromValue(value), CharSequence.FromValue(value, true));
        }

        [TestMethod]
        public void FromValue_Bool()
        {
            Assert.AreSame(CharSequence.True, CharSequence.FromValue(true));
            Assert.AreSame(CharSequence.False, CharSequence.FromValue(false));
        }

        [TestMethod]
        public void FromValue_Int()
        {
            for (int i = 0; i < CachedNumberCount + 5; i++)
            {
                Assert.AreSame(CharSequence.FromValue(i), CharSequence.FromValue(i));
            }
        }

        [TestMethod]
        public void Equals_DifferentStringValue_ReturnsFalse()
        {
            string val1 = GenerateString(1020) + "abcd";
            string val2 = GenerateString(1020) + "1234";
            Assert.AreNotEqual(CharSequence.FromValue(val1), CharSequence.FromValue(val2));
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
