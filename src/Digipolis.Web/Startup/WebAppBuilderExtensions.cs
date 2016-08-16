using System;
using Microsoft.AspNetCore.Builder;
using Digipolis.Web.Swagger;

namespace Digipolis.Web
{
    public static class WebAppBuilderExtensions
    {
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
