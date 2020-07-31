using System;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.TestUtilities
{
    public class LogToActionLoggerProvider : ILoggerProvider
    {
        private readonly Action<string> efCoreLogAction;
        private readonly LogLevel logLevel;

        public LogToActionLoggerProvider(
            Action<string> efCoreLogAction,
            LogLevel logLevel = LogLevel.Information)
        {
            this.efCoreLogAction = efCoreLogAction;
            this.logLevel = logLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EFCoreLogger(this.efCoreLogAction, logLevel);
        }

        public void Dispose()
        {
            // nothing to dispose
        }
    }
}
