using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nier.Commons.Tests
{
    [TestClass]
    public class ThreadLocalRandomTests
    {
        private readonly IRandom _random = ThreadLocalRandom.Instance;

        [TestMethod]
        public void Next()
        {
            int result = _random.Next();
            Assert.IsTrue(result >= 0);

            result = _random.Next(100);
            Assert.IsTrue(result < 100);

            result = _random.Next(42, 200);
            Assert.IsTrue(result >= 42);
            Assert.IsTrue(result < 200);
        }

        [TestMethod]
        public void NextBytes()
        {
            byte[] bytes = new byte[42];
            _random.NextBytes(bytes);
        }

        [TestMethod]
        public void Nex()
        {
            double result = _random.NextDouble();
            Assert.IsTrue(result >= 0.0);
            Assert.IsTrue(result < 1.0);
        }
    }
}
