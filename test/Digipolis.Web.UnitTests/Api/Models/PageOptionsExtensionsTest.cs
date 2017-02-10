using Digipolis.Web.Api;
using Digipolis.Web.Api.Models;
using Digipolis.Web.Api.Tools;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Models
{
    public class PageOptionsExtensionsTest
    {
        [Fact]
        public void ToPagedResultWithoutCallingConfigureThrowsException()
        {
            var pageOptions = new PageOptions();

            Assert.Throws<TypeInitializationException>(() => pageOptions.ToPagedResult(new List<Object>(), 0, "test"));
        }

        [Fact]
        public void ToPagedResultConfiguredReturnsAbsolutePath()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions();
            var result = pageOptions.ToPagedResult(new List<Object>(), 0, "test", new object[] { });

            Assert.Equal("xhs://myhost.be:999/test", result.Links.First.Href);
        }


        private Mock<IActionContextAccessor> GetActionContextAccessorWithLinkProvider()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            Mock<IActionContextAccessor> accessor = new Mock<IActionContextAccessor>();

            var actionContext = MockHelpers.ActionContext();

            var httpContextMock = Mock.Get(actionContext.HttpContext);

            httpContextMock.SetupGet((h) => h.RequestServices).Returns(serviceProvider.Object);


            actionContext.HttpContext.RequestServices = serviceProvider.Object;

            actionContext.HttpContext.Request.Host = new Microsoft.AspNetCore.Http.HostString("myhost.be",999);
            actionContext.HttpContext.Request.Scheme = "XHS";

            var apiExtOptions = new Mock<IOptions<ApiExtensionOptions>>();
            var urlHelper = new Mock<IUrlHelper>();

            urlHelper.Setup((u) => u.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("/test");

            accessor.SetupGet((o) => o.ActionContext).Returns(actionContext);

            var linkProvider = new LinkProvider(accessor.Object, urlHelper.Object, apiExtOptions.Object);

            serviceProvider.Setup((x) => x.GetService(typeof(ILinkProvider))).Returns(linkProvider);

            return accessor;
        }

    }
}
