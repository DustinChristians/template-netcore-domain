using CompanyName.ProjectName.Core.Models.Logging;

namespace CompanyName.ProjectName.Core.Abstractions.Services.Logging
{
    public interface ILoggerService
    {
        void Log(LogEntry entry);
    }
}
