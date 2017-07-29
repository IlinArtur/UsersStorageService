using System;
using System.ServiceModel;

namespace UserStorageService.Read
{
    [ServiceContract]
    public interface IUserInfoProvider
    {
        [OperationContract]
        [FaultContract(typeof(UserNotFound))]
        UserInfo GetUserInfo(Guid id);
    }
}
