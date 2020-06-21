using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CompanyName.ProjectName.Mapping
{
    public class LoggerConfig
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
        .Build();

        public static Serilog.ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration) // modify configuration settings in appsettings.json
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public static void AddDependencies(IServiceCollection services)
        {
            // Give the Serilog CorrelationId Enricher access to the requests HttpContext so that it
            // can attach the correlation ID to the request/response.
            services.AddHttpContextAccessor();
        }

        public static void Configure(ILoggerFactory loggerFactory)
        {
            // Add Serilog to the Logging Pipeline
            loggerFactory.AddSerilog();
        }
    }
}
