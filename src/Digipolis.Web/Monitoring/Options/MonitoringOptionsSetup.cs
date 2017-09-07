using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Monitoring

{
    public class MonitoringOptionsSetup : IConfigureOptions<MvcOptions>
    {
        public MonitoringOptionsSetup(IServiceProvider serviceProvider)
        {
            OptionsServices = serviceProvider;
        }

        public IServiceProvider OptionsServices { get; private set; }

        public void Configure(MvcOptions options)
        {
            var versionOptions = OptionsServices.GetService<IOptions<MonitoringOptions>>();
            options.Conventions.Add(new MonitoringConvention(versionOptions));
        }
    }
}
