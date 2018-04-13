using System;
using System.Linq;
using System.Reflection;


// ReSharper disable once CheckNamespace
namespace NbPilot.Common
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取当前类型的Assembly
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        private static string _namespacePrefix = null;
        public static string GetNamespacePrefix(this Type type)
        {
            if (_namespacePrefix != null)
            {
                return _namespacePrefix;
            }
            
            var ns = type.Namespace;
            if (ns != null)
            {
                var result = ns.Split('.').FirstOrDefault();
                _namespacePrefix = !string.IsNullOrWhiteSpace(result) ? result : "NbCloud";
            }
            return _namespacePrefix;
        }
    }
}