using System;
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
            if(acceptedVersions == null) throw new ArgumentNullException(nameof(acceptedVersions));
            if (!acceptedVersions.Any()) throw new ArgumentOutOfRangeException(nameof(acceptedVersions), "There need to be one or more versions specified");
            if (acceptedVersions.Any(string.IsNullOrWhiteSpace)) throw new ArgumentException("Parameter acceptedVersions cannot contain an empty string", nameof(acceptedVersions));

            _acceptedVersions = acceptedVersions;
            Order = -1;
        }

        public int Order { get; private set; }

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
