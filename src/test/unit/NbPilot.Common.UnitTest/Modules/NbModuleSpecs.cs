using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Modules
{
    [TestClass]
    public class NbModuleSpecs
    {
        [TestMethod]
        public void IsNbModule_Should_OK()
        {
            NbModule.IsNbModule(typeof(ModuleA)).ShouldTrue();
            NbModule.IsNbModule(typeof(ModuleB)).ShouldTrue();
            NbModule.IsNbModule(typeof(ModuleC)).ShouldTrue();
            NbModule.IsNbModule(typeof(NbModule)).ShouldFalse();
            NbModule.IsNbModule(typeof(NbModuleSpecs)).ShouldFalse();
        }
        
        [TestMethod]
        public void FindDependedModuleTypes_Should_OK()
        {
            var moduleTypes = NbModule.FindDependedModuleTypes(typeof(ModuleA));
            moduleTypes.Count.ShouldEqual(2);
            moduleTypes[0].Name.ShouldEqual(typeof(ModuleB).Name);
            moduleTypes[1].Name.ShouldEqual(typeof(ModuleC).Name);
        }

        [TestMethod]
        public void FindDependedModuleTypesRecursivelyIncludingGivenModule_NotAutoIncludeKernelModule_Should_OK()
        {
            var moduleTypes = NbModule.FindDependedModuleTypesRecursivelyIncludingGivenModule(typeof(ModuleA), false);
            moduleTypes.Count.ShouldEqual(3);
            moduleTypes[0].Name.ShouldEqual(typeof(ModuleA).Name);
            moduleTypes[1].Name.ShouldEqual(typeof(ModuleB).Name);
            moduleTypes[2].Name.ShouldEqual(typeof(ModuleC).Name);
        }

        [TestMethod]
        public void FindDependedModuleTypesRecursivelyIncludingGivenModule_AutoIncludeKernelModule_Should_OK()
        {
            var moduleTypes = NbModule.FindDependedModuleTypesRecursivelyIncludingGivenModule(typeof(ModuleA), true);
            moduleTypes.Count.ShouldEqual(4);
            moduleTypes[0].Name.ShouldEqual(typeof(ModuleA).Name);
            moduleTypes[1].Name.ShouldEqual(typeof(ModuleB).Name);
            moduleTypes[2].Name.ShouldEqual(typeof(ModuleC).Name);
            moduleTypes[3].Name.ShouldEqual(typeof(ModuleMain).Name);
        }
    }
}
