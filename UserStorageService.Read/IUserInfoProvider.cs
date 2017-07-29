using System;
using System.ServiceModel;

namespace UserStorageService.Read
{
    [ServiceContract]
    public interface IUserInfoProvider
    {
        UserInfo GetUserInfo(Guid id);
    }
}
