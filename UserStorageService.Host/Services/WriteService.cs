using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Filters;

namespace UserStorageService.Host.Services
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

                var filters = context.Resolve<IEnumerable<ActionFilterAttribute>>();
                config.Filters.AddRange(filters);
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
            Stop();
        }
    }
}
