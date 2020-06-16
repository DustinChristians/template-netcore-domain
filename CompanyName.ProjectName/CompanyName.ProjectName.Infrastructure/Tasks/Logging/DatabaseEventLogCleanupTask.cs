using System;
using CompanyName.ProjectName.Core.Abstractions.Repositories.Logging;
using CompanyName.ProjectName.Core.Abstractions.Tasks.Logging;

namespace CompanyName.ProjectName.Infrastructure.Tasks.Logging
{
    public class DatabaseEventLogCleanupTask : IDatabaseEventLogCleanupTask
    {
        private readonly IEventLogRepository eventLogRepository;

        public DatabaseEventLogCleanupTask(IEventLogRepository eventLogRepository)
        {
            this.eventLogRepository = eventLogRepository;
        }

        public void DeleteOldEventLogs()
        {
            eventLogRepository.DeleteLogsOlderThanDateTime(DateTime.Now.AddMinutes(-5));
        }
    }
}
