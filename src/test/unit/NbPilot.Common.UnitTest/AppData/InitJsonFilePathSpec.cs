using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.AppData
{
    [TestClass]
    public class InitJsonFilePathSpec
    {
        [TestMethod]
        public void MakeInitDataFilePath_WithoutCustomizeName_Should_UseTypeName()
        {
            ITypeFilePathHelper typeFilePathHelper = new TypeFilePathHelper();
            typeFilePathHelper.AutoGuessTypeFilePath(@"a:\b\c", typeof(MockInitJsonFileItem)).ShouldEqual(@"a:\b\c\MockInitJsonFileItem.json");
            typeFilePathHelper.AutoGuessTypeFilePath(@"a:\b\c\", typeof(MockInitJsonFileItem)).ShouldEqual(@"a:\b\c\MockInitJsonFileItem.json");
            typeFilePathHelper.AutoGuessTypeFilePath(@"a:\", typeof(MockInitJsonFileItem)).ShouldEqual(@"a:\MockInitJsonFileItem.json");

            typeFilePathHelper.AutoGuessTypeFilePath(@"a\b\c", typeof(MockInitJsonFileItem)).ShouldEqual(@"a\b\c\MockInitJsonFileItem.json");
            typeFilePathHelper.AutoGuessTypeFilePath(@"a\b\c\", typeof(MockInitJsonFileItem)).ShouldEqual(@"a\b\c\MockInitJsonFileItem.json");
        }

        [TestMethod]
        public void MakeInitDataFilePath_WithCustomizeName_Should_UseIt()
        {
            ITypeFilePathHelper typeFilePathHelper = new TypeFilePathHelper();
            typeFilePathHelper.AutoGuessTypeFilePath(@"a:\b\c", typeof(MockInitJsonFileItem), "x").ShouldEqual(@"a:\b\c\x.json");
            typeFilePathHelper.AutoGuessTypeFilePath(@"a:\b\c\", typeof(MockInitJsonFileItem),"x").ShouldEqual(@"a:\b\c\x.json");
            typeFilePathHelper.AutoGuessTypeFilePath(@"a:\", typeof(MockInitJsonFileItem), "x").ShouldEqual(@"a:\x.json");

            typeFilePathHelper.AutoGuessTypeFilePath(@"a\b\c", typeof(MockInitJsonFileItem), "x").ShouldEqual(@"a\b\c\x.json");
            typeFilePathHelper.AutoGuessTypeFilePath(@"a\b\c\", typeof(MockInitJsonFileItem), "x").ShouldEqual(@"a\b\c\x.json");
        }
    }

    public class MockInitJsonFileItem
    {

    }
}
