using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TestN1.Infrastructure
{
    public class HttpServiceExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpServiceExceptionFilter> _logger;

        public HttpServiceExceptionFilter(ILogger<HttpServiceExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);


            ValidationProblemDetails problemDetails = new ValidationProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add("DomainValidations", new[] { context.Exception.Message });

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.ExceptionHandled = true;
        }
    }
}