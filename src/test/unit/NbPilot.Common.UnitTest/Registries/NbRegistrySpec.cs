using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Registries
{
    [TestClass]
    public class NbRegistrySpec
    {
        [TestMethod]
        public void Instance_Should_Singleton()
        {
            var demoRegistry = DemoRegistry.Instance;
            var demoRegistry2 = DemoRegistry.Instance;
            demoRegistry2.ShouldSame(demoRegistry);
        }

        [TestMethod]
        public void Init_Repeat_Should_ThrowEx()
        {
            var demoRegistry = new DemoRegistry();
            var services = new List<INbRegistryService<DemoRegistry>>();
            services.Add(new DemoRegistryService());
            services.Add(new DemoRegistryService2());

            demoRegistry.Init(services);
            AssertHelper.ShouldThrows<InvalidOperationException>(() =>
            {
                demoRegistry.Init(services); 
            });
        }

        [TestMethod]
        public void Init_Once_Should_OK()
        {
            var demoRegistry = new DemoRegistry();
            var services = new List<INbRegistryService<DemoRegistry>>();
            services.Add(new DemoRegistryService());
            services.Add(new DemoRegistryService2());

            demoRegistry.Init(services);

            demoRegistry.DemoItems.Count.ShouldEqual(2);
        }

        [TestMethod]
        public void FindAllServices_NoImpls_Should_Return_Empty()
        {
            var demoEmptyRegistry = new DemoEmptyRegistry();
            var demoEmptyRegistryServices = demoEmptyRegistry.FindAllServices();
            foreach (var demoEmptyRegistryService in demoEmptyRegistryServices)
            {
                demoEmptyRegistryService.GetType().FullName.Log();
            }
            demoEmptyRegistryServices.Count.ShouldEqual(0);
        }

        [TestMethod]
        public void FindAllServices_WithImpls_Should_Return_All()
        {
            var demoRegistry = new DemoRegistry();
            var nbRegistryServices = demoRegistry.FindAllServices();
            foreach (var nbRegistryService in nbRegistryServices)
            {
                nbRegistryService.GetType().FullName.Log();
            }
            nbRegistryServices.Count.ShouldEqual(2);
        }
    }
}
