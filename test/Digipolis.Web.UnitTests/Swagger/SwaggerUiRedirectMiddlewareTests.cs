using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Digipolis.Web.Swagger;
using Xunit;

namespace Digipolis.Web.UnitTests.Swagger
{
    public class SwaggerUiRedirectMiddlewareTests
    {
        [Fact]
        private void NextDelegateNullRaisesArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new SwaggerUiRedirectMiddleware(null));
            Assert.Equal("next", ex.ParamName);
        }

        [Fact]
        private void NextDelegateIsSet()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, null);
            Assert.Same(requestDelegate, middleware.NextDelegate);
        }

        [Fact]
        private void UrlNullSetsDefault()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, null);
            Assert.Equal(Defaults.Swagger.Url, middleware.Url);
        }

        [Fact]
        private void UrlEmptySetsDefault()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, "");
            Assert.Equal(Defaults.Swagger.Url, middleware.Url);
        }

        [Fact]
        private void UrlWhitespaceSetsDefault()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, "   ");
            Assert.Equal(Defaults.Swagger.Url, middleware.Url);
        }

        [Fact]
        private void UrlIsSet()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, "myUrl");
            Assert.Equal("myUrl", middleware.Url);
        }

        [Fact]
        private async Task RootRequestRedirectsToUrl()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, "myUrl");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/";

            await middleware.Invoke(httpContext);

            Assert.Equal(httpContext.Response.StatusCode, 302);
            Assert.Equal(1, httpContext.Response.Headers.Count);
            Assert.Equal(1, httpContext.Response.Headers["location"].Count);
            Assert.Equal("myUrl", httpContext.Response.Headers["location"].ToArray().First());
        }

        [Fact]
        private async Task NonRootRequestDoesNotRedirect()
        {
            var requestDelegate = new RequestDelegate(CreateDefaultHttpContext);
            var middleware = new SwaggerUiRedirectMiddleware(requestDelegate, "myUrl");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/anotherUrl";

            await middleware.Invoke(httpContext);

            Assert.Equal(httpContext.Response.StatusCode, 200);
            Assert.Equal(0, httpContext.Response.Headers.Count);
        }

        private Task CreateDefaultHttpContext(HttpContext HttpContext)
        {
            return Task.Run(() => { var dummy = 1; return dummy; });
        }
    }
}
