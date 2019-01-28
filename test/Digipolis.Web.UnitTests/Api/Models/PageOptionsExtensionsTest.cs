using Digipolis.Web.Api;
using Digipolis.Web.Api.Models;
using Digipolis.Web.Api.Tools;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Models
{
    public class PageOptionsExtensionsTest
    {
        [Fact]
        public void ToPagedResultConfiguredReturnsAbsolutePath()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions();
            var result = pageOptions.ToPagedResult(new List<Object>(), 0, "test", new object[] { });

            Assert.Equal("xhs://myhost.be:999/test", result.Links.First.Href);
        }

        [Fact]
        public void ToPagedResultFirstPageHasNoPreviousLink()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions() { Page = 1, PageSize = 10 };
            var result = pageOptions.ToPagedResult(new List<Object>(), 30, "test", new object[] { });

            Assert.Null(result.Links.Previous);
        }

        [Fact]
        public void ToPagedResultFirstPageHasFirstAndNextAndLastLink()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions() { Page = 1, PageSize = 10 };
            var result = pageOptions.ToPagedResult(new List<Object>(), 40, "test", new object[] { });

            Assert.NotNull(result.Links.First);
            Assert.NotNull(result.Links.Next);
            Assert.NotNull(result.Links.Last);
        }

        [Fact]
        public void ToPagedResultMiddlePageHasFirstAndNextAndPreviousAndLastLink()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions() { Page = 2, PageSize = 10 };
            var result = pageOptions.ToPagedResult(new List<Object>(), 40, "test", new object[] { });

            Assert.NotNull(result.Links.First);
            Assert.NotNull(result.Links.Previous);
            Assert.NotNull(result.Links.Next);
            Assert.NotNull(result.Links.Last);
        }


        [Fact]
        public void ToPagedResultSecondToLastPageHasFirstAndNextAndPreviousAndLastLink()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions() { Page = 3, PageSize = 10 };
            var result = pageOptions.ToPagedResult(new List<Object>(), 40, "test", new object[] { });

            Assert.NotNull(result.Links.First);
            Assert.NotNull(result.Links.Previous);
            Assert.NotNull(result.Links.Next);
            Assert.NotNull(result.Links.Last);
        }

        [Fact]
        public void ToPagedResultLastPageHasFirstAndPreviousAndLastLink()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions() { Page = 4, PageSize = 10 };
            var result = pageOptions.ToPagedResult(new List<Object>(), 40, "test", new object[] { });

            Assert.NotNull(result.Links.First);
            Assert.NotNull(result.Links.Previous);
            Assert.NotNull(result.Links.Last);
        }

        [Fact]
        public void ToPagedResultLastPageHasHasNextLink()
        {
            var accessor = GetActionContextAccessorWithLinkProvider();

            PageOptionsExtensions.Configure(accessor.Object);

            var pageOptions = new PageOptions() { Page = 4, PageSize = 10 };
            var result = pageOptions.ToPagedResult(new List<Object>(), 40, "test", new object[] { });

            Assert.Null(result.Links.Next);
        }

        private Mock<IActionContextAccessor> GetActionContextAccessorWithLinkProvider()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            Mock<IActionContextAccessor> accessor = new Mock<IActionContextAccessor>();

            var actionContext = MockHelpers.ActionContext();

            var httpContextMock = Mock.Get(actionContext.HttpContext);

            httpContextMock.SetupGet((h) => h.RequestServices).Returns(serviceProvider.Object);

            actionContext.HttpContext.RequestServices = serviceProvider.Object;

            actionContext.HttpContext.Request.Host = new Microsoft.AspNetCore.Http.HostString("myhost.be", 999);
            actionContext.HttpContext.Request.Scheme = "XHS";
            actionContext.HttpContext.Request.Query = new QueryCollection();
            actionContext.RouteData = new Microsoft.AspNetCore.Routing.RouteData();

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
