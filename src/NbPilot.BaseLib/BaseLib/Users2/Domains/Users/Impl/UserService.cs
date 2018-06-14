using System;
using System.Linq;
using ZQNB.Common;

namespace ZQNB.BaseLib.Users2.Domains.Users.Impl
{
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