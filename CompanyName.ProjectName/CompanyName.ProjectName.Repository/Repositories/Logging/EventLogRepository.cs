using System;
using CompanyName.ProjectName.Core.Abstractions.Repositories.Logging;
using CompanyName.ProjectName.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Repositories.Logging
{
    public class EventLogRepository : IEventLogRepository
    {
        protected CompanyNameProjectNameContext context;

        public EventLogRepository(CompanyNameProjectNameContext context)
        {
            this.context = context;
        }

        public void DeleteLogsOlderThanDateTime(DateTime dateTime)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM EventLog WHERE TimeStamp < {0}", dateTime);
        }
    }
}
