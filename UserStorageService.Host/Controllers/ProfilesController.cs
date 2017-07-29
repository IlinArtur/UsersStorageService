using LiteDB;
using System.Web.Http;
using UserStoreageService.Host;

namespace WebApiCondoleTest.Controllers
{
    public class ProfilesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(SyncProfileRequest request)
        {
            Save(request);
            return Ok();
        }

        [NonAction]
        private static void Save(SyncProfileRequest request)
        {
            using (var db = new LiteDatabase(@"C:\Profiles.db"))
            {
                var collection = db.GetCollection<SyncProfileRequest>("profiles");
                collection.Upsert(request);
            }
        }
    }
}
