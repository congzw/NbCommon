using System;
using ZQNB.Common;
using ZQNB.Common.Data.Model;

namespace ZQNB.BaseLib.Users2.Domains.Users
{
    /// <summary>
    /// 用户更改组织的记录
    /// </summary>
    public interface IUserChangeOrg : IEntityId
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        Guid UserId { get; set; }

        /// <summary>
        /// 转出组织Id
        /// </summary>
        Guid FromOrgId { get; set; }

        /// <summary>
        /// 转入组织Id
        /// </summary>
        Guid ToOrgId { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        Guid OperatorId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        DateTime OperateDate { get; set; }

        /// <summary>
        /// 操作来源 App、个人空间、后台管理
        /// </summary>
        string OperateSource { get; set; }
    }

    /// <summary>
    /// 用户组织变迁历史记录
    /// </summary>
    public class UserChangeOrg : NbEntity<UserChangeOrg>, IUserChangeOrg
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 转出组织Id
        /// </summary>
        public virtual Guid FromOrgId { get; set; }
        /// <summary>
        /// 转入组织Id
        /// </summary>
        public virtual Guid ToOrgId { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public virtual Guid OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime OperateDate { get; set; }
        /// <summary>
        /// 操作来源 App、个人空间、后台管理
        /// </summary>
        public virtual string OperateSource { get; set; }
    }
}
