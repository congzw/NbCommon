// ReSharper disable CheckNamespace

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbPilot.Common.Serialize;

namespace NbPilot.Common
{
    [TestClass]
    public class DynamicHashDictionarySpecs
    {
        private DynamicHashDictionary _dynamicHashModel = null;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _dynamicHashModel = new DynamicHashDictionary();
            var dynamicHashModel = _dynamicHashModel.AsDynamic();

            var mockTraceItemA = new MockTraceItem();
            dynamicHashModel.A = mockTraceItemA;
            
            var mockTraceItemB = new MockTraceItem();
            mockTraceItemB.Items.Add(new MockTraceItem());
            mockTraceItemB.Items.Add(new MockTraceItem());
            dynamicHashModel.B = mockTraceItemB;
            
            var mockTraceItemC = new MockTraceItem();
            dynamicHashModel.C = mockTraceItemC;
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
            var dynamicHashModel = _dynamicHashModel.AsDynamic();
            dynamicHashModel.A.Name = "Changed";
            _dynamicHashModel.CheckAnyChanged().ShouldTrue(); //same logic => ((bool)dynamicHashModel.CheckAnyChanged()).ShouldTrue();
        }

        [TestMethod]
        public void CheckChanged_PropertyChange_Should_OK()
        {
            var dynamicHashModel = _dynamicHashModel.AsDynamic();
            dynamicHashModel.A.Name = "Changed";

            _dynamicHashModel.CheckChanged("A").ShouldTrue();
            _dynamicHashModel.CheckChanged("B").ShouldFalse();
        }

        [TestMethod]
        public void CheckChanged_NotExistPropertyChange_Should_False()
        {
            var dynamicHashModel = _dynamicHashModel.AsDynamic();
            object notExist = dynamicHashModel.NotExist;
            notExist.ShouldNull();
            _dynamicHashModel.CheckChanged("NotExist").ShouldFalse();
        }
        
        [TestMethod]
        public void CheckChanged_PropertyRemove_Should_OK()
        {
            var dynamicHashModel = _dynamicHashModel.AsDynamic();
            dynamicHashModel.A = null;
            _dynamicHashModel.CheckChanged("A").ShouldTrue("A Removed");
            _dynamicHashModel.CheckChanged("B").ShouldFalse("B Not Changed");
        }

        [TestMethod]
        public void CheckChanged_Serialize_Should_OK()
        {
            var nbJsonSerialize = NbJsonSerialize.Resolve();

            var dynamicHashModel = _dynamicHashModel.AsDynamic();

            var httpGetJson = (string)nbJsonSerialize.Serialize(dynamicHashModel);
            AssertHelper.WriteLine("--httpGetJson--");
            httpGetJson.Log();

            var vo = nbJsonSerialize.Deserialize<DynamicHashDictionary>(httpGetJson);
            var voDynamic = vo.AsDynamic();
            voDynamic.A = new MockTraceItem() { Name = "Foo" };
            voDynamic.C = new MockTraceItem();
            voDynamic.D = new MockTraceItem();

            var httpPostJson = (string)nbJsonSerialize.Serialize(voDynamic);
            AssertHelper.WriteLine("--httpPostJson--");
            httpPostJson.Log();

            var serverGetVo = nbJsonSerialize.Deserialize<DynamicHashDictionary>(httpPostJson);
            var dynamicServerGetVo = serverGetVo.AsDynamic();
            var serverGetVoJson = (string)nbJsonSerialize.Serialize(dynamicServerGetVo);
            AssertHelper.WriteLine("--serverGetVoJson--");
            serverGetVoJson.Log();

            serverGetVo.CheckChanged("A").ShouldTrue("A Reset Foo");
            serverGetVo.CheckChanged("B").ShouldFalse("B NotRest");
            serverGetVo.CheckChanged("C").ShouldFalse("C Rest Same");
            serverGetVo.CheckChanged("D").ShouldTrue("Add NEW D");
        }
    }
}
