using System;
using System.Threading;
using System.Threading.Tasks;
using Digipolis.Errors;
using Digipolis.Web.Exceptions;
using Digipolis.Web.UnitTests.Utilities;
using Digipolis.Web.UnitTests._TestObjects;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Digipolis.Web.Api;
using Moq;
using System.Linq;

namespace Digipolis.Web.UnitTests.Exceptions
{
    public class ExceptionHandlerTests
    {
        [Fact]
        private void CtorMapperNullException()
        {
           Assert.Throws<ArgumentNullException>(() => new ExceptionHandler(null, null, null));
        }

        [Fact]
        private void CtorLoggerNullException()
        {
            var mapper = new ExceptionMapperTest();
            Assert.Throws<ArgumentNullException>(() => new ExceptionHandler(mapper, null, null));
        }

        [Fact]
        private void CtorSuccesWithoutOptions()
        {
            var mapper = new ExceptionMapperTest();
            var logger = new TestLogger<ExceptionHandler>();
            Assert.NotNull(new ExceptionHandler(mapper, logger, null));
        }

        [Fact]
        private void HandleErrorStatusCodeOnlyMapping()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            handler.Handle(ctx, new ArgumentNullException());
            Assert.Equal(300, ctx.Response.StatusCode);
        }

        [Fact]
        private void HandleErrorMapping()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            handler.Handle(ctx, new AggregateException());
            Assert.Equal(400, ctx.Response.StatusCode);
            Assert.Equal("application/problem+json", ctx.Response.ContentType);
        }

        [Fact]
        private void HandleDefaultError()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            handler.Handle(ctx, new Exception());
            Assert.Equal(500, ctx.Response.StatusCode);
            Assert.Equal("application/problem+json", ctx.Response.ContentType);
        }

        [Fact]
        private void HandleNoErrorMappingEqualsDefaultError()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            handler.Handle(ctx, new AbandonedMutexException());
            Assert.Equal(500, ctx.Response.StatusCode);
            Assert.Equal("application/problem+json", ctx.Response.ContentType);
        }

        [Fact]
        private async Task HandleAsyncErrorStatusCodeOnlyMapping()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            await handler.HandleAsync(ctx, new ArgumentNullException());
            Assert.Equal(300, ctx.Response.StatusCode);
        }

        [Fact]
        private async Task HandleAsyncErrorMapping()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            await handler.HandleAsync(ctx, new AggregateException());
            Assert.Equal(400, ctx.Response.StatusCode);
            Assert.Equal("application/problem+json", ctx.Response.ContentType);
        }

        [Fact]
        private async Task HandleAsyncDefaultError()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            await handler.HandleAsync(ctx, new Exception());
            Assert.Equal(500, ctx.Response.StatusCode);
            Assert.Equal("application/problem+json", ctx.Response.ContentType);
        }

        [Fact]
        private async Task HandleAsyncNoErrorMappingEqualsDefaultError()
        {
            var handler = MockHelpers.ExceptionHandler();
            var ctx = MockHelpers.HttpContext();
            await handler.HandleAsync(ctx, new AbandonedMutexException());
            Assert.Equal(500, ctx.Response.StatusCode);
            Assert.Equal("application/problem+json", ctx.Response.ContentType);
        }

        [Fact]
        private async Task LogVerboseException()
        {
            var loggedMessages = new List<LogMessage>();
            var logger = new TestLogger<ExceptionHandler>(loggedMessages);

            var handler = MockHelpers.ExceptionHandler(null, authOptions =>
            {
                authOptions.SetupGet(o => o.Value).Returns(new ApiExtensionOptions { LogExceptionObject = true });
            }, logger);

            var ctx = MockHelpers.HttpContext();
            await handler.HandleAsync(ctx, new InvalidOperationException());

            Assert.Matches(".*\"Exception\":{.+}", loggedMessages[0].Message);
            Assert.Matches(".*\"ExceptionInfo\":\".*\"", loggedMessages[0].Message);
        }

        [Fact]
        private async Task LogOnlyExceptionInfo()
        {
            var loggedMessages = new List<LogMessage>();
            var logger = new TestLogger<ExceptionHandler>(loggedMessages);

            var handler = MockHelpers.ExceptionHandler(null, authOptions =>
            {
                authOptions.SetupGet(o => o.Value).Returns(new ApiExtensionOptions { LogExceptionObject = false });
            }, logger);

            var ctx = MockHelpers.HttpContext();
            await handler.HandleAsync(ctx, new AbandonedMutexException());

            await handler.HandleAsync(ctx, new InvalidOperationException());

            Assert.DoesNotMatch(".*\"Exception\":{.+}", loggedMessages[0].Message);
            Assert.Matches(".*\"ExceptionInfo\":\".*\"", loggedMessages[0].Message);
        }
    }
}

//    public class ExceptionHandlerTests
//    {
//        private TestLogger<ExceptionHandler> _logger = TestLogger<ExceptionHandler>.CreateLogger();

//        private Mock<HttpResponse> _mockHttpResponse;

//        [Fact]
//        private void mappingsNullRaisesArgumentNullException()
//        {
//            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionHandler(null, _logger));
//            Assert.Equal("mappings", ex.ParamName);
//        }

//        [Fact]
//        private void loggerNullRaisesArgumentNullException()
//        {
//            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionHandler(new HttpStatusCodeMappings(), null));
//            Assert.Equal("logger", ex.ParamName);
//        }

//        [Fact]
//        private async Task HandlesBaseExceptionWithStatusCode4xx()
//        {
//            var mappings = new HttpStatusCodeMappings();
//            var handler = new ExceptionHandler(mappings, _logger);

//            var exception = new NotFoundException();
//            var mockHttpContext = CreateMockHttpContext(exception);

//            await handler.HandleAsync(mockHttpContext);

//            Assert.Equal(404, _mockHttpResponse.Object.StatusCode);
//            Assert.Equal("application/json", _mockHttpResponse.Object.ContentType);

//            var serializedError = JsonConvert.SerializeObject(exception.Error);
//            Assert.Equal(serializedError, GetResponseBodyAsString(_mockHttpResponse.Object));
//            Assert.StartsWith("Debug", _logger.LoggedMessages[0]);
//            Assert.Contains(serializedError, _logger.LoggedMessages[0]);
//        }

//        [Fact]
//        private async Task HandlesBaseExceptionWithStatusCode5xx()
//        {
//            var mappings = new HttpStatusCodeMappings();
//            mappings.Add<NotFoundException>(500);
//            var handler = new ExceptionHandler(mappings, _logger);

//            var exception = new NotFoundException();
//            var mockHttpContext = CreateMockHttpContext(exception);

//            await handler.HandleAsync(mockHttpContext);

//            Assert.Equal(500, _mockHttpResponse.Object.StatusCode);
//            Assert.Equal("application/json", _mockHttpResponse.Object.ContentType);

//            var serializedError = JsonConvert.SerializeObject(exception.Error);
//            Assert.Equal(serializedError, GetResponseBodyAsString(_mockHttpResponse.Object));
//            Assert.StartsWith("Error", _logger.LoggedMessages[0]);
//            Assert.Contains(serializedError, _logger.LoggedMessages[0]);
//        }

//        [Fact]
//        private async Task HandlesNonBaseException()
//        {
//            var mappings = new HttpStatusCodeMappings();
//            var handler = new ExceptionHandler(mappings, _logger);

//            var exception = new ArgumentNullException();
//            var mockHttpContext = CreateMockHttpContext(exception);

//            await handler.HandleAsync(mockHttpContext);

//            Assert.Equal(500, _mockHttpResponse.Object.StatusCode);

//            var returnedError = GetResponseBodyAsError(_mockHttpResponse.Object);
//            Assert.NotNull(returnedError);
//            Assert.Equal("Exception of type System.ArgumentNullException occurred. Check logs for more info.", returnedError.Messages.First().Message);

//            Assert.StartsWith("Error", _logger.LoggedMessages[0]);
//        }

//        [Fact]
//        private async Task HandlesNonBaseExceptionWithCustomStatusCode()
//        {
//            var mappings = new HttpStatusCodeMappings();
//            mappings.Add<NullReferenceException>(400);
//            var handler = new ExceptionHandler(mappings, _logger);

//            var exception = new NullReferenceException();
//            var mockHttpContext = CreateMockHttpContext(exception);

//            await handler.HandleAsync(mockHttpContext);

//            Assert.Equal(400, _mockHttpResponse.Object.StatusCode);
//            Assert.StartsWith("Debug", _logger.LoggedMessages[0]);
//        }

//        [Fact]
//        private async Task LogsExceptionsOccurredInHandler()
//        {
//            var mappings = new HttpStatusCodeMappings();
//            var handler = new ExceptionHandler(mappings, _logger);

//            var mockHttpContext = new Mock<HttpContext>();
//            mockHttpContext.SetupGet(c => c.Features).Throws<Exception>();

//            await handler.HandleAsync(mockHttpContext.Object);

//            Assert.StartsWith("Error", _logger.LoggedMessages[0]);
//        }

//        private HttpContext CreateMockHttpContext(Exception exception)
//        {
//            _mockHttpResponse = new Mock<HttpResponse>();
//            _mockHttpResponse.SetupProperty(r => r.ContentType, "");
//            _mockHttpResponse.SetupProperty(r => r.StatusCode, 500);

//            var memStream = new MemoryStream();
//            _mockHttpResponse.SetupProperty(r => r.Body, memStream);

//            var mockHttpContext = new Mock<HttpContext>();
//            mockHttpContext.SetupGet(c => c.Response).Returns(_mockHttpResponse.Object);

//            var exceptionHandlerFeature = new ExceptionHandlerFeature
//            {
//                Error = exception
//            };

//            var featureCollection = new FeatureCollection();
//            featureCollection.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);

//            mockHttpContext.SetupGet(c => c.Features).Returns(featureCollection);

//            return mockHttpContext.Object;
//        }

//        private string GetResponseBodyAsString(HttpResponse httpResponse)
//        {
//            var stream = httpResponse.Body;
//            stream.Position = 0;
//            var reader = new StreamReader(stream);
//            var bodyContent = reader.ReadToEnd();
//            return bodyContent;
//        }

//        private Error GetResponseBodyAsError(HttpResponse httpResponse)
//        {
//            var stream = httpResponse.Body;
//            stream.Position = 0;
//            var reader = new StreamReader(stream);
//            var bodyContent = reader.ReadToEnd();
//            return JsonConvert.DeserializeObject<Error>(bodyContent);
//        }
//    }
//}
