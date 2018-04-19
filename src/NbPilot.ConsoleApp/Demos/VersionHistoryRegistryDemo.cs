using System;
using System.Collections.Generic;
using NbPilot.Common.Metas;
using NbPilot.Common.Serialize;
using NbPilot.Common.VersionHistories;

namespace NbPilot.ConsoleApp.Demos
{
    public class VersionHistoryRegistryDemo
    {
        public static void Run()
        {
            var versionHistoryRegistry = VersionHistoryRegistry.Resolve();
            versionHistoryRegistry.Init(new List<IVersionHistoryConfig>(){new VersionHistoryConfig()});

            Console.WriteLine("show VersionHistoryRegistry:");
            var nbJsonSerialize = NbJsonSerialize.Resolve();
            var serialize = nbJsonSerialize.Serialize(versionHistoryRegistry);
            Console.WriteLine(serialize);
        }
    }
}
