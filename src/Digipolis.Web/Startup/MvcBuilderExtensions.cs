using System;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Digipolis.Web.Api.Conventions;
using Digipolis.Web.Api.JsonConverters;
using Digipolis.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Linq;
using Digipolis.Web.Api.Tools;
using Microsoft.AspNetCore.Mvc.Routing;
using Digipolis.Web.Api.Models;
using Digipolis.Web.Modelbinders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Digipolis.Web
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddApiExtensions(this IMvcBuilder builder, IConfigurationSection config = null, Action<ApiExtensionOptions> build = null)
        {
            var apiOptions = new ApiExtensionOptions();

            #region Include services needed for building uri's in the paging object

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            builder.Services.AddScoped<ILinkProvider, LinkProvider>();
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

            if (!apiOptions.DisableVersioning)
            {
                builder.AddMvcOptions(options =>
                {
                    options.Conventions.Insert(0, new RouteConvention(new RouteAttribute("{apiVersion}")));
                });

                builder.Services.ConfigureSwaggerGen(options =>
                {
                    options.DocInclusionPredicate((version, apiDescription) =>
                    {
                        var allowedVersions = apiDescription.ActionAttributes().OfType<VersionsAttribute>().FirstOrDefault();

                        return (apiDescription.RelativePath.StartsWith("{apiVersion}/Status/") || (allowedVersions != null && allowedVersions.AcceptedVersions.Contains(version)));
                    });
                });
            }


            #endregion

            builder.AddMvcOptions(options =>
            {
                options.Filters.Insert(0, new ConsumesAttribute("application/json"));
                options.Filters.Insert(1, new ProducesAttribute("application/json"));

                options.ModelBinderProviders.Insert(0, new CommaDelimitedArrayModelBinderProvider());

                JsonOutputFormatter jsonFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();

                jsonFormatter?.SupportedMediaTypes.Add("application/hal+json");
            });

            builder.AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new BaseContractResolver();
                x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                x.SerializerSettings.Converters.Add(new TimeSpanConverter());
                x.SerializerSettings.Converters.Add(new PagedResultConverter());
                x.SerializerSettings.Converters.Add(new GuidConverter());
                x.SerializerSettings.Formatting = Formatting.None;
            });

            return builder;
        }
    }
}
