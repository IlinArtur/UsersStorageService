﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using UserStoreageService.Host.IntegrationTests.Helpers;
using static UserStoreageService.Host.IntegrationTests.ServicesSetupFixture;

namespace UserStoreageService.Host.IntegrationTests
{
    [TestFixture]
    [Category("Slow")]
    public class UserStorageServiceWriteTests
    {
        private readonly Guid userId_DeadBeef = Guid.Parse("00000000-0000-dead-beef-000000000001");

        [Test]
        public async Task WriteService_ByDefault_ShouldRespondOK()
        {
            var response = await SendJson(userId_DeadBeef);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Test]
        public async Task WriteService_BeDefault_ShouldUpdateExistingUser()
        {
            
            await SendJson(userId_DeadBeef);

            var response = await SendJson(userId_DeadBeef);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [TestCase("abc")]
        [TestCase("ab1")]
        [TestCase("12")]
        [TestCase("")]
        [TestCase("a")]
        [TestCase(null)]
        [TestCase("цп")]
        public async Task WriteService_BadCountryCode_ShouldResondBadRequest(string countyIsoCode)
        {
            var response = await SendJson(userId_DeadBeef, countyIsoCode);

            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        [TestCase("abc")]
        [TestCase("ab1")]
        [TestCase("12")]
        [TestCase("")]
        [TestCase("a")]
        [TestCase(null)]
        [TestCase("цп")]
        [TestCase("en-usus")]
        [TestCase("en-us-us")]
        public async Task WriteService_BadLocale_ShouldRespondbadrequest(string locale)
        {
            var response = await SendJson(userId_DeadBeef, locale: locale);

            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task WriteService_GoodLocale_shouldResponseOk()
        {
            var response = await SendJson(userId_DeadBeef, locale: "ru");

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
    }
}