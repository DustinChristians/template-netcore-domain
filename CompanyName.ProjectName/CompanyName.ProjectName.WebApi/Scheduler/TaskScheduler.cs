using CompanyName.ProjectName.Core.Abstractions.Tasks.Logging;
using CompanyName.ProjectName.Infrastructure.Tasks.Logging;
using Hangfire;

namespace CompanyName.ProjectName.WebApi.Scheduler
{
    public class TaskScheduler
    {
        private readonly IDatabaseEventLogCleanupTask databaseEventLogCleanupTask;

        public TaskScheduler(IDatabaseEventLogCleanupTask databaseEventLogCleanupTask)
        {
            this.databaseEventLogCleanupTask = databaseEventLogCleanupTask;
        }

        public static void ScheduleRecurringTasks()
        {
            RecurringJob.RemoveIfExists(nameof(DatabaseEventLogCleanupTask.DeleteOldEventLogs));
            RecurringJob.AddOrUpdate<IDatabaseEventLogCleanupTask>(nameof(DatabaseEventLogCleanupTask),
            task => task.DeleteOldEventLogs(),
            "0 */5 * ? * *");
        }
    }
}
