using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    [TestClass]
    public class SimpleActivatorSpec
    {
        [TestMethod]
        public void GetCurrentDomainAssemblies_Should_OK()
        {
            var reflectionHelper = new SimpleActivator();
            var count = reflectionHelper.GetCurrentDomainAssemblies().Count;
            (count > 0).ShouldTrue();
            
        }


        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateAllInstance_Null_Should_ThrowEx()
        {
            var reflectionHelper = new SimpleActivator();
            reflectionHelper.CreateAllInstance(null);
        }

        [ExpectedException(typeof(MissingMethodException))]
        [TestMethod]
        public void CreateAllInstance_NoDefaultCtor_Should_ThrowEx()
        {
            var reflectionHelper = new SimpleActivator();
            reflectionHelper.CreateAllInstance(typeof(NoDefaultCtor));
        }

        [TestMethod]
        public void CreateInstance_Default_Should_ReturnOK()
        {
            var reflectionHelper = new SimpleActivator();
            var instances = reflectionHelper.CreateAllInstance<InvokeDemoA>().SingleOrDefault();
            instances.ShouldNotNull();
            instances.Foo().ShouldEqual("A");

        }
    }

    #region mocks

    public interface IInvokeDemo
    {
        string Foo();
    }
    public class InvokeDemoA : IInvokeDemo
    {
        public string Foo()
        {
            return "A";
        }
    }
    public class InvokeDemoB : IInvokeDemo
    {
        public string Foo()
        {
            return "B";
        }
    }

    public class NoDefaultCtor
    {
        public NoDefaultCtor(string foo)
        {
            Foo = foo;
        }
        public string Foo { get; set; }
    }

    #endregion
}
