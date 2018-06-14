using System;
using System.Collections.Generic;
using System.Linq;

namespace ZQNB.BaseLib.Rbac2.Domains.UserRoles
{
    /// <summary>
    /// [用户角色]关系服务
    /// </summary>
    public interface IUserRoleService
    {
        /// <summary>
        /// 查找用户在某个站点下的所有角色
        /// Guest, Member两个角色不需要主动分配，要根据上下文（用户是否登录）计算推算出来
        /// 所以这两个角色不在返回的列表之内，如果需要，应该进行Fix！
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IList<UserRoleDto> FindAssignRoles(IFindAssignRolesArgs args);
        
        /// <summary>
        /// 赋予角色， 有则修改，无则增加
        /// Guest, Member两个角色不需要主动分配，要根据上下文（用户是否登录）计算推算出来
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool AssignRoles(AssignRolesDto dto);

        /// <summary>
        /// 清除在某个站点下的某个用户的所有赋予角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool ClearRoles(ClearRolesDto dto);
    }

    /// <summary>
    /// [用户角色]关系
    /// </summary>
    public class UserRoleDto : IUserRole
    {
        public Guid SiteId { get; set; }
        public string UserLoginName { get; set; }
        public string RoleCode { get; set; }
    }

    /// <summary>
    /// 查找用户在某个站点下的所有角色的参数
    /// </summary>
    public interface IFindAssignRolesArgs
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        Guid SiteId { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        string UserLoginName { get; set; }
        /// <summary>
        /// 是否根据登录状态， 自动补齐自动分配角色 isLogin ? "Guest, Member" : "Guest"
        /// </summary>
        bool AutoFix { get; set; }
    }

    /// <summary>
    /// 查找用户在某个站点下的所有角色的参数
    /// </summary>
    public class FindAssignRolesArgs : IFindAssignRolesArgs
    {
        public Guid SiteId { get; set; }
        public string UserLoginName { get; set; }
        public bool AutoFix { get; set; }
    }

    /// <summary>
    /// 赋予角色的参数
    /// </summary>
    public class AssignRolesDto
    {
        public AssignRolesDto()
        {
            RoleCodes = new List<string>();
        }

        /// <summary>
        /// 站点Id,不可以为空
        /// </summary>
        public Guid SiteId { get; set; }
        /// <summary>
        /// 用户登录名
        /// </summary>
        public string UserLoginName { get; set; }
        /// <summary>
        /// 所有的角色代码
        /// </summary>
        public IEnumerable<string> RoleCodes { get; set; }
        /// <summary>
        /// 是否先按siteId和userLoginName清空roleCodes， 默认不清空
        /// </summary>
        public bool ClearFirst { get; set; }

    }

    /// <summary>
    /// 清除所有站点下的某个用户的所有赋予角色的参数
    /// </summary>
    public class ClearRolesDto
    {
        /// <summary>
        /// 站点Id,为空则清空所有站的角色
        /// </summary>
        public Guid? SiteId { get; set; }
        /// <summary>
        /// 用户登录名
        /// </summary>
        public string UserLoginName { get; set; }
    }

    #region extensions

    /// <summary>
    /// UserRoleService Extensions
    /// </summary>
    public static class UserRoleServiceExtensions
    {
        /// <summary>
        /// 查找用户在某个站点下的所有角色
        /// Guest, Member两个角色不需要主动分配，要根据上下文（用户是否登录）计算推算出来
        /// 所以这两个角色不在返回的列表之内，如果需要，应该进行Fix！
        /// </summary>
        /// <param name="userRoleService"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IList<string> FindAssignRoleCodes(this IUserRoleService userRoleService, IFindAssignRolesArgs args)
        {
            var userRoleDtos = userRoleService.FindAssignRoles(args);
            return userRoleDtos.Select(x => x.RoleCode).ToList();
        }
    }

    #endregion
}
