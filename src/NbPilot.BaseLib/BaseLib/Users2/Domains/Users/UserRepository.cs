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
    }

    //todo
    public class UserRepository 
    {
        
    }
}
