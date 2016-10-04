using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Api;
using Digipolis.Web.Api.Constraints;
using Digipolis.Web.UnitTests.Utilities;
using Digipolis.Web.UnitTests._TestObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Constraints
{
    public class VersionConstraintTests
    {
        [Fact]
        public void AcceptedVersionNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(()=> new VersionConstraint(null));
        }

        [Fact]
        public void AcceptedVersionEmptyThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VersionConstraint(new string[0]));
        }

        [Fact]
        public void AcceptedVersionEmptyStringThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new VersionConstraint(new string[]{""}));
        }

        [Fact]
        public void AcceptFalseWhenDisableVersioningFalseAndInValid()
        {
            var acc = MvcMockHelpers.MoqHttpContext(ctx =>
            {
                ctx.Setup(x => x.RequestServices.GetService(typeof(IOptions<ApiExtensionOptions>))).Returns<object>(x => new TestApiExtensionOptions(new ApiExtensionOptions { DisableVersioning = false }));
            }).MoqActionConstraintContext();

            acc.RouteContext.RouteData.Values["apiVersion"] = "v2";

            var versions = new VersionConstraint(new string[] { "v1" });
            Assert.False(versions.Accept(acc));
        }

        [Fact]
        public void AcceptTrueWhenDisableVersioningTrue()
        {
            var acc = MvcMockHelpers.MoqHttpContext(ctx =>
            {
                ctx.Setup(x => x.RequestServices.GetService(typeof(IOptions<ApiExtensionOptions>))).Returns<object>(x => new TestApiExtensionOptions(new ApiExtensionOptions { DisableVersioning = false }));
            }).MoqActionConstraintContext();
            var versions = new VersionConstraint(new string[] { "v1" });
            Assert.False(versions.Accept(acc));
        }

        [Fact]
        public void AcceptTrueWhenDisableVersioningFalseAndValid()
        {
            var acc = MvcMockHelpers.MoqHttpContext(ctx =>
            {
                ctx.Setup(x => x.RequestServices.GetService(typeof(IOptions<ApiExtensionOptions>))).Returns<object>(x => new TestApiExtensionOptions(new ApiExtensionOptions { DisableVersioning = false }));
            }).MoqActionConstraintContext();

            acc.RouteContext.RouteData.Values["apiVersion"] = "v1";

            var versions = new VersionConstraint(new string[] { "v1" });
            Assert.True(versions.Accept(acc));
        }
    }
}
