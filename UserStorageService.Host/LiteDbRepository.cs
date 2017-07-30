using LiteDB;
using System;
using UserStorageService.Read;
using UserStoreageService.Host.Models;

namespace UserStorageService.Host
{
    public class LiteDbRepository : IUserInfoDao
    {
        private readonly string connectionString;
        private readonly string profilesCollection = "profiles";

        public LiteDbRepository(string connectionString)
        {
            this.connectionString = connectionString;
            var mapper = BsonMapper.Global;
            mapper.Entity<SyncProfileRequest>().Id(x => x.UserId);
            mapper.Entity<UserInfo>().Id(x => x.UserId);
        }

        public UserInfo GetUserInfo(Guid id)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<UserInfo>(profilesCollection);
                return collection.FindOne(x => x.UserId == id);
            }
        }

        public void Save(SyncProfileRequest profile)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<SyncProfileRequest>(profilesCollection);
                collection.Upsert(profile);
            }
        }
    }
}
