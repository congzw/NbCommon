using System;
using ZQNB.Common.Data.Model;

namespace ZQNB.BaseLib.Users2.Domains.Users
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public interface IUser
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

        /// <summary>
        /// 用户类型
        /// </summary>
        string UserTypeCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        bool IsMale { get; set; }

        /// <summary>
        /// 空间容量
        /// </summary>
        double SpaceCapacity { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        string HomeAddress { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// 用户必须有隶属组织，必填项
        /// </summary>
        Guid OrgId { get; set; }

        /// <summary>
        /// 外部导入的预留字段 //外部，某厂商等，不是必填项
        /// </summary>
        string FromSource { get; set; }

        /// <summary>
        /// 外部导入的预留字段 //可以用此字段存取外来的Id，不是必填项
        /// </summary>
        string UserPk { get; set; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class User : NbEntity<User>, IUser
    {
        /// <summary>
        /// 支持作为凭证登录的三种方式（都是全局唯一）
        /// 必填项（1）
        /// </summary>
        public virtual string LoginName { get; set; }

        /// <summary>
        /// 邮箱
        /// 选填项（2）
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 工号
        /// 选填项（3）
        /// </summary>
        public virtual string CustomNo { get; set; }

        //======================用户信息=============
        /// <summary>
        /// 用户类型
        /// </summary>
        public virtual string UserTypeCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool IsMale { get; set; }
        /// <summary>
        /// 空间容量
        /// </summary>
        public virtual double SpaceCapacity { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public virtual string PhoneNumber { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public virtual string HomeAddress { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 用户必须有隶属组织，必填项
        /// </summary>
        public virtual Guid OrgId { get; set; }

        /// <summary>
        /// 外部导入的预留字段 //外部，某厂商等，不是必填项
        /// </summary>
        public virtual string FromSource { get; set; }
        /// <summary>
        /// 外部导入的预留字段 //可以用此字段存取外来的Id，不是必填项
        /// </summary>
        public virtual string UserPk { get; set; }
    }

}
