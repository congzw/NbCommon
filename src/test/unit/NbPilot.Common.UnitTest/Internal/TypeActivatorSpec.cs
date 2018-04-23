using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Internal
{
    [TestClass]
    public class TypeActivatorSpec
    {
        [TestMethod]
        public void Create_TypeOfBase_ShouldOk()
        {
            var dics = new Dictionary<Type, Type>
                {
                    { typeof(List<int>), typeof(IList<int>)},
                    { typeof(Dictionary<int, int>), typeof(IDictionary<int, int>)},
                    { typeof(List<HttpStatusCode>), typeof(IEnumerable<HttpStatusCode>)},
                };

            foreach (var item in dics)
            {
                CreateOfTBase(item.Key, item.Value);
            }
        }

        [TestMethod]
        public void Create_Type_ShouldOk()
        {
            var dics = new Dictionary<Type, Type>
                {
                    { typeof(List<int>), typeof(IList<int>)},
                    { typeof(Dictionary<int, int>), typeof(IDictionary<int, int>)},
                    { typeof(List<HttpStatusCode>), typeof(IEnumerable<HttpStatusCode>)},
                };

            foreach (var item in dics)
            {
                CreateType(item.Key, item.Value);
            }
        }

        [TestMethod]
        public void Create_TypeOfT_ShouldOk()
        {
            var dics = new Dictionary<Type, Type>
                {
                    { typeof(List<int>), typeof(IList<int>)},
                    { typeof(Dictionary<int, int>), typeof(IDictionary<int, int>)},
                    { typeof(List<HttpStatusCode>), typeof(IEnumerable<HttpStatusCode>)},
                };

            foreach (var item in dics)
            {
                CreateType(item.Key, item.Value);
            }
        }


        public void CreateType(Type instanceType, Type baseType)
        {
            // Arrange
            Func<object> instanceDelegate = TypeActivator.Create(instanceType);

            // Act
            object instance = instanceDelegate();

            // Assert
            Assert.IsInstanceOfType(instance, instanceType);
            instance.GetType().Log();
        }

        public void CreateOfT(Type instanceType, Type baseType)
        {
            // Arrange
            Type activatorType = typeof(TypeActivator);
            MethodInfo createMethodInfo = activatorType.GetMethod("Create", Type.EmptyTypes);
            MethodInfo genericCreateMethodInfo = createMethodInfo.MakeGenericMethod(instanceType);
            Func<object> instanceDelegate = (Func<object>)genericCreateMethodInfo.Invoke(null, null);

            // Act
            object instance = instanceDelegate();

            // Assert
            Assert.IsInstanceOfType(instance, instanceType);
            instance.GetType().Log();
        }
        
        public void CreateOfTBase(Type instanceType, Type baseType)
        {
            // Arrange
            Type activatorType = typeof(TypeActivator);
            MethodInfo createMethodInfo = null;
            foreach (MethodInfo methodInfo in activatorType.GetMethods())
            {
                ParameterInfo[] parameterInfo = methodInfo.GetParameters();
                if (methodInfo.Name == "Create" && methodInfo.ContainsGenericParameters && parameterInfo.Length == 1 && parameterInfo[0].ParameterType == typeof(Type))
                {
                    createMethodInfo = methodInfo;
                    break;
                }
            }

            MethodInfo genericCreateMethodInfo = createMethodInfo.MakeGenericMethod(baseType);
            Func<object> instanceDelegate = (Func<object>)genericCreateMethodInfo.Invoke(null, new object[] { instanceType });

            // Act
            object instance = instanceDelegate();

            // Assert
            Assert.IsInstanceOfType(instance, instanceType);
            instance.GetType().Log();
        }
    }
}
