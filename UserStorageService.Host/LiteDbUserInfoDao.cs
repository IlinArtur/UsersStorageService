using LiteDB;
using System;
using UserStorageService.Read;
using UserStoreageService.Host;

namespace UserStorageService.Host
{
    class LiteDbUserInfoDao : IUserInfoDao
    {
        private readonly string connectionString;
        private readonly string profilesCollection = "profiles";

        public LiteDbUserInfoDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public UserInfo GetUserInfo(Guid id)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<UserInfo>(profilesCollection);
                return collection.FindOne(x => x.UserId == id);
            }
        }

        private void Save(SyncProfileRequest request)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<SyncProfileRequest>(profilesCollection);
                collection.Upsert(request);
            }
        }
    }
}
