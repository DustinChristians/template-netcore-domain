using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilter> logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// A controller exception handler for more specific API related error handling.
        /// </summary>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                this.logger.LogError(filterContext.Exception, filterContext.Exception.Message);
                if (filterContext.Exception.InnerException != null)
                {
                    this.logger.LogError(filterContext.Exception.InnerException, filterContext.Exception.InnerException.Message);
                }
                else
                {
                    this.logger.LogError(filterContext.Exception, filterContext.Exception.Message);
                }

                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                filterContext.Result = new BadRequestObjectResult(new { message = "An error occurred. Please try again", currentDate = DateTime.Now });
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
