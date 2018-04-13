using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Trees
{
    [TestClass]
    public class HaveRelationCodeExtensionsSpec
    {
        [TestMethod]
        public void OrderByRelationCode_Should_OK()
        {
            //Name=A, RelationCode=1
            //Name=A.1, RelationCode=1.1
            //Name=A.11, RelationCode=1.1.1
            //Name=A.12, RelationCode=1.1.2
            //Name=A.13, RelationCode=1.1.3
            //Name=A.2, RelationCode=1.2
            //Name=A.21, RelationCode=1.2.1
            //Name=A.22, RelationCode=1.2.2
            //Name=A.23, RelationCode=1.2.3
            //Name=A.3, RelationCode=1.3
            //Name=A.31, RelationCode=1.3.1
            //Name=A.32, RelationCode=1.3.2
            //Name=A.33, RelationCode=1.3.3
            //Name=A.14, RelationCode=1.1.4
            //Name=A.33, RelationCode=1.2.4
            //Name=A.33, RelationCode=1.2.10
            //Name=A.33, RelationCode=1.2.11
            //Name=A.33, RelationCode=1.20.11
            //Name=A.33, RelationCode=1.20.12

            var mockTreeItems = MockTreeItem.CreateRelationsUnordered();
            mockTreeItems.Add(new MockTreeItem() { Name = "Foo.", RelationCode = "1.2.10" });
            mockTreeItems.Add(new MockTreeItem() { Name = "Foo.", RelationCode = "1.2.11" });
            mockTreeItems.Add(new MockTreeItem() { Name = "Foo.", RelationCode = "1.20.11" });
            mockTreeItems.Add(new MockTreeItem() { Name = "Foo.", RelationCode = "1.20.12" });
            var orderByRelationCode = mockTreeItems.OrderByRelationCode();
            orderByRelationCode.LogProperties();
            orderByRelationCode.First().Name.ShouldEqual("A");
        }
        [TestMethod]
        public void OrderByRelationCodeDescending_Should_OK()
        {
            //Name=A, RelationCode=1
            //Name=A.1, RelationCode=1.1
            //Name=A.11, RelationCode=1.1.1
            //Name=A.12, RelationCode=1.1.2
            //Name=A.13, RelationCode=1.1.3
            //Name=A.2, RelationCode=1.2
            //Name=A.21, RelationCode=1.2.1
            //Name=A.22, RelationCode=1.2.2
            //Name=A.23, RelationCode=1.2.3
            //Name=A.3, RelationCode=1.3
            //Name=A.31, RelationCode=1.3.1
            //Name=A.32, RelationCode=1.3.2
            //Name=A.33, RelationCode=1.3.3
            //Name=A.14, RelationCode=1.1.4
            //Name=A.33, RelationCode=1.2.4

            var mockTreeItems = MockTreeItem.CreateRelationsUnordered();
            var orderByRelationCode = mockTreeItems.OrderByRelationCodeDescending();
            orderByRelationCode.LogProperties();
            orderByRelationCode.Last().Name.ShouldEqual("A");
        }

        [TestMethod]
        public void GenerateNextRelationCode_Empty_Should_OK()
        {
            var mockTreeItems = new List<MockTreeItem>();
            //mockTreeItems.Add(new MockTreeItem());
            mockTreeItems.GenerateNextRelationCode("1").ShouldEqual("1.1");
        }

        [TestMethod]
        public void GenerateNextRelationCode_Skip_Should_OK()
        {
            var mockTreeItems = new List<MockTreeItem>();
            mockTreeItems.Add(new MockTreeItem() { Name = "A", RelationCode = "1" });
            mockTreeItems.Add(new MockTreeItem() { Name = "A", RelationCode = "1.2" });
            mockTreeItems.GenerateNextRelationCode("1").ShouldEqual("1.3");
        }

        [TestMethod]
        public void GenerateNextRelationCode_Should_OK()
        {
            //Name=A, RelationCode=1
            //Name=A.1, RelationCode=1.1
            //Name=A.11, RelationCode=1.1.1
            //Name=A.12, RelationCode=1.1.2
            //Name=A.13, RelationCode=1.1.3
            //Name=A.2, RelationCode=1.2
            //Name=A.21, RelationCode=1.2.1
            //Name=A.22, RelationCode=1.2.2
            //Name=A.23, RelationCode=1.2.3
            //Name=A.3, RelationCode=1.3
            //Name=A.31, RelationCode=1.3.1
            //Name=A.32, RelationCode=1.3.2
            //Name=A.33, RelationCode=1.3.3
            //Name=A.14, RelationCode=1.1.4
            //Name=A.33, RelationCode=1.2.4

            var mockTreeItems = MockTreeItem.CreateRelationsUnordered();
            mockTreeItems.GenerateNextRelationCode("1.2").ShouldEqual("1.2.5");
        }

        [TestMethod]
        public void GetMaxDeep_Should_OK()
        {
            //Name=A, RelationCode=1
            //Name=A.1, RelationCode=1.1
            //Name=A.11, RelationCode=1.1.1
            //Name=A.12, RelationCode=1.1.2
            //Name=A.13, RelationCode=1.1.3
            //Name=A.2, RelationCode=1.2
            //Name=A.21, RelationCode=1.2.1
            //Name=A.22, RelationCode=1.2.2
            //Name=A.23, RelationCode=1.2.3
            //Name=A.3, RelationCode=1.3
            //Name=A.31, RelationCode=1.3.1
            //Name=A.32, RelationCode=1.3.2
            //Name=A.33, RelationCode=1.3.3
            //Name=A.14, RelationCode=1.1.4
            //Name=A.33, RelationCode=1.2.4

            var mockTreeItems = MockTreeItem.CreateRelationsUnordered();
            mockTreeItems.GetMaxDeep(0).ShouldEqual(2);
            mockTreeItems.GetMaxDeep(1).ShouldEqual(3);
            mockTreeItems.GetMaxDeep(2).ShouldEqual(4);
            mockTreeItems.GetMaxDeep(3).ShouldEqual(5);
        }

        [TestMethod]
        public void GetCurrentDeep_Should_OK()
        {
            var mockTreeItem = new MockTreeItem() { Name = "", RelationCode = "1.1.1.1" };
            mockTreeItem.GetCurrentDeep(0).ShouldEqual(3);
            mockTreeItem.GetCurrentDeep(1).ShouldEqual(4);
            mockTreeItem.GetCurrentDeep(2).ShouldEqual(5);
        }

        [TestMethod]
        public void GetDotCount_Should_OK()
        {
            "1".GetDotCount().ShouldEqual(0);
            "1.1".GetDotCount().ShouldEqual(1);
            "1.1.1".GetDotCount().ShouldEqual(2);
            "1.1.1.1".GetDotCount().ShouldEqual(3);
            "1.1.1.1.".GetDotCount().ShouldEqual(4);
        }
    }
}
