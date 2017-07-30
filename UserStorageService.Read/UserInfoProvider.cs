using System;
using System.ServiceModel;
using UserStorageService.Logging;

namespace UserStorageService.Read
{
    public class UserInfoProvider : IUserInfoProvider
    {
        private readonly IUserInfoDao userInfoDao;
        private readonly ILogger logger;

        public UserInfoProvider(IUserInfoDao userInfoDao, ILogger logger)
        {
            this.userInfoDao = userInfoDao;
            this.logger = logger;
        }

        public UserInfo GetUserInfo(Guid id)
        {
            var request = new { UserId = id };
            try
            {
                var userInfo = userInfoDao.GetUserInfo(id);
                if (userInfo == null)
                {
                    throw UserNotFound(id);
                }

                logger.LogResponse(response: userInfo, request: request);
                return userInfo;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, request);
                throw ex;
            }
        }

        private FaultException<UserNotFound> UserNotFound(Guid id)
        {
            return new FaultException<UserNotFound>(new UserNotFound { Id = id });
        }
    }
}
