using System;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.TestUtilities
{
    public static class DatabaseUtilities
    {
        public static DbContextOptions<TContext> GetTestDbConextOptions<TContext>() where TContext : DbContext
        {
            // Random Guids are used for database names so in memory databases aren't
            // reused between tests to ensure test isolation.  
            return new DbContextOptionsBuilder<TContext>()
                            .UseInMemoryDatabase(new Guid().ToString())
                            .Options;
        }
    }
}
