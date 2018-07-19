using System;
using System.Collections.Generic;

namespace NbPilot.Common
{
    /// <summary>
    /// 实例容器（单例）
    /// </summary>
    public class NbConstHolder
    {
        /// <summary>
        /// 实例容器（单例）
        /// </summary>
        public NbConstHolder()
        {
            Items = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 所有注册的项
        /// </summary>
        public Dictionary<Type, object> Items { get; set; }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : new()
        {
            var type = typeof (T);
            if (!Items.ContainsKey(type))
            {
                Items[type] = new T();
            }
            return (T)Items[type];
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Set<T>(T value) where T : new()
        {
            var type = typeof(T);
            Items[type] = value;
        }

        #region instance

        /// <summary>
        /// ctor
        /// </summary>
        static NbConstHolder()
        {
            Instance = new NbConstHolder();
        }

        /// <summary>
        /// Singleton
        /// </summary>
        public static NbConstHolder Instance { get; set; }

        #endregion
    }
}
