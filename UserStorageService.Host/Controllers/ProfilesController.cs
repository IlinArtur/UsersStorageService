using System.Web.Http;
using UserStorageService.Host;
using UserStoreageService.Host;

namespace WebApiCondoleTest.Controllers
{
    public class ProfilesController : ApiController
    {
        private LiteDbUserInfoDao repository;

        public ProfilesController(LiteDbUserInfoDao repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public IHttpActionResult Post(SyncProfileRequest request)
        {
            repository.Save(request);
            return Ok();
        }
    }
}
