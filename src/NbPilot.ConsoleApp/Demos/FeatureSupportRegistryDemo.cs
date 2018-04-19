using System;
using NbPilot.Common.Serialize;
using NbPilot.Common.Supports;

namespace NbPilot.ConsoleApp.Demos
{
    public class FeatureSupportRegistryDemo
    {
        public static void Run()
        {
            Console.WriteLine("save FeatureSupportRegistry:");
            var supportRegistry = new FeatureSupportRegistry();
            supportRegistry.Features.Add(new Feature() { Code = "FeatureA", SinceVersion = "1.0.0", DeadVersion = null });
            supportRegistry.Features.Add(new Feature() { Code = "FeatureB", SinceVersion = "1.1.0", DeadVersion = null });
            FeatureSupportRegistry.SaveToInitData(supportRegistry);
            
            Console.WriteLine("load FeatureSupportRegistry:");
            var featureSupportRegistry = FeatureSupportRegistry.LoadFromInitData();

            Console.WriteLine("show FeatureSupportRegistry:");
            var nbJsonSerialize = NbJsonSerialize.Resolve();
            var serialize = nbJsonSerialize.Serialize(featureSupportRegistry);
            Console.WriteLine(serialize);
        }
    }
}
