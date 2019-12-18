using System;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Digipolis.Web.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionMapper _mapper;
        private readonly ILogger<ExceptionHandler> _logger;
        private readonly IOptions<ApiExtensionOptions> _apiExtensionOptions;

        public ExceptionHandler(IExceptionMapper mapper, ILogger<ExceptionHandler> logger, IOptions<ApiExtensionOptions> apiExtensionOptions)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiExtensionOptions = apiExtensionOptions;
        }

        public async Task HandleAsync(HttpContext context, Exception ex)
        {
            if (_apiExtensionOptions?.Value?.DisableGlobalErrorHandling == true) return;

            var error = _mapper?.Resolve(ex);
            if (error == null) return;
            if (!string.IsNullOrWhiteSpace(error.Title) || !string.IsNullOrWhiteSpace(error.Code) || error.Type != null || error.ExtraParameters?.Any() == true)
            {
                context.Response.Clear();
                context.Response.ContentType = "application/problem+json";
                if (error.Status != default) context.Response.StatusCode = error.Status;
                var json = JsonConvert.SerializeObject(error);
                await context.Response.WriteAsync(json);
            }
            else if (error.Status != default) context.Response.StatusCode = error.Status;

            LogException(error, ex);
        }

        public void Handle(HttpContext context, Exception ex)
        {
            if (_apiExtensionOptions?.Value?.DisableGlobalErrorHandling == true) return;

            var error = _mapper?.Resolve(ex);
            if (error == null) return;
            if (!string.IsNullOrWhiteSpace(error.Title) || !string.IsNullOrWhiteSpace(error.Code) || error.Type != null || error.ExtraParameters?.Any() == true)
            {
                context.Response.Clear();
                context.Response.ContentType = "application/problem+json";
                if (error.Status != default) context.Response.StatusCode = error.Status;
                var json = JsonConvert.SerializeObject(error);
                context.Response.WriteAsync(json).Wait();
            }
            else if (error.Status != default) context.Response.StatusCode = error.Status;

            LogException(error, ex);
        }

        private void LogException(Error error, Exception exception)
        {
            var logMessage = new ExceptionLogMessage
            {
                Error = error,
                ExceptionInfo = exception.ToString()
            };

            if ((_apiExtensionOptions.Value?.LogExceptionObject).GetValueOrDefault())
            {
                logMessage.Exception = exception;
            }

            var logAsJson = JsonConvert.SerializeObject(logMessage);
            if (error.Status >= 500 && error.Status <= 599)
                _logger?.LogError(logAsJson);
            else if (error.Status >= 400 && error.Status <= 499)
                _logger?.LogDebug(logAsJson);
            else _logger?.LogInformation(logAsJson);
        }
    }
}