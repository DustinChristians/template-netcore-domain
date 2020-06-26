using System;
using CompanyName.ProjectName.Core.Abstractions.Repositories.Logging;
using CompanyName.ProjectName.Core.Abstractions.Tasks.Logging;
using CompanyName.ProjectName.Scheduler.Constants;
using Microsoft.Extensions.Configuration;

namespace CompanyName.ProjectName.Scheduler.Tasks.Logging
{
    public class DatabaseEventLogCleanupTask : IDatabaseEventLogCleanupTask
    {
        private readonly IConfiguration configuration;
        private readonly IEventLogRepository eventLogRepository;

        public DatabaseEventLogCleanupTask(IConfiguration configuration, IEventLogRepository eventLogRepository)
        {
            this.configuration = configuration;
            this.eventLogRepository = eventLogRepository;
        }

        public void DeleteOldEventLogs()
        {
            var days = configuration.GetValue<int>(ConfigurationKeys.DeleteDatabaseLogsOlderThanDays);

            eventLogRepository.DeleteLogsOlderThanDateTime(DateTime.Now.AddDays(-days));
        }
    }
}
