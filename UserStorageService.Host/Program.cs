using Autofac;
using Autofac.Integration.Wcf;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.ServiceModel;
using System.Web.Http;
using UserStorageService.Host.Filters;
using UserStorageService.Read;
using WebApiCondoleTest.Controllers;

namespace UserStorageService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "http://localhost:51488";
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(_ => new LiteDbUserInfoDao(@"C:\Profiles.db")).AsSelf().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterApiControllers(typeof(ProfilesController).Assembly);
            containerBuilder.RegisterType<UserInfoProvider>().AsImplementedInterfaces();
            var container = containerBuilder.Build();
            var writeHost = WebApp.Start(host, appBuilder => 
            {
                var config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();

                config.Filters.Add(new ValidateModelAttribute());
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "import.json",
                    defaults: new { controller = "profiles" }
                );

                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
                appBuilder.UseAutofacMiddleware(container);
                appBuilder.UseAutofacWebApi(config);
                appBuilder.UseWebApi(config);
            });

            using (writeHost)
            {
                using (var readService = new ServiceHost(typeof(UserInfoProvider), new Uri[0]))
                {
                    readService.AddDependencyInjectionBehavior<IUserInfoProvider>(container);
                    readService.Open();
                    Console.ReadKey();
                    readService.Close();
                }
            }
        }
    }
}
