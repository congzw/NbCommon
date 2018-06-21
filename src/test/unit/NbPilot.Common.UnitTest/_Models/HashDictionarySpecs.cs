using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
// ReSharper disable CheckNamespace

namespace NbPilot.Common
{
    [TestClass]
    public class HashDictionarySpecs
    {
        private HashDictionary _hashModel = null;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _hashModel = new HashDictionary();
            var mockTraceItemA = new MockTraceItem();
            var mockTraceItemB = new MockTraceItem();
            mockTraceItemB.Items.Add(new MockTraceItem());
            mockTraceItemB.Items.Add(new MockTraceItem());
            _hashModel.Add("A", mockTraceItemA);
            _hashModel.Add("B", mockTraceItemB);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            _hashModel = null;
        }

        [TestMethod]
        public void CheckAnyChanged_NotChange_Should_OK()
        {
            _hashModel.CheckAnyChanged().ShouldFalse();
        }

        [TestMethod]
        public void CheckAnyChanged_AnyChange_Should_OK()
        {
            _hashModel.GetValueAs<MockTraceItem>("A").Name = "Changed";
            _hashModel.CheckAnyChanged().ShouldTrue();
        }

        [TestMethod]
        public void CheckChanged_PropertyChange_Should_OK()
        {
            _hashModel.GetValueAs<MockTraceItem>("A").Name = "Changed";
            _hashModel.CheckChanged("A").ShouldTrue();
            _hashModel.CheckChanged("B").ShouldFalse();
        }

        [TestMethod]
        public void CheckChanged_PropertyReset_Should_OK()
        {
            _hashModel["A"] = new MockTraceItem();
            _hashModel.CheckChanged("A").ShouldFalse("A Reset Same");

            _hashModel["A"] = new MockTraceItem() { Name = "Foo" };
            _hashModel.CheckChanged("A").ShouldTrue("A Reset Diff");
            
            _hashModel["B"] = null;
            _hashModel.CheckChanged("B").ShouldTrue("B Reset NULL");
        }

        [TestMethod]
        public void CheckChanged_PropertyRemove_Should_OK()
        {
            _hashModel.Remove("A");
            _hashModel.CheckChanged("A").ShouldTrue("A Removed");
            _hashModel.CheckChanged("B").ShouldFalse("B Not Changed");
        }
    }
}
