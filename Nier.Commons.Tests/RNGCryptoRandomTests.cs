using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nier.Commons.Tests
{
    [TestClass]
    public class RNGCryptoRandomTests
    {
        private readonly IRandom _random = RNGCryptoRandom.Instance;

        [TestMethod]
        public void Next()
        {
            Assert.AreEqual(0, _random.Next(0));
            Assert.AreEqual(0, _random.Next(1));
            Assert.AreEqual(1, _random.Next(1, 1));
            Assert.AreEqual(1, _random.Next(1, 2));
            for (int i = 0; i < 1024; i++)
            {
                TestNext();
            }
        }

        private void TestNext()
        {
            int i = _random.Next();
            Assert.IsTrue(i >= 0, "real value {0}", i);
            i = _random.Next(42);
            Assert.IsTrue(i >= 0, "real value {0}", i);
            Assert.IsTrue(i < 42, "real value {0}", i);

            i = _random.Next(3, 42);
            Assert.IsTrue(i >= 3, "real value {0}", i);
            Assert.IsTrue(i < 42, "real value {0}", i);

            double d = _random.NextDouble();
            Assert.IsTrue(d >= 0.0, "real value {0}", d);
            Assert.IsTrue(d < 1.0, "real value {0}", d);
        }

        [TestMethod]
        public void Next_ArgumentOutOfBounds_ThrowsException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _random.Next(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _random.Next(5, 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _random.Next(-1, 2));
        }

        [TestMethod]
        public void Next_Bytes()
        {
            _random.NextBytes(new byte[32]);
        }
    }
}
