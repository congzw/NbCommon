using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using NbPilot.Common.Serialize;

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
            _hashModel.Add("A", mockTraceItemA);

            var mockTraceItemB = new MockTraceItem();
            mockTraceItemB.Items.Add(new MockTraceItem());
            mockTraceItemB.Items.Add(new MockTraceItem());
            _hashModel.Add("B", mockTraceItemB);

            var mockTraceItemC = new MockTraceItem();
            _hashModel.Add("C", mockTraceItemC);
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
        

        [TestMethod]
        public void CheckChanged_Serialize_Should_OK()
        {
            var nbJsonSerialize = NbJsonSerialize.Resolve();
            var httpGetJson = nbJsonSerialize.Serialize(_hashModel);
            AssertHelper.WriteLine("--httpGetJson--");
            httpGetJson.Log();

            var vo = nbJsonSerialize.Deserialize<HashDictionary>(httpGetJson);
            vo["A"] = new MockTraceItem() { Name = "Foo" };
            vo.CheckChanged("A").ShouldTrue("A Reset Foo");
            
            vo.GetCurrentVersion().ShouldNotNull("GetCurrentVersion").Log();
            vo.GetHashValues()["B"].Version.ShouldNotNull("B NotRest hash Version").Log();

            vo.CheckChanged("B").ShouldFalse("B NotRest");

            vo["C"] = new MockTraceItem();
            vo.CheckChanged("C").ShouldFalse("C Rest Same");

            vo["D"] = new MockTraceItem();
            vo.GetHashValues()["D"].LogProperties();
            vo.GetCurrentVersion().Log();
            vo.CheckChanged("D").ShouldTrue("Add NEW D");
            
            var httpPostJson = nbJsonSerialize.Serialize(vo);
            AssertHelper.WriteLine("--httpPostJson--");
            httpPostJson.Log();

            var serverGetVo = nbJsonSerialize.Deserialize<HashDictionary>(httpPostJson);
            var serverGetVoJson = nbJsonSerialize.Serialize(serverGetVo);
            AssertHelper.WriteLine("--serverGetVoJson--");
            serverGetVoJson.Log();

            serverGetVo.CheckChanged("A").ShouldTrue("A Reset Foo");
            serverGetVo.CheckChanged("B").ShouldFalse("B NotRest");
            serverGetVo.CheckChanged("C").ShouldFalse("C Rest Same");
            serverGetVo.CheckChanged("D").ShouldTrue("Add NEW D");
        }
    }
}
