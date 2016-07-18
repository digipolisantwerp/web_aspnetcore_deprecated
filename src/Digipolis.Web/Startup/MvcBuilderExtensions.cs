using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Digipolis.Web.Versioning;

namespace Digipolis.Web
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddVersionEndpoint(this IMvcBuilder builder, Action<WebVersioningOptions> setupAction = null)
        {
            if ( setupAction != null )
            {
                builder.Services.Configure(setupAction);
            }

            builder.Services.TryAddSingleton<IVersionProvider, WebVersionProvider>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, WebVersioningOptionsSetup>());

            return builder;
        }
    }
}
