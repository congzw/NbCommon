using System;
using NbPilot.Common.VersionHistories;

namespace NbPilot.Common.Metas
{
    public class VersionHistoryConfig : IVersionHistoryConfig
    {
        public VersionHistoryConfig()
        {
            VersionCategory = "NbPilot.Common";
        }
        public string VersionCategory { get; set; }
        public void Config(VersionHistoryRegistry registry)
        {
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "0.1.0", new DateTime(2018, 04, 11), "初始版本"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "0.2.0", new DateTime(2018, 04, 11), "增加ResolveAsSingleton的支持;增加GuidHelper"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "0.3.0", new DateTime(2018, 04, 12), "增加模块化的支持;"));
        }
    }
}
