using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.TestUtilities
{
    public static class DatabaseUtilities
    {
        public static DbContextOptions<TContext> GetTestDbConextOptions<TContext>() where TContext : DbContext
        {
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };

            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            // Random Guids are used for database names so in memory databases aren't
            // reused between tests to ensure test isolation.  
            return new DbContextOptionsBuilder<TContext>()
                            .UseSqlite(connection)
                            .Options;
        }
    }
}
