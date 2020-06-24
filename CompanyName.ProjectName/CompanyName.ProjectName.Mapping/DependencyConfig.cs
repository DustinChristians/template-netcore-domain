using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Abstractions.Tasks.Logging;
using CompanyName.ProjectName.Core.Types;
using CompanyName.ProjectName.Infrastructure.Services;
using CompanyName.ProjectName.Infrastructure.Tasks.Logging;
using CompanyName.ProjectName.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName.Mapping
{
    public class DependencyConfig
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, string projectAssemblyName)
        {
            DatabaseConfig.AddDatabases(services, configuration);
            AddDependenciesAutomatically(services);
            ConfigureAutomapper(services, projectAssemblyName);
            LoggerConfig.AddDependencies(services);
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
            RegisterInterfaces("Task", services, Assembly.GetAssembly(typeof(IDatabaseEventLogCleanupTask)), Assembly.GetAssembly(typeof(DatabaseEventLogCleanupTask)), DependencyTypes.Transient);
            RegisterInterfaces("Repository", services, Assembly.GetAssembly(typeof(IMessagesRepository)), Assembly.GetAssembly(typeof(MessagesRepository)));
        }

        private static void RegisterInterfaces(string interfaceType, IServiceCollection services, Assembly coreAssembly, Assembly serviceAssembly, DependencyTypes type = DependencyTypes.Scoped)
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
                switch (type)
                {
                    case DependencyTypes.Scoped:
                        services.AddScoped(match.serviceType, match.implementingType);
                        break;
                    case DependencyTypes.Singleton:
                        services.AddSingleton(match.serviceType, match.implementingType);
                        break;
                    case DependencyTypes.Transient:
                        services.AddTransient(match.serviceType, match.implementingType);
                        break;
                }
            }
        }
    }
}
