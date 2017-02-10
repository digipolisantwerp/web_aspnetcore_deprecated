using Digipolis.Web.Api.Tools;
using Digipolis.Web.UnitTests._TestObjects;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Tools
{
    public class LinkProviderTest
    {
        [Fact]
        public void GetAbsoluteUrlBuilder_BuildsUrlFromRequestHostAndScheme()
        {
            var actionContext = MockHelpers.ActionContext();

            var actionContextAccessorMock = new Mock<IActionContextAccessor>();
            actionContextAccessorMock.SetupGet(x => x.ActionContext).Returns(actionContext);

            var host = new HostString("test.be", 99);

            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Host).Returns(host);
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Scheme).Returns("xyz");


            var urlHelper = new Mock<IUrlHelper>().Object;

            var linkProvider = new LinkProvider(actionContextAccessorMock.Object, urlHelper, new TestApiExtensionOptions(new Web.Api.ApiExtensionOptions()));


            var absoluteUrl = linkProvider.GetAbsoluteUrlBuilder();

            Assert.Equal("xyz://test.be:99/", absoluteUrl.ToString());

        }


        [Fact]
        public void GetFullUrlBuilder_BuildsFullUrlWithQueryString()
        {
            var actionContext = MockHelpers.ActionContext();

            var actionContextAccessorMock = new Mock<IActionContextAccessor>();
            actionContextAccessorMock.SetupGet(x => x.ActionContext).Returns(actionContext);

            var host = new HostString("test.be", 99);

            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Host).Returns(host);
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Scheme).Returns("xyz");


            var urlHelper = new Mock<IUrlHelper>().Object;

            var linkProvider = new LinkProvider(actionContextAccessorMock.Object, urlHelper, new TestApiExtensionOptions(new Web.Api.ApiExtensionOptions()));
            var fullUrl = linkProvider.GetFullUrlBuilder("/v1/test?q=99");

            Assert.Equal("xyz://test.be:99/v1/test?q=99", fullUrl.ToString());

        }






    }
}
