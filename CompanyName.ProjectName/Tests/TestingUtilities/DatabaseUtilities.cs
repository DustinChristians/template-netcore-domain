using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CompanyName.ProjectName.TestUtilities
{
    public static class DatabaseUtilities
    {
        public static DbContextOptions<TContext> GetTestDbConextOptions<TContext>() where TContext : DbContext
        {
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };

            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            return new DbContextOptionsBuilder<TContext>()
                // Log all Entity Framework Core information to
                // the test log for easy troubleshooting
                .UseLoggerFactory(new LoggerFactory(
                    new[] { new LogToActionLoggerProvider((log) =>
                    {
                        TestContext.Out.WriteLine(log);
                    }) }))
                .UseSqlite(connection)
                .Options;
        }
    }
}
