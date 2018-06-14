using System;
using System.Linq;
using ZQNB.Common;

namespace ZQNB.BaseLib.Users2.Domains.Users
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IQueryable<User> QueryUsers(QueryUsersArgs args);
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
        User TryGetUser(ILocateUser args);
        /// <summary>
        /// 修改用户组织
        /// </summary>
        void ChangeOrg(UserChangeOrg changeOrgDto);
        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        MessageResult SaveUser(IUser user);
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

    public class UserDto : IUser
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string CustomNo { get; set; }
        public string UserTypeCode { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public bool IsMale { get; set; }
        public double SpaceCapacity { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid OrgId { get; set; }
        public string FromSource { get; set; }
        public string UserPk { get; set; }
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository<User> _userRepository;

        public UserService(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IQueryable<User> QueryUsers(QueryUsersArgs args)
        {
            throw new NotImplementedException();
        }

        public bool TryCheckUserExist(CheckUserExistArgs args)
        {
            throw new NotImplementedException();
        }

        public User TryGetUser(ILocateUser args)
        {
            var tryGetUser = TryFindUser(args);
            return tryGetUser;
        }

        public MessageResult SaveUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = new MessageResult();

            if (user.Id == Guid.Empty)
            {
                result.Message = "保存失败";
                return result;
            }

            //todo validate self
            //var vr = ValidateSelf(user);
            //if (!vr.Success)
            //{
            //    return vr;
            //}

            var theUser = _userRepository.Get(user.Id);
            if (theUser == null)
            {
                result.Message = "没有找到用户:" + user.Id;
                return result;
            }

            ////todo validate logic
            //var vr = Validate(user);
            //if (!vr.Success)
            //{
            //    return vr;
            //}

            //validate ok, set properties todo
            //theUser.Xxx = user.Xxx;
            _userRepository.Edit(theUser);

            result.Message = "保存成功";
            result.Success = true;
            return result;
        }

        public void ChangeOrg(UserChangeOrg changeOrgDto)
        {
            throw new NotImplementedException();
        }

        private User TryFindUser(ILocateUser args)
        {
            if (args == null)
            {
                return null;
            }

            if (args.UserId != null)
            {
                return _userRepository.Get(args.UserId.Value);
            }

            var userQuery = _userRepository.Query();
            if (string.IsNullOrWhiteSpace(args.LoginName))
            {
                return userQuery.FirstOrDefault(x => x.LoginName == args.LoginName);
            }

            if (string.IsNullOrWhiteSpace(args.Email))
            {
                return userQuery.FirstOrDefault(x => x.Email == args.Email);
            }

            if (string.IsNullOrWhiteSpace(args.CustomNo))
            {
                return userQuery.FirstOrDefault(x => x.CustomNo == args.CustomNo);
            }

            return null;
        }

        private MessageResult Validate(IUser user)
        {
            //todo check unique keys and logics...
            throw new NotImplementedException();
        }

    }
}
