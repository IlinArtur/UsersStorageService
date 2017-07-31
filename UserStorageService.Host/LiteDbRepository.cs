using LiteDB;
using System;
using UserStorageService.Read;
using UserStoreageService.Host.Models;

namespace UserStorageService.Host
{
    public class LiteDbRepository : IUserInfoDao
    {
        private const string profilesCollection = "profiles";
        private readonly string connectionString;

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
                var usersInfos = db.GetCollection<UserInfo>(profilesCollection);
                return usersInfos.FindOne(x => x.UserId == id);
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
