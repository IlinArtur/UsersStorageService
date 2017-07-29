using FluentAssertions;
using NUnit.Framework;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using UserStorageService.Read;
using static UserStoreageService.Host.IntegrationTests.ServicesSetupFixture;

namespace UserStoreageService.Host.IntegrationTests
{
    [TestFixture]
    [Category("Slow")]
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

        [Test]
        public async Task ReadService_InvalidUserId_ShouldReturnFaultException()
        {
            await SendJson(userId_DeadBeef);

            Action gettingUserInfo  = () => GetUserInfo(Guid.Empty);

            gettingUserInfo.ShouldThrow<FaultException<UserNotFound>>().Which.Detail.Id.Should().Be(Guid.Empty);
        }
    }
}
