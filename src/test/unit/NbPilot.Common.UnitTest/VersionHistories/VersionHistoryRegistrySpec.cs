using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.VersionHistories
{
    [TestClass]
    public class VersionHistoryRegistrySpec
    {
        [ExpectedException(typeof (InvalidOperationException))]
        [TestMethod]
        public void AddWithAutoKey_SameCategoryAndVersion_Should_ThrowEx()
        {
            var registry = new VersionHistoryRegistry();
            registry.AddWithAutoKey(VersionHistory.Create("Demo", "1.0.0", new DateTime(2000, 1, 1), "feature 1.0.0"));
            registry.AddWithAutoKey(VersionHistory.Create("Demo", "1.0.0", new DateTime(2000, 1, 2), "bug fix 1.0.1"));
        }

        [TestMethod]
        public void AddWithAutoKey_DiffCategoryAndVersion_Should_OK()
        {
            var registry = new VersionHistoryRegistry();
            registry.AddWithAutoKey(VersionHistory.Create("Demo1", "1.0.0", new DateTime(2000, 1, 1), "feature 1.0.0"));
            registry.AddWithAutoKey(VersionHistory.Create("Demo2", "1.0.0", new DateTime(2000, 1, 2), "bug fix 1.0.1"));
            
            registry.VersionHistories.Count.ShouldEqual(2);
            registry.VersionHistories.Values.LogProperties();
        }


        [TestMethod]
        public void Init_ArgumentNullOrEmpty_Should_ReturnEmpty()
        {
            var versionHistoryRegistry = new VersionHistoryRegistry();
            versionHistoryRegistry.Init(null);
            versionHistoryRegistry.VersionHistories.Count.ShouldEqual(0);
            versionHistoryRegistry.Init(new List<IVersionHistoryConfig>());
            versionHistoryRegistry.VersionHistories.Count.ShouldEqual(0);
        }

        [TestMethod]
        public void Init_Again_Should_Clear()
        {
            var versionHistoryDeclareServices = new List<IVersionHistoryConfig>();
            versionHistoryDeclareServices.Add(new MockAConfig());
            versionHistoryDeclareServices.Add(new MockBConfig());

            var versionHistoryRegistry = new VersionHistoryRegistry();
            versionHistoryRegistry.Init(versionHistoryDeclareServices);
            versionHistoryRegistry.Init(new List<IVersionHistoryConfig>());

            versionHistoryRegistry.VersionHistories.Count.ShouldEqual(0);
        }

        [TestMethod]
        public void Init_NoRepeatConfig_Should_OK()
        {
            var versionHistoryDeclareServices = new List<IVersionHistoryConfig>();
            versionHistoryDeclareServices.Add(new MockAConfig());
            versionHistoryDeclareServices.Add(new MockBConfig());

            var versionHistoryRegistry = new VersionHistoryRegistry();
            versionHistoryRegistry.Init(versionHistoryDeclareServices);

            versionHistoryRegistry.VersionHistories.Count.ShouldEqual(8);
        }
    }
}
