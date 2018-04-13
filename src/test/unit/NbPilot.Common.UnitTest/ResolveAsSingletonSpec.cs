using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NbPilot.Common
{
    [TestClass]
    public class ResolveAsSingletonSpec
    {
        [TestMethod]
        public void ResolveAsInterface_SingleThread_Singleton_ShouldSame()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>(), resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>());
            resolveDemoTestResult.ShouldSame();
        }

        int multiThreadTaskCount = 20;
        [TestMethod]
        public void ResolveAsInterface_MultiThread_Singleton_ShouldSame()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>
                , resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>
                , true);
        }

        [TestMethod]
        public void ResolveAsInterface_Should_Return_Default_First()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            var resolveDemo = resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            var resolveDemo2 = resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            resolveDemo.Desc.ShouldNull();
            resolveDemo2.Desc.ShouldNull();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2);
            resolveDemoTestResult.ShouldSame();
        }


        [TestMethod]
        public void Resolve_SingleThread_Singleton_ShouldSame()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveAsSingleton.Resolve<ResolveDemo>(), resolveAsSingleton.Resolve<ResolveDemo>());
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void Resolve_MultiThread_Singleton_ShouldSame()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , resolveAsSingleton.Resolve<ResolveDemo>
                , resolveAsSingleton.Resolve<ResolveDemo>
                , true);
        }

        [TestMethod]
        public void Resolve_Should_Return_Default_First()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            var resolveDemo = resolveAsSingleton.Resolve<ResolveDemo>();
            var resolveDemo2 = resolveAsSingleton.Resolve<ResolveDemo>();
            resolveDemo.Desc.ShouldNull();
            resolveDemo2.Desc.ShouldNull();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2);
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void Resolve_AsInterfaceAndInstance_ShouldSame()
        {
            IResolveAsSingleton resolveAsSingleton = new ResolveAsSingleton();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>(), resolveAsSingleton.Resolve<ResolveDemo>());
            resolveDemoTestResult.ShouldSame();
        }

        //ingore singleton test problems
        //[TestMethod]
        public void Factory_Replace_ShouldOK()
        {
            var mockNinjectReslover = new MockNinjectReslover();
            ResolveAsSingleton.Factory = () => mockNinjectReslover;

            var resolveDemo = ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            var resolveDemo2 = ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            var resolveDemo3 = ResolveAsSingleton.Resolve<ResolveDemo>();
            var resolveDemo4 = ResolveAsSingleton.Resolve<ResolveDemo>();

            ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2).ShouldSame();
            ObjectIntanceTestResult.Create(resolveDemo, resolveDemo3).ShouldSame();
            ObjectIntanceTestResult.Create(resolveDemo, resolveDemo4).ShouldSame();

            mockNinjectReslover.InvokedResolve.ShouldTrue();
            mockNinjectReslover.InvokedResolveAsInterface.ShouldTrue();
        }

        #region test helper

        public interface IResolveDemo
        {
            string Desc { get; set; }
        }
        public class ResolveDemo : IResolveDemo
        {
            public string Desc { get; set; }
        }
        public class MockNinjectReslover : IResolveAsSingleton
        {
            public bool InvokedResolveAsInterface { get; set; }
            public bool InvokedResolve { get; set; }

            private readonly Dictionary<Type, object> _mockNinject = new Dictionary<Type, object>();

            public TInterface Resolve<T, TInterface>() where T : TInterface, new()
            {
                InvokedResolveAsInterface = true;
                var type = typeof(T);
                if (!_mockNinject.ContainsKey(type))
                {
                    _mockNinject[type] = CreateService(type);
                }
                return (TInterface)_mockNinject[type];
            }

            public T Resolve<T>() where T : new()
            {
                InvokedResolve = true;
                var type = typeof(T);
                if (!_mockNinject.ContainsKey(type))
                {
                    _mockNinject[type] = CreateService(type);
                }
                return (T)_mockNinject[type];
            }

            private object CreateService(Type serviceType)
            {
                // Since attempting to create an instance of an interface or an abstract type results in an exception, immediately return null
                // to improve performance and the debugging experience with first-chance exceptions enabled.
                if (serviceType.IsInterface || serviceType.IsAbstract)
                {
                    return null;
                }

                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion
    }
}
