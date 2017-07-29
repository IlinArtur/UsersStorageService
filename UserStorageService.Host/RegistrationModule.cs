using Autofac;
using Autofac.Integration.WebApi;
using System.Configuration;
using UserStorageService.Read;
using WebApiCondoleTest.Controllers;
using System;
using UserStorageService.Host.Services;

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
            builder.Register(ctx => new LiteDbUserInfoDao(ctx.ResolveNamed<string>(LiteDbConnectionString))).AsSelf()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterApiControllers(typeof(ProfilesController).Assembly);
            builder.RegisterType<UserInfoProvider>().AsImplementedInterfaces();
            builder.Register(SetupWriteService);
            builder.Register(SetupReadService);
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
