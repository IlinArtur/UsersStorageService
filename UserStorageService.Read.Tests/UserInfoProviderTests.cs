using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.ServiceModel;
using UserStorageService.Logging;
using UserStorageService.Read;

namespace UserStoreageService.Read.Tests
{
    [TestFixture]
    [Category("Fast")]
    public class UserInfoProviderTests
    {
        private static readonly Guid UserId_deadbeef = Guid.Parse("00000000-0000-dead-beef-000000000000");

        [Test]
        public void GetUserInfo_ByDefault_ReturnsExistingUser()
        {
            var userInfo = new UserInfo { UserId = Guid.Empty, Locale = "ru-RU" };
            var fakeUserInfoDao = A.Fake<IUserInfoDao>();
            A.CallTo(() => fakeUserInfoDao.GetUserInfo(Guid.Empty)).Returns(userInfo);
            var userInfoProvider = CreateUserInfoProvider(fakeUserInfoDao);

            var result = userInfoProvider.GetUserInfo(Guid.Empty);

            result.Should().Be(userInfo);
        }

        private static UserInfoProvider CreateUserInfoProvider(IUserInfoDao fakeUserInfoDao)
        {
            return new UserInfoProvider(fakeUserInfoDao, A.Fake<ILogger>());
        }

        [Test]
        public void GetUserInfo_InvalidUserId_ThrowsUserNotFound()
        {
            var fakeUserInfoDao = A.Fake<IUserInfoDao>();
            A.CallTo(() => fakeUserInfoDao.GetUserInfo(A<Guid>.Ignored)).Returns(null);
            var userInfoProvider = CreateUserInfoProvider(fakeUserInfoDao);

            Action gettingUserInfo = () => userInfoProvider.GetUserInfo(UserId_deadbeef);

            gettingUserInfo.ShouldThrow<FaultException<UserNotFound>>().Which.Detail.Id.Should().Be(UserId_deadbeef);
        }
    }
}
