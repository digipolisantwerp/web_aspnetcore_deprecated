using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Digipolis.Web.Api;
using Digipolis.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Digipolis.Web
{
    public static class SwaggerExtensionsBuilder
    {
        /// <summary>
        /// Configure Swagger completly to your need by inheriting from <see cref="SwaggerSettings{TResponseGuidelines}"/>
        /// </summary>
        /// <typeparam name="TSwaggerSettings"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwaggerGen<TSwaggerSettings>(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null) 
            where TSwaggerSettings : SwaggerSettings<SwaggerResponseDefaults>, new()
        {
            return services.ConfigureSwaggerGen<TSwaggerSettings, SwaggerResponseDefaults>(setupAction);
        }

        /// <summary>
        /// Add and configure Swagger completly to your need by inheriting from <see cref="SwaggerSettings{TResponseGuidelines}"/>
        /// </summary>
        /// <typeparam name="TSwaggerSettings"></typeparam>
        /// <typeparam name="TSwaggerResponseDefinitions"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static IServiceCollection ConfigureSwaggerGen<TSwaggerSettings, TSwaggerResponseDefinitions>(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null)
            where TSwaggerSettings : SwaggerSettings<TSwaggerResponseDefinitions>, new()
            where TSwaggerResponseDefinitions : SwaggerResponseDefinitions
        {
            var settings = new TSwaggerSettings();

            services.Configure<SwaggerGenOptions>(settings.Configure);
            if (setupAction != null) services.ConfigureSwaggerGen(setupAction);
            return services;
        }

        /// <summary>
        /// Add and configure Swagger completly to your need by inheriting from <see cref="SwaggerSettings{TResponseGuidelines}"/>
        /// </summary>
        /// <typeparam name="TSwaggerSettings"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void AddSwaggerGen<TSwaggerSettings>(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null) 
            where TSwaggerSettings : SwaggerSettings<SwaggerResponseDefaults>, new()
        {

            services.AddSwaggerGen<TSwaggerSettings, SwaggerResponseDefaults>(setupAction);
        }

        /// <summary>
        /// Add and configure Swagger completly to your need by inheriting from <see cref="SwaggerSettings{TResponseGuidelines}"/>
        /// </summary>
        /// <typeparam name="TSwaggerSettings"></typeparam>
        /// <typeparam name="TSwaggerResponseDefinitions"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void AddSwaggerGen<TSwaggerSettings, TSwaggerResponseDefinitions>(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null)
            where TSwaggerSettings : SwaggerSettings<TSwaggerResponseDefinitions>, new()
            where TSwaggerResponseDefinitions : SwaggerResponseDefinitions
        {
            services.AddSwaggerGen(a => { });
            services.ConfigureSwaggerGen<TSwaggerSettings, TSwaggerResponseDefinitions>(setupAction);
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
