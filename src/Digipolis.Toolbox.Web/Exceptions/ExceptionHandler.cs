using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Digipolis.Toolbox.Errors;
using Digipolis.Toolbox.Errors.Exceptions;

namespace Digipolis.Toolbox.Web.Exceptions
{
    public class ExceptionHandler
    {
        private HttpStatusCodeMappings _mappings;
        private ILogger _logger;

        public ExceptionHandler(HttpStatusCodeMappings mappings, ILogger<ExceptionHandler> logger)
        {
            if (mappings == null) throw new ArgumentNullException(nameof(mappings), $"{nameof(mappings)} cannot be null");
            if (logger == null) throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null");

            _mappings = mappings;
            _logger = logger;
        }

        public async Task HandleAsync(HttpContext context)
        {
            try
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandlerFeature == null)
                    return;

                var exception = exceptionHandlerFeature.Error;
                var exceptionType = exception.GetType();

                if (_mappings.ContainsKey(exceptionType))
                    context.Response.StatusCode = _mappings.GetStatusCode(exceptionType);

                Error error = null;

                if (exception is BaseException)
                {
                    var baseException = exception as BaseException;
                    error = baseException.Error;
                }
                else
                {
                    error = new Error(Guid.NewGuid().ToString());
                    error.AddErrorMessage(new ErrorMessage("", $"Exception of type {exception.GetType()} occurred. Check logs for more info."));
                }

                context.Response.ContentType = "application/json";

                var responseBody = JsonConvert.SerializeObject(error);
                await context.Response.WriteAsync(responseBody);

                LogException(context.Response.StatusCode, exception);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred in the exception handler.", ex);
            }
        }
        
        private void LogException(int httpStatusCode, Exception exception)
        {
            var logMessage = new ExceptionLogMessage
            {
                HttpStatusCode = httpStatusCode,
                Exception = exception
            };

            if (exception is BaseException)
            {
                var baseException = exception as BaseException;
                logMessage.Error = baseException.Error;
            }

            var logAsJson = JsonConvert.SerializeObject(logMessage);

            if (httpStatusCode >= 500 && httpStatusCode <= 599)
                _logger.LogError(logAsJson);

            if (httpStatusCode >= 400 && httpStatusCode <= 499)
                _logger.LogDebug(logAsJson);
        }
    }
}
