using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Filters
{
    public class Net5TemplateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<Net5TemplateExceptionFilterAttribute> _logger;
        private readonly IWebHostEnvironment _env;

        public Net5TemplateExceptionFilterAttribute(ILogger<Net5TemplateExceptionFilterAttribute> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            //if (context.Exception is BadRequestException) BuildResponse(context, HttpStatusCode.BadRequest, "BadRequestError");
            //else if (context.Exception is NotFoundException) BuildResponse(context, HttpStatusCode.NotFound, "NotFoundError");
            //else 
                BuildResponse(context, HttpStatusCode.InternalServerError);
        }
        private void BuildResponse(ExceptionContext context, HttpStatusCode statusCode, string errorCode = "UnhandledError")
        {
            var trace = Guid.NewGuid();
            var listErrors = new List<KeyValuePair<string, string>>();
            var ex = context.Exception.GetBaseException().Message;

            listErrors.Add(new KeyValuePair<string, string>(errorCode, ex));

            if (_env.IsDevelopment())
            {
                var valuePairStackTrace = new KeyValuePair<string, string>("StackTrace", context.Exception.StackTrace);
                listErrors.Add(valuePairStackTrace);
            }

            context.Result = new ObjectResult(new { Trace = trace, Errors = listErrors });
            context.HttpContext.Response.StatusCode = (int)statusCode;

            _logger.LogError(ex, $"Trace->{trace}");
        }
    }
}