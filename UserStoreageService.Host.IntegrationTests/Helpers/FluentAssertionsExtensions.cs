using FluentAssertions.Primitives;
using System.Net;
using System.Net.Http;

namespace UserStoreageService.Host.IntegrationTests.Helpers
{
    public static class FluentAssertionsExtensions
    {
        public static void HaveStatusCode(this ObjectAssertions objectAssertion, HttpStatusCode statusCode)
        {
            objectAssertion.Match<HttpResponseMessage>(x => x.StatusCode == statusCode);
        }
    }
}
