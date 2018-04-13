using System.Collections.Generic;
using System.Linq;

namespace NbPilot.Common.Trees
{
    public class MockTreeItem : IHaveRelationCode
    {
        public string Name { get; set; }
        public string RelationCode { get; set; }

        public static IList<MockTreeItem> CreateMocksWithRelations()
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
            return treeItemHolder.ToRelations(1);
        }

        public static IList<MockTreeItem> CreateRelationsUnordered()
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
            var mockTreeItems = treeItemHolder.ToRelations(1);

            //Name=A.14, RelationCode=1.1.4
            //Name=A.24, RelationCode=1.2.4
            mockTreeItems.Add(new MockTreeItem() { Name = "A.14", RelationCode = "1.1.4" });
            mockTreeItems.Add(new MockTreeItem() { Name = "A.24", RelationCode = "1.2.4" });
            return mockTreeItems;
        }
    }
}
