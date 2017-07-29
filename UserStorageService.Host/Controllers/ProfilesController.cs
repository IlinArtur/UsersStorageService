using System.Web.Http;
using UserStorageService.Host;
using UserStoreageService.Host;
using UserStoreageService.Host.Models;

namespace WebApiCondoleTest.Controllers
{
    public class ProfilesController : ApiController
    {
        private LiteDbRepository repository;

        public ProfilesController(LiteDbRepository repository)
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
