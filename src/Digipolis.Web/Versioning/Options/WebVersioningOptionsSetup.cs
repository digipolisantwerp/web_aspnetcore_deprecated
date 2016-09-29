using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Versioning
{
    public class WebVersioningOptionsSetup : IConfigureOptions<MvcOptions>
    {
        public WebVersioningOptionsSetup(IServiceProvider serviceProvider)
        {
            OptionsServices = serviceProvider;
        }

        public IServiceProvider OptionsServices { get; private set; }

        public void Configure(MvcOptions options)
        {
            var versionOptions = OptionsServices.GetService<IOptions<WebVersioningOptions>>();
            options.Conventions.Add(new WebVersioningConvention(versionOptions));
        }
    }
}
