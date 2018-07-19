using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    [TestClass]
    public class NbConstHolderSpec
    {
        [TestMethod]
        public void SetAndGet_Should_OK()
        {
            var instanceHolder = new NbConstHolder();
            var mockItem = new MockHolderItem();
            instanceHolder.Set(mockItem);

            var holderMockItem = instanceHolder.Get<MockHolderItem>();
            holderMockItem.ShouldNotNull("holderMockItem");
            holderMockItem.ShouldSame(mockItem);
            
            var holderMockItem2 = instanceHolder.Get<MockHolderItem>();
            holderMockItem2.ShouldSame(mockItem);
        }

        [TestMethod]
        public void NotSetAndGet_Should_OK()
        {
            var instanceHolder = new NbConstHolder();
            var mockHolderItemAuto = instanceHolder.Get<MockHolderItemB>();
            mockHolderItemAuto.ShouldNotNull("mockHolderItemAuto");

            var mockHolderItemAuto2 = instanceHolder.Get<MockHolderItemB>();
            mockHolderItemAuto2.ShouldSame(mockHolderItemAuto);
        }
    }

    #region mocks
    
    public class MockHolderItem
    {
        public string Foo = "Foo";
    }

    public class MockHolderItemB
    {
        public string Foo = "Foo";
    }

    #endregion
}
