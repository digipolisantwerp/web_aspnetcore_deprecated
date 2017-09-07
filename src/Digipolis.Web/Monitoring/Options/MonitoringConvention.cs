using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Monitoring
{
    public class MonitoringConvention : IControllerModelConvention
    {
        public MonitoringConvention(IOptions<MonitoringOptions> options)
        {
            Options = options.Value;
        }

        public MonitoringOptions Options { get; private set; }

        public void Apply(ControllerModel controller)
        {
            if ( controller.ControllerType.FullName == "Digipolis.Web.Monitoring.StatusController" && !String.IsNullOrWhiteSpace(Options.Route) )
            {
                controller.RouteValues.Clear();
                controller.RouteValues.Add("WebMonitoringRoute", Options.Route);
            }
        }
    }
}
