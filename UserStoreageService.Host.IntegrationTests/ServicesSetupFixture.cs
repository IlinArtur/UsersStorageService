using Autofac;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using UserStorageService.Host;
using UserStorageService.Host.Services;
using UserStorageService.Logging;
using UserStorageService.Read;
using UserStoreageService.Host.Models;

namespace UserStoreageService.Host.IntegrationTests
{
    [SetUpFixture]
    public class ServicesSetupFixture
    {
        private const string WriteServiceAddress = "http://localhost:51488";
        private const string ReadServiceAddress = "http://localhost:8081/Service1";
        private const string LiteDbConnectionstring = "C:\\Profiles.db";
        private static ServicesSetupFixture instance;
        private readonly ChannelFactory<IUserInfoProvider> channelFactory;
        private ILifetimeScope container;
        private IEnumerable<IService> services;

        public ServicesSetupFixture()
        {
            channelFactory = new ChannelFactory<IUserInfoProvider>(new BasicHttpBinding(), new EndpointAddress("http://localhost:8081/Service1/"));
            instance = this;
        }


        [OneTimeSetUp]
        public void StartServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<RegistrationModule>();
            builder.RegisterInstance(WriteServiceAddress).Named<string>(RegistrationModule.WriteHostAddress);
            builder.RegisterInstance(ReadServiceAddress).Named<string>(RegistrationModule.ReadHostAddress);
            builder.RegisterInstance(LiteDbConnectionstring).Named<string>(RegistrationModule.LiteDbConnectionString);
            builder.RegisterInstance(A.Fake<ILogger>());
            container = builder.Build();
            services = container.Resolve<IEnumerable<IService>>();
            DoWithServices(x => x.Start());
        }

        private void DoWithServices(Action<IService> execute)
        {
            foreach (var service in services)
            {
                execute(service);
            }
        }

        [OneTimeTearDown]
        public void StopServices()
        {
            
            DoWithServices(x => x.Stop());
            container.Dispose();            
        }

        public static Task<HttpResponseMessage> SendJson(Guid userId = default(Guid), string countryIsoCode = "ru", string locale = "ru-RU")
        {
            return instance.SenJsonInternal(userId, countryIsoCode, locale);
        }

        private Task<HttpResponseMessage> SenJsonInternal(Guid userId, string countryIsoCode, string locale)
        {
            var client = new HttpClient();
            var request = new SyncProfileRequest { UserId = userId, CountryIsoCode = countryIsoCode, Locale = locale };
            return client.PostAsJsonAsync("http://localhost:51488/import.json", request);
        }

        public static UserInfo GetUserInfo(Guid id)
        {
            return instance.GetUserInfoInternal(id);
        }

        private UserInfo GetUserInfoInternal(Guid id)
        {
            var client = channelFactory.CreateChannel();
            try
            {
                return client.GetUserInfo(id);
            }
            finally
            {
                (client as IClientChannel)?.Close();
            }
        }
    }
}
