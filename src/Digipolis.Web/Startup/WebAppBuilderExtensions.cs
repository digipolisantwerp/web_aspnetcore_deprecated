using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Digipolis.Web.Swagger;
using Digipolis.Web.Headers;
using Digipolis.Web.Exceptions;
using Microsoft.Extensions.Logging;

namespace Toolbox.WebApi
{
    public static class WebAppBuilderExtensions
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

        public static IApplicationBuilder UseHeaderHandlers(this IApplicationBuilder app)
        {
            app.UseMiddleware<HeadersMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseSwaggerUiRedirect(this IApplicationBuilder app, string url = null)
        {
            if ( url == null )
                app.UseMiddleware<SwaggerUiRedirectMiddleware>();
            else
                app.UseMiddleware<SwaggerUiRedirectMiddleware>(url);

            return app;
        }
    }
}
