using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Supports
{
    [TestClass]
    public class ProductCodeContextSpec
    {
        [TestMethod]
        public void Match_Query_Empty_Or_Star_Should_Match()
        {
            var productCodeContext = new ProductSupportContext();
            productCodeContext.CurrentProductCode = "PuJiao";

            productCodeContext.Match("").ShouldTrue();
            productCodeContext.Match(" ").ShouldTrue();
            productCodeContext.Match("*").ShouldTrue();
            productCodeContext.Match("A,*,B").ShouldTrue();
            productCodeContext.Match(" A, * ,B ").ShouldTrue();
        }

        [TestMethod]
        public void Match_Query_Same_Should_Match()
        {
            var productCodeContext = new ProductSupportContext();
            productCodeContext.CurrentProductCode = "PuJiao";

            productCodeContext.Match("PuJiao").ShouldTrue();
            productCodeContext.Match("PuJiao ").ShouldTrue();
            productCodeContext.Match(" PuJiao ").ShouldTrue();
        }

        [TestMethod]
        public void Match_Query_UnknowCode_Should_NoMatch()
        {
            var productCodeContext = new ProductSupportContext();
            productCodeContext.CurrentProductCode = "PuJiao";

            productCodeContext.Match("GaoJiao").ShouldFalse();
            productCodeContext.Match("PuJiaoA").ShouldFalse();
            productCodeContext.Match("Pu").ShouldFalse();
            productCodeContext.Match("A, , C").ShouldFalse();
        }
    }
}
