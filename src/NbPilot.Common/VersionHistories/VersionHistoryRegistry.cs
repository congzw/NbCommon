using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NbPilot.Common.VersionHistories
{
    /// <summary>
    /// 版本记录注册表
    /// </summary>
    public class VersionHistoryRegistry
    {
        /// <summary>
        /// 版本记录
        /// </summary>
        public VersionHistoryDictionary VersionHistories { get; set; }

        public VersionHistoryRegistry AddWithAutoKey(VersionHistory versionHistory)
        {
            if (versionHistory == null)
            {
                throw new ArgumentNullException("versionHistory");
            }

            if (VersionHistories == null)
            {
                VersionHistories = new VersionHistoryDictionary();
            }

            if (VersionHistories.ContainsKey(versionHistory.Id()))
            {
                throw new InvalidOperationException("重复注册: " + versionHistory.Id());
            }

            VersionHistories.Add(versionHistory.Id(), versionHistory);
            return this;
        }
        
        public bool Inited { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="versionHistoryDeclareServices"></param>
        public void Init(IList<IVersionHistoryConfig> versionHistoryDeclareServices)
        {
            Inited = true;
            VersionHistories = new VersionHistoryDictionary();
            if (versionHistoryDeclareServices == null || versionHistoryDeclareServices.Count == 0)
            {
                return;
            }

            foreach (var versionHistoryDeclareService in versionHistoryDeclareServices)
            {
                versionHistoryDeclareService.Config(this);
            }
        }

        #region for di extensions

        private static Func<VersionHistoryRegistry> _resolve = () => ResolveAsSingleton.Resolve<VersionHistoryRegistry>();
        public static Func<VersionHistoryRegistry> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}