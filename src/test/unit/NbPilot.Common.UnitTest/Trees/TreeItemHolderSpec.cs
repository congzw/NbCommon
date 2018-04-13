using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Trees
{
    [TestClass]
    public class TreeItemHolderSpec
    {
        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ToRelations_NullTree_Should_ThrowEx()
        {
            var treeItemHolder = new TreeItemHolder<MockTreeItem>();
            treeItemHolder.Value = null;
            treeItemHolder.ToRelations();
        }

        [TestMethod]
        public void ToRelations_EmptyTree_Should_OK()
        {
            var treeItemHolder = new TreeItemHolder<MockTreeItem>();
            treeItemHolder.Value = new MockTreeItem() { Name = "A" };
            var mockTreeItems = treeItemHolder.ToRelations();
            //mockTreeItems.LogProperties();
            mockTreeItems.Count.ShouldEqual(1);
            mockTreeItems[0].RelationCode.ShouldEqual("1");
        }

        [TestMethod]
        public void ToRelations_Default_TopCode_Should_OK()
        {
            var treeItemHolder = new TreeItemHolder<MockTreeItem>();
            treeItemHolder.Value = new MockTreeItem() { Name = "A" };
            for (int i = 1; i <= 3; i++)
            {
                var child = new TreeItemHolder<MockTreeItem>();
                child.Value = new MockTreeItem() { Name = "A." + i };
                treeItemHolder.Children.Add(child);
                for (int j = 1; j <= 3; j++)
                {
                    var cc = new TreeItemHolder<MockTreeItem>();
                    cc.Value = new MockTreeItem() { Name = child.Value.Name + j };
                    child.Children.Add(cc);
                }
            }
            var mockTreeItems = treeItemHolder.ToRelations();
            //mockTreeItems.LogProperties();
            mockTreeItems.Count.ShouldEqual(13);
            foreach (var mockTreeItem in mockTreeItems)
            {
                mockTreeItem.RelationCode.StartsWith("1").ShouldTrue();
            }
        }

        [TestMethod]
        public void ToRelations_0_TopCode_Should_OK()
        {
            var treeItemHolder = new TreeItemHolder<MockTreeItem>();
            treeItemHolder.Value = new MockTreeItem() { Name = "A" };
            for (int i = 1; i <= 3; i++)
            {
                var child = new TreeItemHolder<MockTreeItem>();
                child.Value = new MockTreeItem() { Name = "A." + i };
                treeItemHolder.Children.Add(child);
                for (int j = 1; j <= 3; j++)
                {
                    var cc = new TreeItemHolder<MockTreeItem>();
                    cc.Value = new MockTreeItem() { Name = child.Value.Name + j };
                    child.Children.Add(cc);
                }
            }
            var mockTreeItems = treeItemHolder.ToRelations(0);
            //mockTreeItems.LogProperties();
            foreach (var mockTreeItem in mockTreeItems)
            {
                mockTreeItem.RelationCode.StartsWith("0").ShouldTrue();
            }
        }
    }
}
