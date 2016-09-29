using System;
using Digipolis.Web.Api.Constraints;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Digipolis.Web.Api
{
    public class VersionsAttribute : Attribute, IActionConstraintFactory
    {
        public VersionsAttribute(params string[] acceptedVersions)
        {
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
