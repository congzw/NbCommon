using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Modules
{

    [DependsOn(typeof(ModuleC))]
    public class ModuleB : NbModule
    {
        public override Assembly[] GetAdditionalAssemblies()
        {
            return new[] { typeof(TestMethodAttribute).GetAssembly() };
        }
    }

    public class ModuleC : NbModule
    {
        public override Assembly[] GetAdditionalAssemblies()
        {
            return new[] { typeof(TestMethodAttribute).GetAssembly() };
        }
    }

    [DependsOn(typeof(ModuleB), typeof(ModuleC))]
    public class ModuleA : NbModule
    {
        public override Assembly[] GetAdditionalAssemblies()
        {
            return new[] { typeof(TestMethodAttribute).GetAssembly() };
        }
    }

    //还没太想好！
    public class ModuleMain : NbKernelModule
    {

    }

    //public class ModuleMain :  NbModule, INbKernelModule
    //{

    //}
}
