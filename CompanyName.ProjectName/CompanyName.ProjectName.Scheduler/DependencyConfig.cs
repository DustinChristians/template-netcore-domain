using System.Reflection;
using CompanyName.ProjectName.Core.Abstractions.Tasks.Logging;
using CompanyName.ProjectName.Core.Types;
using CompanyName.ProjectName.Core.Utilities;
using CompanyName.ProjectName.Scheduler.Tasks.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName.Scheduler
{
    public class DependencyConfig
    {
        public static void Register(IServiceCollection services)
        {
            AddDependenciesAutomatically(services);
        }

        // Register all tasks
        private static void AddDependenciesAutomatically(IServiceCollection services)
        {
            DependencyUtility.RegisterInterfaces("Task", services, Assembly.GetAssembly(typeof(IDatabaseEventLogCleanupTask)), Assembly.GetAssembly(typeof(DatabaseEventLogCleanupTask)), DependencyTypes.Transient);
        }
    }
}
