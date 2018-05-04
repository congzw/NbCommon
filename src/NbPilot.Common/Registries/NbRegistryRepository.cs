using System;
using System.Collections.Generic;
using System.Linq;
using NbPilot.Common.AppData.Init;

namespace NbPilot.Common.Registries
{
    /// <summary>
    /// 注册表的仓储服务，用于从数据源存储和加载（可用于二次编辑和保存）
    /// </summary>
    public interface INbRegistryRepository
    {
        /// <summary>
        /// 从数据源加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Load<T>() where T : NbRegistry<T>;
        /// <summary>
        /// 保存到数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registry"></param>
        void Save<T>(T registry) where T : NbRegistry<T>;
    }

    /// <summary>
    /// 注册表的仓储服务（默认使用JSON文件数据源）
    /// </summary>
    public class NbRegistryRepository : INbRegistryRepository
    {
        private IInitDataContext _initDataContext;
        public IInitDataContext InitDataContext
        {
            get
            {
                if (_initDataContext == null)
                {
                    _initDataContext = AppData.Init.InitDataContext.Resolve();
                }
                return _initDataContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _initDataContext = value;
            }
        }
        
        public T Load<T>() where T : NbRegistry<T>
        {
            var featureSupportRegistries = InitDataContext.Read<T>();
            var theOne = featureSupportRegistries.SingleOrDefault();
            return theOne;
        }

        public void Save<T>(T registry) where T : NbRegistry<T>
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }
            InitDataContext.Save(new List<T>() { registry });
        }

        #region for di extensions

        private static Func<INbRegistryRepository> _resolve = () => ResolveAsSingleton.Resolve<NbRegistryRepository, INbRegistryRepository>();
        public static Func<INbRegistryRepository> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}
