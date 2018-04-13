using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NbPilot.Common.Collections;

namespace NbPilot.Common.Modules
{
    /// <summary>
    /// 声明一个模块，一个模块能够依赖于另一个模块。在通常情况下，一个程序集就可以看成是一个模块。
    /// </summary>
    public interface INbModule
    {
        /// <summary>
        /// This is the first event called on application startup. 
        /// Codes can be placed here to run before dependency injection registrations.
        /// </summary>
        void PreInitialize();

        /// <summary>
        /// This method is used to register dependencies for this module.
        /// </summary>
        void Initialize();

        /// <summary>
        /// This method is called lastly on application startup.
        /// </summary>
        void PostInitialize();

        /// <summary>
        /// This method is called when the application is being shutdown.
        /// </summary>
        void Shutdown();
    }
    
    /// <summary>
    /// This class must be implemented by all module definition classes.
    /// </summary>
    /// <remarks>
    /// A module definition class is generally located in it's own assembly
    /// and implements some action in module events on application startup and shutdown.
    /// It also defines depended modules.
    /// </remarks>
    public abstract class NbModule : INbModule
    {
        /// <summary>
        /// This is the first event called on application startup. 
        /// Codes can be placed here to run before dependency injection registrations.
        /// </summary>
        public virtual void PreInitialize()
        {

        }

        /// <summary>
        /// This method is used to register dependencies for this module.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// This method is called lastly on application startup.
        /// </summary>
        public virtual void PostInitialize()
        {
            
        }

        /// <summary>
        /// This method is called when the application is being shutdown.
        /// </summary>
        public virtual void Shutdown()
        {
            
        }

        public virtual Assembly[] GetAdditionalAssemblies()
        {
            return new Assembly[0];
        }

        /// <summary>
        /// Checks if given type is an Nb module class.
        /// </summary>
        /// <param name="type">Type to check</param>
        public static bool IsNbModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(NbModule).IsAssignableFrom(type);
        }

        /// <summary>
        /// Finds direct depended modules of a module (excluding given module).
        /// </summary>
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsNbModule(moduleType))
            {
                throw new NbException("This type is not an Nb module: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.GetTypeInfo().IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetTypeInfo().GetCustomAttributes(typeof(DependsOnAttribute), true).Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.AddIfNotContains(dependedModuleType);
                    }
                }
            }

            return list;
        }
       
        /// <summary>
        /// Finds all depended modules of a module recursively(including given module).
        /// </summary>
        /// <param name="moduleType"></param>
        /// <param name="autoIncludeKernelModule"></param>
        /// <returns></returns>
        public static List<Type> FindDependedModuleTypesRecursivelyIncludingGivenModule(Type moduleType, bool autoIncludeKernelModule)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesRecursively(list, moduleType);
            if (autoIncludeKernelModule)
            {
                //list.AddIfNotContains(typeof(NbKernelModule));
                //find all kernel modules
                var kernelModules = FindAllKernelModules(moduleType);
                foreach (var kernelModule in kernelModules)
                {
                    if (!list.Contains(kernelModule))
                    {
                        list.AddIfNotContains(kernelModule);
                    }
                }
            }
            return list;
        }

        //helpers
        private static void AddModuleAndDependenciesRecursively(List<Type> modules, Type module)
        {
            if (!IsNbModule(module))
            {
                throw new NbException("This type is not an Nb module: " + module.AssemblyQualifiedName);
            }

            if (modules.Contains(module))
            {
                return;
            }

            modules.Add(module);

            var dependedModules = FindDependedModuleTypes(module);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesRecursively(modules, dependedModule);
            }
        }

        private static List<Type> FindAllKernelModules(Type moduleType)
        {
            var assembly = moduleType.GetAssembly();
            var kernels = assembly.GetTypes().Where(typeInfo =>
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof (INbKernelModule).IsAssignableFrom(typeInfo)).ToList();
            return kernels;
        }
    }
}
