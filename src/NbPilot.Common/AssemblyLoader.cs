using System;
using System.Collections.Generic;
using System.Reflection;

namespace NbPilot.Common
{
    public interface IAssemblyLoader
    {
        /// <summary>
        /// ���س����ļ�
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        Assembly Load(string assemblyPath);
        /// <summary>
        /// ��ȡ�������е����г���
        /// </summary>
        /// <returns></returns>
        IList<Assembly> GetCurrentDomainAssemblies();
    }

    public class AssemblyLoader : IAssemblyLoader
    {
        #region for di extensions

        private static Func<IAssemblyLoader> _resolve = () => ResolveAsSingleton.Resolve<AssemblyLoader, IAssemblyLoader>();
        public static Func<IAssemblyLoader> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public Assembly Load(string assemblyPath)
        {
            var key = assemblyPath.ToLower();
            if (!Assemblies.ContainsKey(key))
            {
                #region readme

                //https://www.codeproject.com/Articles/34301/Assembly-LoadFile-versus-Assembly-LoadFrom-NET-obs
                //��Use the LoadFile method to load and examine assemblies that have the same identity, but are located in different paths.��

                #endregion

                var assembly = Assembly.LoadFile(assemblyPath);
                Assemblies[key] = assembly;
            }
            return Assemblies[key];
        }

        public IList<Assembly> GetCurrentDomainAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public Dictionary<string, Assembly> Assemblies { get; set; }
        public AssemblyLoader()
        {
            Assemblies = new Dictionary<string, Assembly>();
        }
    }
}