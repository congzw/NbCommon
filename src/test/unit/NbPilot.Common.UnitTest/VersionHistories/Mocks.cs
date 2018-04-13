using System;

namespace NbPilot.Common.VersionHistories
{
    public class MockAConfig : IVersionHistoryConfig
    {
        public MockAConfig()
        {
            VersionCategory = "A";
        }
        public string VersionCategory { get; set; }

        public void Config(VersionHistoryRegistry registry)
        {
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.0.0", new DateTime(2000, 1, 1), "feature 1.0.0"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.0.1", new DateTime(2000, 1, 2), "bug fix 1.0.1"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.0.2", new DateTime(2000, 1, 3), "bug fix 1.0.2"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.1.0", new DateTime(2000, 1, 4), "bug fix 1.1.0"));
        }
    }
    public class MockBConfig : IVersionHistoryConfig
    {
        public MockBConfig()
        {
            VersionCategory = "B";
        }
        public string VersionCategory { get; set; }

        public void Config(VersionHistoryRegistry registry)
        {
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.0.0", new DateTime(2000, 1, 1), "feature 1.0.0"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.0.1", new DateTime(2000, 1, 2), "bug fix 1.0.1"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.0.2", new DateTime(2000, 1, 3), "bug fix 1.0.2"));
            registry.AddWithAutoKey(VersionHistory.Create(VersionCategory, "1.1.0", new DateTime(2000, 1, 4), "bug fix 1.1.0"));
        }
    }
}
