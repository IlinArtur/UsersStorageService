using System;

namespace UserStorageService.Read
{
    public interface IUserInfoDao
    {
        UserInfo GetUserInfo(Guid id);
    }
}
