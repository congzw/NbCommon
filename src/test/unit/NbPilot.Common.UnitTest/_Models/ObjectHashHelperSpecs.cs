// ReSharper disable CheckNamespace

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    [TestClass]
    public class ObjectHashHelperSpecs
    {
        private IObjectHashHelper _objectHashHelper = null;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _objectHashHelper = ObjectHashHelper.Resolve();
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            _objectHashHelper = null;
        }

        [TestMethod]
        public void CreateObjectHash_SameObject_Should_OK()
        {
            _objectHashHelper.CreateObjectHash("ABC").ShouldEqual(_objectHashHelper.CreateObjectHash("ABC"));
            _objectHashHelper.CreateObjectHash("ABc").ShouldEqual(_objectHashHelper.CreateObjectHash("ABc"));
            _objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABC" }).ShouldEqual(_objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABC" }));
        }

        [TestMethod]
        public void CreateObjectHash_NotEqualObject_Should_OK()
        {
            _objectHashHelper.CreateObjectHash("ABc").ShouldNotEqual(_objectHashHelper.CreateObjectHash("ABC"));
            _objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABc" }).ShouldNotEqual(_objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABC" }));
        }

        [TestMethod]
        public void VerifyObjectHash_EqualObject_Should_OK()
        {
            _objectHashHelper.VerifyObjectHash("ABC", _objectHashHelper.CreateObjectHash("ABC")).ShouldTrue();
            _objectHashHelper.VerifyObjectHash(new MockTraceItem() { Name = "ABC" }, _objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABC" })).ShouldTrue();
        }

        [TestMethod]
        public void VerifyObjectHash_NotEqualObject_Should_OK()
        {
            _objectHashHelper.CreateObjectHash("ABc").Log();
            _objectHashHelper.CreateObjectHash("ABC").Log();
            _objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABc" }).Log();
            _objectHashHelper.VerifyObjectHash("ABC", _objectHashHelper.CreateObjectHash("ABc")).ShouldFalse();
            _objectHashHelper.VerifyObjectHash(new MockTraceItem() { Name = "ABc" }, _objectHashHelper.CreateObjectHash(new MockTraceItem() { Name = "ABC" })).ShouldFalse();
        }
    }

    public class MockTraceItem
    {
        public MockTraceItem()
        {
            Items = new List<MockTraceItem>();
        }
        public string Name { get; set; }
        public IList<MockTraceItem> Items { get; set; }
    }
}
