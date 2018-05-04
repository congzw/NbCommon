using System;
using System.Collections.Generic;
using System.Linq;
using NbPilot.Common.Internal;
using System.Reflection;

namespace NbPilot.Common.Registries
{
    public abstract class NbRegistry<T> where T : NbRegistry<T>
    {
        private bool _inited;
        /// <summary>
        /// 初始化注册表
        /// </summary>
        /// <param name="nbRegistryServices"></param>
        /// <returns></returns>
        public void Init(IList<INbRegistryService<T>> nbRegistryServices)
        {
            if (_inited)
            {
                throw new InvalidOperationException("NbRegistry should't be inited more then once!");
            }
            foreach (var dicRegistryService in nbRegistryServices)
            {
                dicRegistryService.Register((T)this);
            }
            _inited = true;
        }

        /// <summary>
        /// 从程序集中查找实现INbRegistryService接口的服务
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public IList<INbRegistryService<T>> FindAllServices(params Assembly[] assemblies)
        {
            var nbRegistryServices = new List<INbRegistryService<T>>();
            var findAllImplTypes = FindAllImplTypes(assemblies);
            foreach (var findAllImplType in findAllImplTypes)
            {
                Func<INbRegistryService<T>> func = TypeActivator.Create<INbRegistryService<T>>(findAllImplType);
                nbRegistryServices.Add(func());
            }
            return nbRegistryServices;
        }
        
        /// <summary>
        /// 唯一的实例
        /// </summary>
        /// <returns></returns>
        public static T Instance
        {
            get { return AutoResolveAsSingletonHelper<T>.Lazy.Value; }
        }

        #region helpers

        private IList<Type> FindAllImplTypes(params Assembly[] assemblies)
        {
            var result = new List<Type>();
            var currentDomainAssembliesFix = assemblies.Length != 0 ? assemblies : AppDomain.CurrentDomain.GetAssemblies().ToArray();
            foreach (var assemblyToScan in currentDomainAssembliesFix)
            {
                Type implementedInterface = typeof(INbRegistryService<T>);
                IEnumerable<Type> typesInTheAssembly;
                try
                {
                    typesInTheAssembly = assemblyToScan.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    typesInTheAssembly = e.Types.Where(t => t != null);
                }

                // if the interface is a generic interface
                if (implementedInterface.IsGenericType)
                {
                    foreach (var typeInTheAssembly in typesInTheAssembly)
                    {
                        if (typeInTheAssembly.IsClass)
                        {
                            var typeInterfaces = typeInTheAssembly.GetInterfaces();
                            foreach (var typeInterface in typeInterfaces)
                            {
                                if (typeInterface.IsGenericType)
                                {
                                    var typeGenericInterface = typeInterface.GetGenericTypeDefinition();
                                    var implementedGenericInterface = implementedInterface.GetGenericTypeDefinition();

                                    if (typeGenericInterface == implementedGenericInterface)
                                    {
                                        var genericArguments = implementedInterface.GetGenericArguments();
                                        if (genericArguments.Length > 0)
                                        {
                                            if (implementedInterface.IsAssignableFrom(typeInTheAssembly))
                                            {
                                                result.Add(typeInTheAssembly);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        #endregion
    }
}