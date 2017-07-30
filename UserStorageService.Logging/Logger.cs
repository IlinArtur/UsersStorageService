using Serilog;
using System;

namespace UserStorageService.Logging
{
    public class Logger : ILogger
    {
        private readonly Serilog.Core.Logger logger;

        public Logger(string path)
        {
            var configuration = new LoggerConfiguration().WriteTo.RollingFile(path);
            logger = configuration.CreateLogger();
        }

        public void LogError(Exception exception, object request)
        {
            logger.Error(exception, "Request failed {@request}", request);
        }

        public void LogResponse(object response, object request)
        {
            logger.Information("Request: {@request} processed. Response {@response}", request, response);
        }
    }
}
