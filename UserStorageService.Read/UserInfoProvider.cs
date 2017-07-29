using System;
using System.ServiceModel;

namespace UserStorageService.Read
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UserInfoProvider : IUserInfoProvider
    {
        private IUserInfoDao userInfoDao;

        public UserInfoProvider(IUserInfoDao userInfoDao)
        {
            this.userInfoDao = userInfoDao;
        }

        public UserInfo GetUserInfo(Guid id)
        {
            return userInfoDao.GetUserInfo(id) ?? throw UserNotFound(id);
        }

        private FaultException<UserNotFound> UserNotFound(Guid id)
        {
            return new FaultException<UserNotFound>(new UserNotFound { Id = id });
        }
    }
}
