using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Validation;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

//using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ILogger = Castle.Core.Logging.ILogger;
using ILoggerFactory = Castle.Core.Logging.ILoggerFactory;


using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System;


namespace inzibackend.Web.Surpath
{
    public static class WatchDogLogger
    {
        public static IApplicationBuilder UseSurpathExceptionLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SurpathWatchdogExceptionMiddleware>();
        }

        public class SurpathWatchdogExceptionMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger _logger;

            public SurpathWatchdogExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
            {
                _next = next;
                _logger = loggerFactory.Create("SurpathWatchdog");

            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    var clientIpAddress = context.Connection.RemoteIpAddress.ToString();
                    var responseStatusCode = context.Response.StatusCode;
                    var exceptionType = ex.GetType().Name;
                    _logger.Error($"Exception Middleware: {clientIpAddress} {ex.Message}");
                    _logger.Error($"Watchdog Exception: statuscode={responseStatusCode} clientip={clientIpAddress} type={exceptionType} message={ex.Message}");

                    throw;
                }
            }
        }
    }

    public class SurpathWatchdogEventBus : IEventHandler<AbpHandledExceptionData>, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ILogger _logger { get; set; }

        public SurpathWatchdogEventBus(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.Create("SurpathWatchdog");
        }

        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            //TODO: Check eventData.Exception!
            var httpContext = _httpContextAccessor.HttpContext;

            // Get the client's IP address
            var clientIpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            // Get the response status code
            var responseStatusCode = httpContext?.Response?.StatusCode;
            var exceptionType = eventData.Exception.GetType().Name;

            // _logger.Error($"Exception EventBus: {responseStatusCode} {clientIpAddress} {eventData.Exception.Message}");
            _logger.Error($"Watchdog Exception: statuscode={responseStatusCode} clientip={clientIpAddress} type={exceptionType} message={eventData.Exception.Message}");

        }
    }
}


public class SurpathWatchdogValidation : IExceptionFilter
{
    public ILogger _logger { get; set; }


    public SurpathWatchdogValidation(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.Create("SurpathWatchdog");

    }

    public void OnException(ExceptionContext context)
    {
        var httpContext = context.HttpContext;

        // Get the client's IP address
        var clientIpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

        var exceptionType = "unknown";
        var responseStatusCode = httpContext?.Response?.StatusCode;

        if (context.Exception is AbpValidationException validationException)
        {
            exceptionType = "AbpValidationException";
        }
        _logger.Error($"Watchdog Exception: statuscode={responseStatusCode} clientip={clientIpAddress} type={exceptionType} message={context.Exception.Message}");

    }

}
