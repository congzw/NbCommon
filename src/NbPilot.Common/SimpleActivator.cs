using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NbPilot.Common
{
    /// <summary>
    /// 建议的反射创建实例的帮助类
    /// </summary>
    public interface ISimpleActivator
    {
        /// <summary>
        /// 获取当前Domain的程序集
        /// </summary>
        /// <returns></returns>
        IList<Assembly> GetCurrentDomainAssemblies();
            /// <summary>
        /// 查找所有的实现类，并实例化
        /// </summary>
        /// <param name="abstractType"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        IList<object> CreateAllInstance(Type abstractType, params Assembly[] assemblies);
        /// <summary>
        /// 查找所有的实现类，并实例化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IList<T> CreateAllInstance<T>(params Assembly[] assemblies);
    }

    public class SimpleActivator : ISimpleActivator
    {
        #region for di extensions

        private static Func<ISimpleActivator> _resolve = () => ResolveAsSingleton.Resolve<SimpleActivator, ISimpleActivator>();
        public static Func<ISimpleActivator> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public IList<Assembly> GetCurrentDomainAssemblies()
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies().ToList();
        }

        public IList<object> CreateAllInstance(Type abstractType, params Assembly[] assemblies)
        {
            var objects = new List<object>();
            var implementingTypes = GetImplementingTypes(abstractType, assemblies);
            if (implementingTypes.Count == 0)
            {
                return objects;
            }
            foreach (var implementingType in implementingTypes)
            {
                objects.Add(CreateInstance(implementingType));
            }
            return objects;
        }

        public IList<T> CreateAllInstance<T>(params Assembly[] assemblies)
        {
            var abstractType = typeof(T);
            var allInstance = CreateAllInstance(abstractType, assemblies);
            return allInstance.Cast<T>().ToList();
        }

        private IList<Type> GetImplementingTypes(Type abstractType, params Assembly[] assemblies)
        {
            if (abstractType == null)
            {
                throw new ArgumentNullException("abstractType");
            }

            List<Assembly> assembliesFix;
            if (assemblies == null || assemblies.Length == 0)
            {
                assembliesFix = AppDomain
                    .CurrentDomain
                    .GetAssemblies().ToList();
            }
            else
            {
                assembliesFix = assemblies.Distinct().ToList();
            }

            var allTypes = assembliesFix.SelectMany(assembly => assembly.GetTypes()).ToList();
            if (!allTypes.Contains(abstractType))
            {
                throw new InvalidOperationException("要查找的abstractType，并不是从assemblies中获取，请确认是否是分别使用了不同的程序集进行了参数的声明");
            }

            //todo GenericType
            var types = allTypes.Where(t => abstractType.IsAssignableFrom(t)
                       && t.IsAbstract == false
                       && t.IsGenericTypeDefinition == false
                       && t.IsInterface == false).ToList();
            return types;
        }

        private object CreateInstance(Type type)
        {
            var instance = Activator.CreateInstance(type);
            return instance;
        }
    }
}
