using System;
using CompanyName.ProjectName.Mapping;
using CompanyName.ProjectName.Scheduler.Abstractions;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("CompanyName.ProjectName.Repository"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddTransient<ITaskScheduler, TaskScheduler>();

            // Register the shared dependencies in the Mapping project
            Mapping.DependencyConfig.Register(services, Configuration, System.Reflection.Assembly.GetEntryAssembly().GetName().Name);

            // Register the task dependencies for the scheduler
            DependencyConfig.Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env, ITaskScheduler taskScheduler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add logging to the request pipeline
            LoggerConfig.Configure(loggerFactory);

            app.UseHangfireDashboard();

            taskScheduler.ScheduleRecurringTasks();

            app.UseRouting();
        }
    }
}
