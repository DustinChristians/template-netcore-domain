using CompanyName.ProjectName.Mapping;
using CompanyName.ProjectName.Repository.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            }).AddXmlDataContractSerializerFormatters(); // Adds the XML API response format, if requested. JSON is supported by default.  

            // Database
            services.AddDbContext<CompanyNameProjectNameContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("CompanyNameProjectNameContext"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(30)));

            // Register the shared dependencies in the Mapping project
            DependencyConfig.Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                        // TODO: log the fault here
                    });
                });
            }

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
