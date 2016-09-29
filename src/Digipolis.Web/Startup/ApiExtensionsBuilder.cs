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
        public static IMvcBuilder AddApiExtensions(this IMvcBuilder builder, IConfigurationSection config = null, Action<ApiExtensionOptions> build = null, Type exception = null)
        {
            var apiOptions = new ApiExtensionOptions();

            #region Include services needed for building uri's in the paging object

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            #endregion

            #region Register Options

            if (config != null && build != null) builder.Services.Configure<ApiExtensionOptions>(x => { });
            if (config != null)
            {
                builder.Services.Configure<ApiExtensionOptions>(config);
                config.Bind(apiOptions);
            }
            if (build != null)
            {
                builder.Services.Configure<ApiExtensionOptions>(build);
                build(apiOptions);
            }

            #endregion

            #region Configuration from options

            if (apiOptions.EnableGlobalErrorHandling)
            {
                if (apiOptions.ExceptionMapper == null)
                    throw new ArgumentNullException(nameof(exception), "An exceptionhadler must be provided on AddApiExtensions when global error handling is turned on.");

                builder.Services.AddSingleton<IExceptionMapper>(apiOptions.ExceptionMapper);
                builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            }

            if (apiOptions.EnableVersioning)
            {
                builder.AddMvcOptions(options =>
                {
                    options.Conventions.Insert(0, new RouteConvention(new RouteAttribute("{apiVersion}")));
                    options.Filters.Add(typeof(GlobalExceptionFilter));
                });
            }

            #endregion

            builder.AddMvcOptions(options =>
            {
                options.Filters.Insert(0, new ConsumesAttribute("application/json"));
                options.Filters.Insert(1, new ProducesAttribute("application/json"));
            });

            builder.AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new BaseContractResolver();
                x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                x.SerializerSettings.Converters.Add(new TimeSpanConverter());
                x.SerializerSettings.Converters.Add(new PageResultConverter());
                x.SerializerSettings.Converters.Add(new GuidConverter());
                x.SerializerSettings.Formatting = Formatting.None;
            });

            return builder;
        }

        public static void UseApiExtensions(this IApplicationBuilder app)
        {
            var settings = app.ApplicationServices.GetService<IOptions<ApiExtensionOptions>>();
            var httpContextAccessor = app.ApplicationServices.GetService<IActionContextAccessor>();

            if (settings?.Value?.EnableGlobalErrorHandling == true) app.UseMiddleware<ExceptionResponseMiddleware>();
            if (httpContextAccessor != null) LinkProvider.Configure(httpContextAccessor);
        }
    }
}
