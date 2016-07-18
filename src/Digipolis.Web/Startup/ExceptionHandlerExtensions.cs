using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Digipolis.Web.Exceptions;

namespace Digipolis.Web
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, Action<HttpStatusCodeMappings> setupAction)
        {
            var logger = app.ApplicationServices.GetService<ILogger<ExceptionHandler>>();

            var mappings = new HttpStatusCodeMappings();
            setupAction.Invoke(mappings);

            var handler = new ExceptionHandler(mappings, logger);

            return app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context => await handler.HandleAsync(context));
            });
        }
    }
}
