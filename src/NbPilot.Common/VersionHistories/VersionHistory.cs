using System;
using System.Collections.Generic;

namespace NbPilot.Common.VersionHistories
{
    /// <summary>
    /// 版本历史
    /// </summary>
    public class VersionHistory
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get { return string.Format("{0}-{1}", Category, Version); } }
        /// <summary>
        /// 分类(系统的分类标识)
        /// 分类+版本号=主键
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="category"></param>
        /// <param name="version"></param>
        /// <param name="description"></param>
        /// <param name="createDate"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static VersionHistory Create(string category, string version, DateTime createDate, string description)
        {
            //todo validate
            var versionHistory = new VersionHistory();
            versionHistory.Category = category.Trim();
            versionHistory.Version = version.Trim();
            versionHistory.Description = description;
            versionHistory.CreateDate = createDate;
            return versionHistory;
        }
    }

    /// <summary>
    /// 版本历史字典
    /// </summary>
    public class VersionHistoryDictionary : Dictionary<String, VersionHistory>
    {
    }
}