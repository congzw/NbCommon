using System;
using ZQNB.Common.Data.Model;

namespace ZQNB.BaseLib.Rbac2.Domains.UserRoles
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public interface IUserRole //: IEntityId
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        Guid SiteId { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        string UserLoginName { get; set; }

        /// <summary>
        /// 角色Code
        /// </summary>
        string RoleCode { get; set; }
    }

    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRole : NbEntity<UserRole>, IUserRole
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual Guid SiteId { get; set; }
        /// <summary>
        /// 用户登录名
        /// </summary>
        public virtual string UserLoginName { get; set; }
        /// <summary>
        /// 角色Code
        /// </summary>
        public virtual string RoleCode { get; set; }
    }
}
