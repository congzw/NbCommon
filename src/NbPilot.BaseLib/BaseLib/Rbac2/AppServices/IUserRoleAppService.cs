using System.Collections.Generic;
using ZQNB.BaseLib.Rbac2.Domains.UserRoles;
using ZQNB.Common;

namespace ZQNB.BaseLib.Rbac2.AppServices
{
    /// <summary>
    /// [用户角色]关系Api
    /// </summary>
    public interface IUserRoleAppService
    {
        /// <summary>
        /// 查找用户在某个站点下的所有角色
        /// Guest, Member两个角色不需要主动分配，要根据上下文（用户是否登录）计算推算出来
        /// 所以这两个角色不在返回的列表之内，如果需要，应该进行Fix！
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IList<string> FindAssignRoleCodes(IFindAssignRolesArgs args);

        /// <summary>
        /// 赋予角色， 有则修改，无则增加
        /// Guest, Member两个角色不需要主动分配，要根据上下文（用户是否登录）计算推算出来
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        MessageResult AssignRoles(AssignRolesDto dto);
    }
}
