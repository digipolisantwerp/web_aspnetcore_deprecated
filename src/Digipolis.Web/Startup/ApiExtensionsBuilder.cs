using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Digipolis.Web.Api.Conventions;
using Digipolis.Web.Api.Filters;
using Digipolis.Web.Api.JsonConverters;
using Digipolis.Web.Api.Tools;
using Digipolis.Web.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
            if (httpContextAccessor != null) LinkProvider.Configure(httpContextAccessor);
        }
    }
}
