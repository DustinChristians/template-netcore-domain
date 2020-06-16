namespace CompanyName.ProjectName.Core.Abstractions.Tasks.Logging
{
    public interface IDatabaseEventLogCleanupTask
    {
        void DeleteOldEventLogs();
    }
}