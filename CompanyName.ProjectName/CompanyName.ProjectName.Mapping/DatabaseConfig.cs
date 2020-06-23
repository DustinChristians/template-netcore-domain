using System;
using CompanyName.ProjectName.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.Mapping
{
    public class DatabaseConfig
    {
        public static void SeedDatabases(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CompanyNameProjectNameContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DependencyConfig>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }

        public static void AddDatabases(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CompanyNameProjectNameContext>(options =>
                options
                .UseSqlServer(
                    configuration.GetConnectionString("CompanyName.ProjectName.Repository"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(30))
                .EnableSensitiveDataLogging());
        }
    }
}
