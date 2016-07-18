using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Versioning
{
    public class WebVersioningConvention : IControllerModelConvention
    {
        public WebVersioningConvention(IOptions<WebVersioningOptions> options)
        {
            Options = options.Value;
        }

        public WebVersioningOptions Options { get; private set; }

        public void Apply(ControllerModel controller)
        {
            if ( controller.ControllerType.FullName == "Digipolis.Web.Versioning.VersionController" && !String.IsNullOrWhiteSpace(Options.Route) )
            {
                controller.RouteValues.Clear();
                controller.RouteValues.Add("WebVersioningRoute", Options.Route);
            }
        }
    }
}
