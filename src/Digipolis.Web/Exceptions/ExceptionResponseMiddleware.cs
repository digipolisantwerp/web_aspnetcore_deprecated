using System;
using System.Net;
using System.Threading.Tasks;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Exceptions
{
    internal class ExceptionResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionResponseMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next), $"{nameof(next)} cannot be null.");
        }

        public async Task Invoke(HttpContext context)
        {
            var handler = context.RequestServices.GetService<IExceptionHandler>();
            var options = context.RequestServices.GetService<IOptions<ApiExtensionOptions>>();

            if (handler == null || options?.Value?.DisableGlobalErrorHandling == true)
            {
                await _next.Invoke(context);
                return;
            }

            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await handler.HandleAsync(context, new UnauthorizedAccessException());
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await handler.HandleAsync(context, new NotFoundException());
                }
            }
            catch (Exception ex)
            {
                await handler.HandleAsync(context, ex);
            }
        }
    }
}
