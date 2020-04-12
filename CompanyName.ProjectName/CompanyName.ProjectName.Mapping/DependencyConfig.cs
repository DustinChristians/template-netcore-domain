using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Infrastructure.Services;
using CompanyName.ProjectName.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName.Mapping
{
    public class DependencyConfig
    {
        public static void Register(IServiceCollection services)
        {
            AddDependenciesAutomatically(services);
            ConfigureAutomapper(services);
        }

        // Add any Assembly Names that need to be scanned for AutoMapper Mapping Profiles here
        private static void ConfigureAutomapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddMaps("CompanyName.ProjectName.Infrastructure");
                    cfg.AddMaps("CompanyName.ProjectName.Repository");
                    cfg.AddMaps("CompanyName.ProjectName.WebApi");
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
