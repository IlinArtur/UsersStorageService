using Owin;
using System.Web.Http;
using LiteDB;
using UserStoreageService.Host;
using UserStorageService.Host.Filters;
using UserStorageService.Read;

namespace UserStorageService.Host
{
    public class WebConfig
    {
        public void Configuration(IAppBuilder builder)
        {
            var mapper = BsonMapper.Global;

            mapper.Entity<SyncProfileRequest>().Id(x => x.UserId);
            mapper.Entity<UserInfo>().Id(x => x.UserId);

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ValidateModelAttribute());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "import.json",
                defaults: new { controller = "profiles" }
            );

            builder.UseWebApi(config);
        }
    }
}
