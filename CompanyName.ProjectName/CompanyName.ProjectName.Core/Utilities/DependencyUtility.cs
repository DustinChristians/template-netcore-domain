using System;
using System.Linq;
using System.Reflection;
using CompanyName.ProjectName.Core.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName.Core.Utilities
{
    public static class DependencyUtility
    {
        public static void RegisterInterfaces(string interfaceType, IServiceCollection services, Assembly coreAssembly, Assembly serviceAssembly, DependencyTypes type = DependencyTypes.Scoped)
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
