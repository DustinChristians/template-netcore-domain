using System;

namespace CompanyName.ProjectName.Core.Abstractions.Repositories.Logging
{
    public interface IEventLogRepository
    {
        void DeleteLogsOlderThanDateTime(DateTime dateTime);
    }
}