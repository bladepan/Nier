using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nier.Lang.Extensions;

namespace Nier.Lang.Tests.Extensions
{
    internal class ToStringBuilderTestObj
    {
        public override string ToString()
        {
            return this.ToStringBuilder().Add("char", 'c').Add("int", 42).ToString();
        }
    }

    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void ToStringBuilder()
        {
            var obj = new ToStringBuilderTestObj();
            Assert.AreEqual("ToStringBuilderTestObj{char=c, int=42}", obj.ToString());
        }
    }
}
