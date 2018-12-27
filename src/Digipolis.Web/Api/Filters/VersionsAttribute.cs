using Digipolis.Web.Api.Constraints;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Linq;

namespace Digipolis.Web.Api
{
    public class VersionsAttribute : Attribute, IActionConstraintFactory
    {
        public VersionsAttribute(params string[] acceptedVersions)
        {
            if (acceptedVersions == null) throw new ArgumentNullException(nameof(acceptedVersions));
            if (!acceptedVersions.Any()) throw new ArgumentOutOfRangeException(nameof(acceptedVersions), "There need to be one or more versions specified");
            if (acceptedVersions.Any(string.IsNullOrWhiteSpace)) throw new ArgumentException("Parameter acceptedVersions cannot contain an empty string", nameof(acceptedVersions));

            AcceptedVersions = acceptedVersions;
        }

        public string[] AcceptedVersions { get; }

        public bool IsReusable => true;

        public IActionConstraint CreateInstance(IServiceProvider services)
        {
            return new VersionConstraint(AcceptedVersions);
        }
    }
}
