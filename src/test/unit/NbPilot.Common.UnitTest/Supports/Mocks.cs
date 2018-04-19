using System.Collections.Generic;

namespace NbPilot.Common.Supports
{
    public class Mocks
    {
        public static FeatureSupportTable Create()
        {
            var featureSupportTable = new FeatureSupportTable();

            var features = new List<Feature>();
            features.Add(new Feature() { Code = "FeatureA", SinceVersion = "1.0.0", DeadVersion = null });
            features.Add(new Feature() { Code = "FeatureB", SinceVersion = "2.0.0", DeadVersion = null });
            features.Add(new Feature() { Code = "FeatureC", SinceVersion = "2.1.0", DeadVersion = null });

            //featureSupportTable.Init();
            return featureSupportTable;
        }

        public static List<Feature> CreateNormalFeatures()
        {
            var features = new List<Feature>();
            features.Add(new Feature() { Code = "FeatureA", SinceVersion = "1.0.0", DeadVersion = null });
            features.Add(new Feature() { Code = "FeatureB", SinceVersion = "2.0.0", DeadVersion = null });
            features.Add(new Feature() { Code = "FeatureC", SinceVersion = "2.1.0", DeadVersion = null });
            return features;
        }
    }
}
