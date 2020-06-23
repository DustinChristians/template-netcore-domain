using System;
using CompanyName.ProjectName.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CompanyName.ProjectName.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Will create a file logger before the database exists
                Log.Logger = LoggerConfig.CreateLogger();

                Log.Information("Starting Up");

                var host = CreateHostBuilder(args).Build();

                DatabaseConfig.SeedDatabases(host);

                // Will create a database logger now that the database exists
                Log.Logger = LoggerConfig.CreateLogger();

                host.Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog();
                });
    }
}
