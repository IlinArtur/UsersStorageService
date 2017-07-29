using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Web.Http;
using UserStorageService.Host.Filters;

namespace UserStorageService.Host
{
    public class WriteService : IService, IDisposable
    {
        private readonly ILifetimeScope context;
        private string address;
        private IDisposable host;

        public WriteService(ILifetimeScope context, string address)
        {
            this.context = context;
            this.address = address;
        }

        public void Start()
        {
            host = WebApp.Start(address, appBuilder =>
            {
                var config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();

                config.Filters.Add(new ValidateModelAttribute());
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "import.json",
                    defaults: new { controller = "profiles" }
                );

                config.DependencyResolver = new AutofacWebApiDependencyResolver(context);
                appBuilder.UseAutofacMiddleware(context);
                appBuilder.UseAutofacWebApi(config);
                appBuilder.UseWebApi(config);
            });
        }

        public void Stop()
        {
            host?.Dispose();
        }

        public void Dispose()
        {
            host?.Dispose();
        }
    }
}
