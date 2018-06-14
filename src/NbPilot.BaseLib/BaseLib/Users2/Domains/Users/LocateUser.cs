using System;

namespace ZQNB.BaseLib.Users2.Domains.Users
{
    /// <summary>
    /// 能定位某一用户的键
    /// </summary>
    public interface ILocateUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        Guid? UserId { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        string LoginName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// 自定义码，例如工号等
        /// </summary>
        string CustomNo { get; set; }
    }

    /// <summary>
    /// 能定位某一用户的键
    /// </summary>
    public class LocateUser : ILocateUser
    {
        public Guid? UserId { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string CustomNo { get; set; }
        
        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginName"></param>
        /// <param name="email"></param>
        /// <param name="customNo"></param>
        /// <returns></returns>
        public static ILocateUser Create(Guid? userId, string loginName = null, string email = null, string customNo = null)
        {
            var args = new LocateUser();
            return args.WithUserId(userId).WithLoginName(loginName).WithEmail(email).WithCustomNo(customNo);
        }
    }

    #region extensions

    public static class LocateUserExtensions
    {
        /// <summary>
        /// 赋值LoginName
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static ILocateUser WithLoginName(this ILocateUser user, string loginName)
        {
            user.LoginName = loginName;
            return user;
        }

        /// <summary>
        /// 赋值Email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static ILocateUser WithEmail(this ILocateUser user, string email)
        {
            user.Email = email;
            return user;
        }

        /// <summary>
        /// 赋值CustomNo
        /// </summary>
        /// <param name="user"></param>
        /// <param name="customNo"></param>
        /// <returns></returns>
        public static ILocateUser WithCustomNo(this ILocateUser user, string customNo)
        {
            user.CustomNo = customNo;
            return user;
        }

        /// <summary>
        /// 赋值UserId
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ILocateUser WithUserId(this ILocateUser user, Guid? userId)
        {
            user.UserId = userId;
            return user;
        }
    }

    #endregion
}
