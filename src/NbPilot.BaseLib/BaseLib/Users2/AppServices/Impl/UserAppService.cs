using System;
using ZQNB.BaseLib.Users2.Domains.Users;
using ZQNB.Common;

namespace ZQNB.BaseLib.Users2.AppServices.Impl
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserService _userService;

        public UserAppService(IUserService userService)
        {
            _userService = userService;
        }

        public UserVo GetUserVo(GetUserVoArgs args)
        {
            var tryGetUser = _userService.TryGetUser(args);
            var vo = tryGetUser.ToMapped<UserVo>();
            //todo dynamic logic
            return vo;
        }

        public MessageResult SaveUserVo(UserVo vo)
        {
            var result = _userService.SaveUser(vo);
            return result;
        }
    }
}
