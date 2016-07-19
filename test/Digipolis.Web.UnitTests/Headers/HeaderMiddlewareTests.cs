using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digipolis.Web.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Digipolis.Web.UnitTests.Headers
{
    public class HeaderMiddlewareTests
    {
        [Fact]
        void NextDelegateNullRaisesArgumentNullException()
        {
            var logger = Mock.Of<ILogger<HeadersMiddleware>>();
            var ex = Assert.Throws<ArgumentNullException>(() => new HeadersMiddleware(null, logger));
            Assert.Equal("next", ex.ParamName);
        }

        [Fact]
        void LoggerNullRaisesArgumentNullException()
        {
            var nextdelegate = new RequestDelegate(CreateDefaultHttpContex);
            var ex = Assert.Throws<ArgumentNullException>(() => new HeadersMiddleware(nextdelegate, null));
            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        void HandlerIsCalledForCorrespondingHeaderKey()
        {
            var handler = new Mock<IHeaderHandler>(MockBehavior.Strict);
            var headerHandlers = new List<KeyValuePair<string, IHeaderHandler>>() { new KeyValuePair<string, IHeaderHandler>("aKey", handler.Object) };
            var options = new HeaderOptions(headerHandlers);

            var headerValues = new StringValues();
            
            handler.Setup(x => x.Handle(headerValues)).Verifiable();

            var nextdelegate = new RequestDelegate(CreateDefaultHttpContex);
            var logger = Mock.Of<ILogger<HeadersMiddleware>>();
            var middleware = new HeadersMiddleware(nextdelegate, logger);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("aKey", headerValues);
            middleware.Invoke(httpContext, Options.Create(options));

            handler.Verify();
        }

        [Fact]
        void HandlerIsNotCalledWhenNoCorrespondingHeaderKey()
        {
            var handler = new Mock<IHeaderHandler>(MockBehavior.Strict);
            var headerHandlers = new List<KeyValuePair<string, IHeaderHandler>>() { new KeyValuePair<string, IHeaderHandler>("aKey", handler.Object) };
            var options = new HeaderOptions(headerHandlers);

            var headerValues = new StringValues();

            var nextdelegate = new RequestDelegate(CreateDefaultHttpContex);
            var logger = Mock.Of<ILogger<HeadersMiddleware>>();
            var middleware = new HeadersMiddleware(nextdelegate, logger);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("anotherKey", headerValues);
            middleware.Invoke(httpContext, Options.Create(options));

            handler.Verify();
        }

        private Task CreateDefaultHttpContex(HttpContext httpContext)
        {
            return Task.Run(() => { return new DefaultHttpContext(); });
        }
    }
}
