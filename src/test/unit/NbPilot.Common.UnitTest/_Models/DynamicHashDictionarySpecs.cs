// ReSharper disable CheckNamespace

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    [TestClass]
    public class DynamicHashDictionarySpecs
    {
        private DynamicHashDictionary _dynamicHashModel = null;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            var mockTraceItemA = new MockTraceItem();
            var mockTraceItemB = new MockTraceItem();
            mockTraceItemB.Items.Add(new MockTraceItem());
            mockTraceItemB.Items.Add(new MockTraceItem());

            _dynamicHashModel = DynamicHashDictionary.Create();
            var dynamicHashModel = AsDynamic();
            dynamicHashModel.A = mockTraceItemA;
            dynamicHashModel.B = mockTraceItemB;
        }

        private dynamic AsDynamic()
        {
            return _dynamicHashModel;
        }


        [TestCleanup()]
        public void MyTestCleanup()
        {
            _dynamicHashModel = null;
        }

        [TestMethod]
        public void CheckAnyChanged_NotChange_Should_OK()
        {
            _dynamicHashModel.CheckAnyChanged().ShouldFalse();
        }
        
        [TestMethod]
        public void CheckAnyChanged_AnyChange_Should_OK()
        {
            var dynamicHashModel = AsDynamic();
            dynamicHashModel.A.Name = "Changed";
            _dynamicHashModel.CheckAnyChanged().ShouldTrue(); //same logic => ((bool)dynamicHashModel.CheckAnyChanged()).ShouldTrue();
        }

        [TestMethod]
        public void CheckChanged_PropertyChange_Should_OK()
        {
            var dynamicHashModel = AsDynamic();
            dynamicHashModel.A.Name = "Changed";

            _dynamicHashModel.CheckChanged("A").ShouldTrue();
            _dynamicHashModel.CheckChanged("B").ShouldFalse();
        }

        [TestMethod]
        public void CheckChanged_NotExistPropertyChange_Should_False()
        {
            var dynamicHashModel = AsDynamic();
            object notExist = dynamicHashModel.NotExist;
            notExist.ShouldNull();
            _dynamicHashModel.CheckChanged("NotExist").ShouldFalse();
        }
    }
}
