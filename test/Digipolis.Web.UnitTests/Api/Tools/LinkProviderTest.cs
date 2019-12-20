using Digipolis.Web.Api.Tools;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Tools
{
    public class LinkProviderTest
    {
        [Fact]
        public void GetAbsoluteUrlBuilder_BuildsUrlFromRequestHostAndScheme()
        {
            // arrange
            var actionContext = MockHelpers.ActionContext();

            var actionContextAccessorMock = new Mock<IActionContextAccessor>();
            actionContextAccessorMock.SetupGet(x => x.ActionContext).Returns(actionContext);

            var host = new HostString("test.be", 99);

            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Host).Returns(host);
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Scheme).Returns("xyz");

            var headerDictionary = new HeaderDictionary {new KeyValuePair<string, StringValues>("Host", new StringValues("test.be:99"))};
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Headers).Returns(headerDictionary);

            var urlHelper = new Mock<IUrlHelper>().Object;

            // act
            var linkProvider = new LinkProvider(actionContextAccessorMock.Object, urlHelper);
            var absoluteUrl = linkProvider.GetAbsoluteUrlBuilder();

            // assert
            Assert.Equal("xyz://test.be:99/", absoluteUrl.ToString());
        }

        [Fact]
        public void GetFullUrlBuilder_BuildsFullUrlWithQueryString()
        {
            // arrange
            var actionContext = MockHelpers.ActionContext();

            var actionContextAccessorMock = new Mock<IActionContextAccessor>();
            actionContextAccessorMock.SetupGet(x => x.ActionContext).Returns(actionContext);

            var host = new HostString("test.be", 99);

            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Host).Returns(host);
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Scheme).Returns("xyz");

            var headerDictionary = new HeaderDictionary();
            headerDictionary.Add(new KeyValuePair<string, StringValues>("Host", new StringValues("test.be:99")));
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Headers).Returns(headerDictionary);
            
            var urlHelper = new Mock<IUrlHelper>().Object;

            // act
            var linkProvider = new LinkProvider(actionContextAccessorMock.Object, urlHelper);
            var fullUrl = linkProvider.GetFullUrlBuilder("/v1/test?q=99&q2=test");

            // assert
            Assert.Equal("xyz://test.be:99/v1/test?q=99&q2=test", fullUrl.ToString());
        }

        [Fact]
        public void GetFullUrlBuilder_BuildsFullUrlWithoutQueryString()
        {
            // arrange
            var actionContext = MockHelpers.ActionContext();

            var actionContextAccessorMock = new Mock<IActionContextAccessor>();
            actionContextAccessorMock.SetupGet(x => x.ActionContext).Returns(actionContext);

            var host = new HostString("test.be", 99);

            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Host).Returns(host);            
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Scheme).Returns("xyz");

            var headerDictionary = new HeaderDictionary();
            headerDictionary.Add(new KeyValuePair<string, StringValues>("Host", new StringValues("test.be:99") ));
            Mock.Get(actionContext.HttpContext.Request).SetupGet(x => x.Headers).Returns(headerDictionary);
            
            var urlHelper = new Mock<IUrlHelper>().Object;

            // act
            var linkProvider = new LinkProvider(actionContextAccessorMock.Object, urlHelper);
            var fullUrl = linkProvider.GetFullUrlBuilder("/v1/test");

            // assert
            Assert.Equal("xyz://test.be:99/v1/test", fullUrl.ToString());
        }
    }
}