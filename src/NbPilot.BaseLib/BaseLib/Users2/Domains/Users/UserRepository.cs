using System;
using System.Linq;
using ZQNB.Common;

namespace ZQNB.BaseLib.Users2.Domains.Users
{
    /// <summary>
    /// 用户接口（核心接口）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUserRepository<T> : ICrudRepository<T, Guid> where T : User, new()
    {
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IQueryable<T> QueryUsers(QueryUsersArgs args);
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool TryCheckUserExist(CheckUserExistArgs args);
        /// <summary>
        /// 通过用户唯一值获取某个用户
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        User TryGetUser(GetUserArgs args);
        /// <summary>
        /// 修改用户组织
        /// </summary>
        bool ChangeOrg(ChangeOrgDto changeOrgDto);
    }
    
    /// <summary>
    /// 查询用户参数
    /// </summary>
    public class QueryUsersArgs
    {
        /// <summary>
        /// 站Id
        /// siteId不为空，站内搜索使用
        /// siteId为空，全局搜索使用
        /// </summary>
        public Guid? SiteId { get; set; }
        /// <summary>
        /// 组织Id
        /// </summary>
        public Guid? OrgId { get; set; }
        /// <summary>
        /// 搜索名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色代码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 包含子组织
        /// </summary>
        public bool IncludeChildOrg { get; set; }

        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="orgId"></param>
        /// <param name="name"></param>
        /// <param name="roleCode"></param>
        /// <param name="includeChildOrg"></param>
        /// <returns></returns>
        public static QueryUsersArgs Create(Guid? siteId, Guid? orgId, string name, string roleCode, bool includeChildOrg = false)
        {
            return new QueryUsersArgs()
            {
                SiteId = siteId,
                OrgId = orgId,
                Name = name,
                RoleCode = roleCode,
                IncludeChildOrg = includeChildOrg
            };
        }
    }

    /// <summary>
    /// 检测用户是否存在的参数
    /// </summary>
    public class CheckUserExistArgs
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 自定义码，例如工号等
        /// </summary>
        public string CustomNo { get; set; }
        /// <summary>
        /// 排除的Id（例如用于修改的场合）
        /// </summary>
        public Guid? ExcludedId { get; set; }

        #region helpers

        /// <summary>
        /// 赋值LoginName
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public CheckUserExistArgs WithLoginName(string loginName)
        {
            this.LoginName = loginName;
            return this;
        }
        /// <summary>
        /// 赋值Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public CheckUserExistArgs WithEmail(string email)
        {
            this.Email = email;
            return this;
        }
        /// <summary>
        /// 赋值CustomNo
        /// </summary>
        /// <param name="customNo"></param>
        /// <returns></returns>
        public CheckUserExistArgs WithCustomNo(string customNo)
        {
            this.CustomNo = customNo;
            return this;
        }
        /// <summary>
        /// 赋值ExcludedId
        /// </summary>
        /// <param name="excludedId"></param>
        /// <returns></returns>
        public CheckUserExistArgs WithExcludedId(Guid? excludedId)
        {
            this.ExcludedId = ExcludedId;
            return this;
        }

        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="email"></param>
        /// <param name="customNo"></param>
        /// <param name="excludedId"></param>
        /// <returns></returns>
        public static CheckUserExistArgs Create(string loginName = null, string email = null, string customNo = null, Guid? excludedId = null)
        {
            var args = new CheckUserExistArgs();
            return args.WithLoginName(loginName).WithEmail(email).WithCustomNo(customNo).WithExcludedId(excludedId);
        }

        #endregion
    }

    /// <summary>
    /// 获取唯一用户的参数
    /// </summary>
    public class GetUserArgs
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 自定义码，例如工号等
        /// </summary>
        public string CustomNo { get; set; }

        #region helpers

        /// <summary>
        /// 赋值LoginName
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public GetUserArgs WithLoginName(string loginName)
        {
            this.LoginName = loginName;
            return this;
        }
        /// <summary>
        /// 赋值Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public GetUserArgs WithEmail(string email)
        {
            this.Email = email;
            return this;
        }
        /// <summary>
        /// 赋值CustomNo
        /// </summary>
        /// <param name="customNo"></param>
        /// <returns></returns>
        public GetUserArgs WithCustomNo(string customNo)
        {
            this.CustomNo = customNo;
            return this;
        }
        /// <summary>
        /// 赋值UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetUserArgs WithUserId(Guid? userId)
        {
            this.UserId = userId;
            return this;
        }

        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginName"></param>
        /// <param name="email"></param>
        /// <param name="customNo"></param>
        /// <returns></returns>
        public static GetUserArgs Create(Guid? userId = null, string loginName = null, string email = null, string customNo = null)
        {
            var args = new GetUserArgs();
            return args.WithUserId(userId).WithLoginName(loginName).WithEmail(email).WithCustomNo(customNo);
        }

        #endregion
    }

    /// <summary>
    /// 修改用户的组织
    /// </summary>
    public class ChangeOrgDto
    {
        public Guid UserId { get; set; }
        public Guid ToOrgId { get; set; }
        public Guid OperatorId { get; set; }
        public string Source { get; set; }
    }

    public class UserRepository
    {
        
    }
}
