using Autofac;
using Autofac.Integration.WebApi;
using System.Configuration;
using UserStorageService.Read;
using WebApiCondoleTest.Controllers;
using System;
using UserStorageService.Host.Services;
using UserStorageService.Host.Filters;
using System.Web.Http.Filters;
using System.IO;
using UserStorageService.Logging;

namespace UserStorageService.Host
{
    public class RegistrationModule : Module
    {
        public const string WriteHostAddress = "WriteHostAddress";
        public const string ReadHostAddress = "ReadHostAddress";
        public const string LiteDbConnectionString = "LiteDbConnectionString";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => ConfigurationManager.AppSettings[WriteHostAddress]).Named<string>(WriteHostAddress);
            builder.Register(_ => ConfigurationManager.AppSettings[ReadHostAddress]).Named<string>(ReadHostAddress);
            builder.Register(_ => ConfigurationManager.AppSettings[LiteDbConnectionString]).Named<string>(LiteDbConnectionString);
            builder.Register(ctx => new LiteDbRepository(ctx.ResolveNamed<string>(LiteDbConnectionString))).AsSelf()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterApiControllers(typeof(ProfilesController).Assembly);
            builder.RegisterType<UserInfoProvider>().AsImplementedInterfaces();
            builder.Register(SetupLogger).SingleInstance();
            builder.RegisterType<ValidateModelAttribute>().As<ActionFilterAttribute>();
            builder.RegisterType<LoggingFilterAttribute>().As<ActionFilterAttribute>();
            builder.Register(SetupWriteService);
            builder.Register(SetupReadService);
        }

        private ILogger SetupLogger(IComponentContext arg)
        {
            var defaultPath = Path.Combine("logs", "log-{Date}.txt");
            var logPath = ConfigurationManager.AppSettings["logPath"] ??  defaultPath;
            return new Logger(Path.GetFullPath(logPath));
        }

        private IService SetupReadService(IComponentContext context)
        {
            var address = new Uri(context.ResolveNamed<string>(ReadHostAddress));
            return new ReadService(context.Resolve<ILifetimeScope>(), address);
        }

        private IService SetupWriteService(IComponentContext context)
        {
            return new WriteService(context.Resolve<ILifetimeScope>(), context.ResolveNamed<string>(WriteHostAddress));
        }
    }
}
