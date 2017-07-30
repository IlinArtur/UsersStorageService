using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using UserStorageService.Host.Logging;

namespace UserStorageService.Host.Filters
{
    public class LoggingFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger logger;

        public LoggingFilterAttribute(ILogger logger)
        {
            this.logger = logger;
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationTooken)
        {
            var response = await ParseResponse(actionExecutedContext.Response);
            var request = ParseRequest(actionExecutedContext.Request, actionExecutedContext.ActionContext.ActionArguments);
            logger.LogResponse(response, request);
        }

        private object ParseRequest(HttpRequestMessage request, Dictionary<string, object> arguments)
        {
            var header = request.ToString();
            var content = arguments;
            return new { header, content };
        }

        private async Task<object> ParseResponse(HttpResponseMessage response)
        {
            string content = null;
            var header = response.ToString();
            if (response.Content != null)
            {
                content = await response.Content.ReadAsStringAsync();
            }

            return new { header, content };
        }

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var request = ParseRequest(actionExecutedContext.Request, actionExecutedContext.ActionContext.ActionArguments);
            logger.LogError(actionExecutedContext.Exception, request: request);
            return Task.FromResult(0);
        }
    }
}
