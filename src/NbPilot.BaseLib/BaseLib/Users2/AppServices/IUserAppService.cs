using System;
using ZQNB.BaseLib.Users2.Domains.Users;
using ZQNB.Common;

namespace ZQNB.BaseLib.Users2.AppServices
{
    /// <summary>
    /// 用户通用接口
    /// </summary>
    public interface IUserAppService
    {
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        UserVo GetUserVo(GetUserVoArgs args);

        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        MessageResult SaveUserVo(UserVo vo);
    }

    /// <summary>
    /// 获取用户的参数
    /// </summary>
    public class GetUserVoArgs : ILocateUser
    {
        public Guid? UserId { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string CustomNo { get; set; }
    }

    /// <summary>
    /// 用户基本信息VO
    /// </summary>
    public class UserVo : BaseViewModel, IUser
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

        public MessageResult ValidateSelf()
        {
            //todo
            throw new NotImplementedException();
        }
    }
}
