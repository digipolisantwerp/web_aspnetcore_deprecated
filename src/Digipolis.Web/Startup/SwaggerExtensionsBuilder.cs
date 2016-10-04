using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Application;
using System.Linq;
using Digipolis.Web.Api;
using Digipolis.Web.Swagger;
using Microsoft.AspNetCore.Builder;

namespace Digipolis.Web
{
    public static class SwaggerExtensionsBuilder
    {
        /// <summary>
        /// Configure Swagger completly to your need by inheriting from <see cref="SwaggerSettings{TResponseGuidelines}"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwaggerGen<TSwaggerSettings>(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null) where TSwaggerSettings : SwaggerSettings<SwaggerResponseDefinitions>, new()
        {
            var settings = new TSwaggerSettings();
            services.Configure<SwaggerGenOptions>(settings.Configure);
            if (setupAction != null) services.ConfigureSwaggerGen(setupAction);
            return services;
        }

        public static void MultipleApiVersions<TInfo>(this SwaggerGenOptions options, IEnumerable<TInfo> apiVersions)
            where TInfo : Info
        {
            options.MultipleApiVersions(apiVersions, (api, version) =>
            {
                var versionAttribute = api.ActionDescriptor.ActionConstraints.OfType<VersionsAttribute>().FirstOrDefault();
                return versionAttribute == null || versionAttribute.AcceptedVersions.Contains(version);
            });
        }

        public static IApplicationBuilder UseSwaggerUiRedirect(this IApplicationBuilder app, string url = null)
        {
            if (url == null)
                app.UseMiddleware<SwaggerUiRedirectMiddleware>();
            else
                app.UseMiddleware<SwaggerUiRedirectMiddleware>(url);

            return app;
        }
    }
}
