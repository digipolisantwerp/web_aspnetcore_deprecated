using Digipolis.Errors.Exceptions;
using Digipolis.Web.Api;
using Digipolis.Web.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Exceptions
{
    public class ExceptionResponseMiddlewareTests
    {
        [Fact]
        public async Task ShouldHandleNotFound()
        {
            var middleware = new ExceptionResponseMiddleware((httpContext) => Task.CompletedTask);
            var mockExceptionHandler = new Mock<IExceptionHandler>();
            var mockHttpContext = CreateMockHttpContext(mockExceptionHandler.Object, new ApiExtensionOptions(), HttpStatusCode.NotFound);

            await middleware.Invoke(mockHttpContext.Object);

            mockExceptionHandler.Verify(h => h.HandleAsync(mockHttpContext.Object, It.IsAny<NotFoundException>()));
        }

        [Fact]
        public async Task ShouldHandleUnauthorized()
        {
            var middleware = new ExceptionResponseMiddleware((httpContext) => Task.CompletedTask);
            var mockExceptionHandler = new Mock<IExceptionHandler>();
            var mockHttpContext = CreateMockHttpContext(mockExceptionHandler.Object, new ApiExtensionOptions(), HttpStatusCode.Unauthorized);

            await middleware.Invoke(mockHttpContext.Object);

            mockExceptionHandler.Verify(h => h.HandleAsync(mockHttpContext.Object, It.IsAny<UnauthorizedAccessException>()));
        }

        private Mock<IServiceProvider> CreateMockServiceProvider(IExceptionHandler exceptionHandler, ApiExtensionOptions options)
        {
            var mock = new Mock<IServiceProvider>();

            mock.Setup(m => m.GetService(typeof(IExceptionHandler)))
                .Returns(exceptionHandler);

            mock.Setup(m => m.GetService(typeof(IOptions<ApiExtensionOptions>)))
                .Returns(Options.Create<ApiExtensionOptions>(options));

            return mock;
        }

        private Mock<HttpContext> CreateMockHttpContext(IExceptionHandler exceptionHandler, ApiExtensionOptions options, HttpStatusCode httpStatusCode)
        {
            var mock = new Mock<HttpContext>();

            mock.SetupGet(m => m.RequestServices)
                .Returns(CreateMockServiceProvider(exceptionHandler, options).Object);

            var defaultContext = new DefaultHttpContext();
            defaultContext.Features.Get<IHttpResponseFeature>().StatusCode = (int)httpStatusCode;

            mock.SetupGet(m => m.Response)
                .Returns(defaultContext.Response);

            return mock;
        }
    }
}
