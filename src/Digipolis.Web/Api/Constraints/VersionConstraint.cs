using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Api.Constraints
{
    public class VersionConstraint : IActionConstraint
    {
        private readonly string[] _acceptedVersions;
        public VersionConstraint(string[] acceptedVersions)
        {
            Order = -1;
            _acceptedVersions = acceptedVersions;
        }

        public int Order { get; set; }

        public bool Accept(ActionConstraintContext context)
        {
            var options = (IOptions<ApiExtensionOptions>)context.RouteContext.HttpContext.RequestServices.GetService(typeof(IOptions<ApiExtensionOptions>));
            if (options?.Value?.DisableVersioning == true) return true;

            var versionValue = context.RouteContext.RouteData.Values["apiVersion"];
            if (versionValue == null) return false;

            return _acceptedVersions.Contains(versionValue.ToString());
        }
    }
}
