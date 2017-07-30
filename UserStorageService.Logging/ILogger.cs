using System;

namespace UserStorageService.Logging
{
    public interface ILogger
    {
        void LogResponse(object response, object request);
        void LogError(Exception exception, object request);
    }
}
