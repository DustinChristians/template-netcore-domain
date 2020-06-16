using System;
using CompanyName.ProjectName.Mapping;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.WebApi.Filters;
using CompanyName.ProjectName.WebApi.Scheduler;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace CompanyName.ProjectName.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services for controllers to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
            // This method will not register services used for views or pages.
            services.AddControllers(setupAction =>
            {
                // Determines if a 406 response code (an unsupprted request response type) is returned 
                // by the API when requested by the consumer.
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(setupAction =>
            {
                // For converting JSON values to Microsoft.AspNetCore.JsonPatch.JsonPatchDocument
                setupAction.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters() // Adds the XML API response format, if requested. JSON is supported by default.  
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "modelvalidationproblem",
                        Title = "One or more model validation errors occurred.",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "See the errors property for details.",
                        Instance = context.HttpContext.Request.Path
                    };

                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            // Database
            services.AddDbContext<CompanyNameProjectNameContext>(options =>
                options
                .UseSqlServer(
                    this.Configuration.GetConnectionString("CompanyNameProjectNameContext"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(30))
                .EnableSensitiveDataLogging());

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
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

            // For catching, logging and returning appropriate controller related errors
            services.AddScoped<ApiExceptionFilter>();

            // Give the Serilog CorrelationId Enricher access to the requests HttpContext so that it 
            // can attach the correlation ID to the request/response.
            services.AddHttpContextAccessor();

            // Register the shared dependencies in the Mapping project
            DependencyConfig.Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Allow cross origin resource sharing for testing all development requests.
                // This should not be done for a production build.
                app.UseCors(builder =>
                {
                    builder.WithOrigins("*");
                });
            }
            else
            {
                // In a non-development environment, this adds middleware to the pipeline that 
                // will catch exceptions, log them, and re-execute the request in an 
                // alternate pipeline. The request will not be re-executed if the response has 
                // already started.
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("There was an unexpected error.");
                    });
                });
            }

            app.UseHangfireDashboard();
            // backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            TaskScheduler.ScheduleRecurringTasks();

            // Add Serilog to the Logging Pipeline
            loggerFactory.AddSerilog();

            // Adds middleware for redirecting HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Marks the position in the middleware pipeline where a routing
            // decision is made (where an endpoint is selected)
            app.UseRouting();

            // Adds the Microsoft.AspNetCore.Authorization.AuthorizationMiddleware to the specified
            // Microsoft.AspNetCore.Builder.IApplicationBuilder, which enables authorization
            // capabilities.
            app.UseAuthorization();

            // Marks the position in the middleware pipeline where the selected
            // endpoint is executed
            app.UseEndpoints(endpoints =>
            {
                // Adds endpoints for our controller actions but no routes are specified
                // For a Web API, use attributes at controller and action level: [Route], [HttpGet], ...
                endpoints.MapControllers();
            });
        }
    }
}
