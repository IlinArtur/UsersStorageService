using NUnit.Framework;
using System;
using UserStorageService.Read;

namespace UserStoreageService.Read.Tests
{
    [TestFixture]
    [Category("Fast")]
    public class UserInfoProviderTests
    {
        [Test]
        public void GetUserInfo_ByDefault_ReturnsExistingUser()
        {
            var userInfoProvider = new UserInfoProvider();

            var userInfo = userInfoProvider.GetUserInfo(Guid.Empty);

            userInfo.Should().NotBeNull();
        }
    }
}
