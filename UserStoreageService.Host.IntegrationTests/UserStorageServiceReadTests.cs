using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using UserStorageService.Read;

namespace UserStoreageService.Host.IntegrationTests
{
    [TestFixture]
    public class UserStorageServiceReadTests
    {
        private ChannelFactory<IUserInfoProvider> channelFactory;
        private IUserInfoProvider client;
        private readonly Guid userId_DeadBeef = Guid.Parse("00000000-0000-dead-beef-000000000001");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            channelFactory = new ChannelFactory<IUserInfoProvider>(new BasicHttpBinding(), new EndpointAddress("http://localhost:8081/Service1/"));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            channelFactory.Close();
        }

        [SetUp]
        public void SetUp()
        {
            client = channelFactory.CreateChannel();
        }

        [TearDown]
        public void TearDown()
        {
            (client as IClientChannel)?.Close();
        }

        [Test]
        public async Task ReadService_ByDefault_ShouldReturnUserInfo()
        {
            await SendJson(userId_DeadBeef);

            var userInfo = GetUserInfo(userId_DeadBeef);

            userInfo.Should().NotBeNull();
        }

        private UserInfo GetUserInfo(Guid id)
        {
            return client.GetUserInfo(id);
        }

        private Task<HttpResponseMessage> SendJson(Guid userId = default(Guid), string countryIsoCode = "ru", string locale = "ru-RU")
        {
            var client = new HttpClient();
            var request = new SyncProfileRequest { UserId = userId, CountryIsoCode = countryIsoCode, Locale = locale };
            return client.PostAsJsonAsync("http://localhost:51488/import.json", request);
        }
    }
}
