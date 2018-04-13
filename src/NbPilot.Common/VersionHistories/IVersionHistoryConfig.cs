namespace NbPilot.Common.VersionHistories
{
    public interface IVersionHistoryConfig
    {
        /// <summary>
        /// 版本分类
        /// </summary>
        string VersionCategory { get; set; }

        /// <summary>
        /// 配置版本历史记录
        /// </summary>
        /// <param name="registry"></param>
        void Config(VersionHistoryRegistry registry);
    }
}