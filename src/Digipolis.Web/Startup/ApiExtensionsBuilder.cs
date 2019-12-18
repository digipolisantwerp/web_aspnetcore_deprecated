using Digipolis.Errors;
using Digipolis.Web.Api;
using Digipolis.Web.Api.Models;
using Digipolis.Web.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Digipolis.Web
{
    public static class ApiExtensionsBuilder
    {
        public static IServiceCollection AddGlobalErrorHandling<TExceptionMapper>(this IServiceCollection services) where TExceptionMapper : ExceptionMapper
        {
            services.TryAddSingleton<IExceptionMapper, TExceptionMapper>();
            services.TryAddSingleton<IExceptionHandler, ExceptionHandler>();

            return services;
        }

        public static void UseApiExtensions(this IApplicationBuilder app)
        {
            var settings = app.ApplicationServices.GetService<IOptions<ApiExtensionOptions>>();

            var httpContextAccessor = app.ApplicationServices.GetService<IActionContextAccessor>();

            if (settings?.Value?.DisableGlobalErrorHandling == false) app.UseMiddleware<ExceptionResponseMiddleware>();

            PageOptionsExtensions.Configure(httpContextAccessor);


            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                RequireHeaderSymmetry = true,
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedHost | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });
        }
    }
}
