using LiteDB;
using System;
using UserStorageService.Read;

namespace UserStorageService.Host
{
    class LiteDbUserInfoDao : IUserInfoDao
    {
        private readonly string connectionString;

        public LiteDbUserInfoDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public UserInfo GetUserInfo(Guid id)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<UserInfo>("profiles");
                return collection.FindOne(x => x.UserId == id);
            }
        }
    }
}
