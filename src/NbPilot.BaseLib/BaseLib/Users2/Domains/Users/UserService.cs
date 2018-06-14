using System;

namespace ZQNB.BaseLib.Users2.Domains.Users
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 查找某个用户
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        User GetUser(IUserUniqueKeys args);
    }
    
    /// <summary>
    /// 唯一检索用户的标识
    /// </summary>
    public interface IUserUniqueKeys
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// 支持作为凭证登录的三种方式（都是全局唯一）
        /// 必填项（1）
        /// </summary>
        string LoginName { get; set; }

        /// <summary>
        /// 邮箱
        /// 选填项（2）
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// 工号
        /// 选填项（3）
        /// </summary>
        string CustomNo { get; set; }
    }

    public class UserService : IUserService
    {
        public User GetUser(IUserUniqueKeys args)
        {
            throw new NotImplementedException();
        }
    }
}
