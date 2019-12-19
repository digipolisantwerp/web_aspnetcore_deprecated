using Digipolis.Web.Api;
using Digipolis.Web.Api.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using Digipolis.Web.Api.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
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

            builder.Services.AddScoped(sp =>
            {
                var ac = sp.GetRequiredService<IActionContextAccessor>().ActionContext;
                var f = sp.GetRequiredService<IUrlHelperFactory>();
                return f.GetUrlHelper(ac);
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
                builder.Services.Configure(build);
                build(apiOptions);
            }

            #endregion

            #region Configuration from options

            if (!apiOptions.DisableVersioning)
            {
                builder.AddMvcOptions(options => { options.Conventions.Insert(0, new RouteConvention(new RouteAttribute("{apiVersion}"))); });

                builder.Services.ConfigureSwaggerGen(options =>
                {
                    options.DocInclusionPredicate((version, apiDescription) =>
                    {
                        if (!apiDescription.TryGetMethodInfo(out var methodInfo)) return false;

                        var allowedVersions = methodInfo.GetCustomAttributes(true).OfType<VersionsAttribute>().FirstOrDefault();
                        return (allowedVersions != null && allowedVersions.AcceptedVersions.Contains(version));
                    });
                });
            }

            #endregion

            return builder;
        }
    }
}