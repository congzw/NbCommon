using System.Collections.Generic;

namespace NbPilot.Common.Registries
{
    public class DemoRegistry : NbRegistry<DemoRegistry>
    {
        public DemoRegistry()
        {
            DemoItems = new List<DemoItem>();
        }
        public IList<DemoItem> DemoItems { get; set; }
    }

    public class DemoItem
    {
        public string Name { get; set; }
    }

    public class DemoRegistryService : INbRegistryService<DemoRegistry>
    {
        public void Register(DemoRegistry nbRegistry)
        {
            var demoItem = new DemoItem() { Name = "A" };
            nbRegistry.DemoItems.Add(demoItem);
        }
    }

    public class DemoRegistryService2 : INbRegistryService<DemoRegistry>
    {
        public void Register(DemoRegistry nbRegistry)
        {
            var demoItem = new DemoItem() { Name = "B" };
            nbRegistry.DemoItems.Add(demoItem);
        }
    }

    public class DemoEmptyRegistry : NbRegistry<DemoEmptyRegistry>
    {
        public DemoEmptyRegistry()
        {
            DemoItems = new List<DemoItem>();
        }

        public IList<DemoItem> DemoItems { get; set; }
    }
}
