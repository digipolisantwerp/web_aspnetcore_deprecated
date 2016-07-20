using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Digipolis.Web.Startup
{
    public static class WebAppServiceCollectionExtensions
    {
        public static IServiceCollection AddHeaderHandlers(this IServiceCollection services, Action<HeaderOptions> setupAction)
        {

            var options = new HeaderOptions();
            setupAction.Invoke(options);

            foreach ( var handler in options.Handlers )
            {
                //services.TryAddSingleton(handler.Value)
            }


            return services;
        }
    }
}
