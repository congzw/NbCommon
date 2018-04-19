using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.AppData.Init
{
    [TestClass]
    public class InitDataContextSpec
    {
        [TestMethod]
        public void Read_ExistFile_Should_Return_OK()
        {
            InitDataContext initDataContext = new InitDataContext();
            var mockFileDbHelper = new MockFileDbHelper();
            initDataContext.FileDbHelper = mockFileDbHelper;
            
            var mockItems = initDataContext.Read<MockExistItem>();
            mockItems.ShouldNotNull();
            mockItems.Count.ShouldEqual(1);
        }

        [TestMethod]
        public void Read_NotExistFile_Should_Return_Empty()
        {
            InitDataContext initDataContext = new InitDataContext();
            var mockFileDbHelper = new MockFileDbHelper();
            initDataContext.FileDbHelper = mockFileDbHelper;

            var mockItems = initDataContext.Read<MockNotExistItem>();
            mockItems.ShouldNotNull();
            mockItems.Count.ShouldEqual(0);
        }
        
        [TestMethod]
        public void Save_Should_Replace()
        {
            InitDataContext initDataContext = new InitDataContext();
            var mockFileDbHelper = new MockFileDbHelper();
            initDataContext.FileDbHelper = mockFileDbHelper;

            var mockExistItems = new List<MockExistItem>();
            initDataContext.Save(mockExistItems);
            
            mockFileDbHelper.MockExistItems.ShouldNotNull();
            mockFileDbHelper.MockExistItems.Count.ShouldEqual(0);
        }
    }

    public class MockExistItem
    {
        public string Name { get; set; }
    }

    public class MockNotExistItem
    {
        public string Name { get; set; }
    }

    public class MockFileDbHelper : IFileDbHelper
    {
        internal IList<MockExistItem> MockExistItems = new List<MockExistItem>(); 
        public MockFileDbHelper()
        {
            MockExistItems.Add(new MockExistItem(){Name = "A"});
        }

        public IList<T> Read<T>(string path)
        {
            AssertHelper.WriteLine("read from path: " + path);
            if (typeof(T) == typeof(MockExistItem))
            {
                return MockExistItems.Cast<T>().ToList();
            }
            return new List<T>();
        }

        public void Save<T>(string path, IList<T> list)
        {
            AssertHelper.WriteLine("save to path: " + path);
            if (typeof(T) == typeof(MockExistItem))
            {
                MockExistItems = list.Cast<MockExistItem>().ToList();
            }
        }
    }
}
