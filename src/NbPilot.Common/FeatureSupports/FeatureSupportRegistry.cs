using System;
using System.Collections.Generic;
using System.Linq;
using NbPilot.Common.AppData.Init;

namespace NbPilot.Common.FeatureSupports
{
    public class FeatureSupportRegistry
    {
        public FeatureSupportRegistry()
        {
            Features = new List<Feature>();
            Products = new List<Product>();
            FeatureSupports = new List<FeatureSupport>();
        }
        
        public IList<Feature> Features { get; set; }
        public IList<Product> Products { get; set; }
        public IList<FeatureSupport> FeatureSupports { get; set; }

        public static FeatureSupportRegistry LoadFromInitData()
        {
            var initDataContext = InitDataContext.Resolve();
            var featureSupportRegistries = initDataContext.Read<FeatureSupportRegistry>();
            var theOne = featureSupportRegistries.SingleOrDefault();
            if (theOne == null)
            {
                throw new InvalidOperationException("未发现预置数据: " + typeof(FeatureSupportRegistry).Name);
            }
            return theOne;
        }
        public static void SaveToInitData(FeatureSupportRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            var initDataContext = InitDataContext.Resolve();
            initDataContext.Save(new List<FeatureSupportRegistry>() { registry });
        }
    }
}
