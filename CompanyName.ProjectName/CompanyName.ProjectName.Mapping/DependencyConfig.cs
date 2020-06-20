using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Abstractions.Tasks.Logging;
using CompanyName.ProjectName.Infrastructure.Services;
using CompanyName.ProjectName.Infrastructure.Tasks.Logging;
using CompanyName.ProjectName.Logger;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName.Mapping
{
    public class DependencyConfig
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, string projectAssemblyName)
        {
            AddDatabases(services, configuration);
            AddDependenciesAutomatically(services);
            ConfigureAutomapper(services, projectAssemblyName);
            LoggerConfig.AddDependencies(services);
        }

        private static void AddDatabases(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CompanyNameProjectNameContext>(options =>
                options
                .UseSqlServer(
                    configuration.GetConnectionString("CompanyName.ProjectName.Repository"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(30))
                .EnableSensitiveDataLogging());
        }

        // Add any Assembly Names that need to be scanned for AutoMapper Mapping Profiles here
        private static void ConfigureAutomapper(IServiceCollection services, string projectAssemblyName)
        {
            var mappingConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddMaps(projectAssemblyName);
                    cfg.AddMaps("CompanyName.ProjectName.Infrastructure");
                    cfg.AddMaps("CompanyName.ProjectName.Repository");
                    cfg.AddExpressionMapping();
                    cfg.ConstructServicesUsing(
                        type => ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type));
                });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // The choice of seeding types used to get the exact assemblies is arbitrary as long as they
        // reside in the correct assemblies
        private static void AddDependenciesAutomatically(IServiceCollection services)
        {
            RegisterInterfaces("Service", services, Assembly.GetAssembly(typeof(IMessagesService)), Assembly.GetAssembly(typeof(MessagesService)));
            RegisterInterfaces("Task", services, Assembly.GetAssembly(typeof(IDatabaseEventLogCleanupTask)), Assembly.GetAssembly(typeof(DatabaseEventLogCleanupTask)));
            RegisterInterfaces("Repository", services, Assembly.GetAssembly(typeof(IMessagesRepository)), Assembly.GetAssembly(typeof(MessagesRepository)));
        }

        private static void RegisterInterfaces(string interfaceType, IServiceCollection services, Assembly coreAssembly, Assembly serviceAssembly)
        {
            var matches = serviceAssembly.GetTypes()
               .Where(t => t.Name.EndsWith(interfaceType, StringComparison.Ordinal) && t.GetInterfaces().Any(i => i.Assembly == coreAssembly))
               .Select(t => new
               {
                   serviceType = t.GetInterfaces().FirstOrDefault(i => i.Assembly == coreAssembly && !i.Name.ToLower().StartsWith("ibase")),
                   implementingType = t
               }).ToList();

            // Registers the interface to the implementation.
            foreach (var match in matches)
            {
                services.AddScoped(match.serviceType, match.implementingType);
            }
        }
    }
}
