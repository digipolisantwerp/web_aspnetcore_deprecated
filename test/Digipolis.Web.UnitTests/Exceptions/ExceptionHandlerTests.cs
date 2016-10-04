using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Exceptions;
using Digipolis.Web.UnitTests.Utilities;
using Xunit;

namespace Digipolis.Web.UnitTests.Exceptions
{
    public class ExceptionHandlerTests
    {
        private TestLogger<ExceptionHandler> _logger = TestLogger<ExceptionHandler>.CreateLogger();

        private Mock<HttpResponse> _mockHttpResponse;

        [Fact]
        private void mappingsNullRaisesArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionHandler(null, _logger));
            Assert.Equal("mappings", ex.ParamName);
        }

        [Fact]
        private void loggerNullRaisesArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionHandler(new HttpStatusCodeMappings(), null));
            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        private async Task HandlesBaseExceptionWithStatusCode4xx()
        {
            var mappings = new HttpStatusCodeMappings();
            var handler = new ExceptionHandler(mappings, _logger);

            var exception = new NotFoundException();
            var mockHttpContext = CreateMockHttpContext(exception);

            await handler.HandleAsync(mockHttpContext);

            Assert.Equal(404, _mockHttpResponse.Object.StatusCode);
            Assert.Equal("application/json", _mockHttpResponse.Object.ContentType);

            var serializedError = JsonConvert.SerializeObject(exception.Error);
            Assert.Equal(serializedError, GetResponseBodyAsString(_mockHttpResponse.Object));
            Assert.StartsWith("Debug", _logger.LoggedMessages[0]);
            Assert.Contains(serializedError, _logger.LoggedMessages[0]);
        }

        [Fact]
        private async Task HandlesBaseExceptionWithStatusCode5xx()
        {
            var mappings = new HttpStatusCodeMappings();
            mappings.Add<NotFoundException>(500);
            var handler = new ExceptionHandler(mappings, _logger);

            var exception = new NotFoundException();
            var mockHttpContext = CreateMockHttpContext(exception);

            await handler.HandleAsync(mockHttpContext);

            Assert.Equal(500, _mockHttpResponse.Object.StatusCode);
            Assert.Equal("application/json", _mockHttpResponse.Object.ContentType);

            var serializedError = JsonConvert.SerializeObject(exception.Error);
            Assert.Equal(serializedError, GetResponseBodyAsString(_mockHttpResponse.Object));
            Assert.StartsWith("Error", _logger.LoggedMessages[0]);
            Assert.Contains(serializedError, _logger.LoggedMessages[0]);
        }

        [Fact]
        private async Task HandlesNonBaseException()
        {
            var mappings = new HttpStatusCodeMappings();
            var handler = new ExceptionHandler(mappings, _logger);

            var exception = new ArgumentNullException();
            var mockHttpContext = CreateMockHttpContext(exception);

            await handler.HandleAsync(mockHttpContext);

            Assert.Equal(500, _mockHttpResponse.Object.StatusCode);

            var returnedError = GetResponseBodyAsError(_mockHttpResponse.Object);
            Assert.NotNull(returnedError);
            Assert.Equal("Exception of type System.ArgumentNullException occurred. Check logs for more info.", returnedError.Messages.First().Message);

            Assert.StartsWith("Error", _logger.LoggedMessages[0]);
        }

        [Fact]
        private async Task HandlesNonBaseExceptionWithCustomStatusCode()
        {
            var mappings = new HttpStatusCodeMappings();
            mappings.Add<NullReferenceException>(400);
            var handler = new ExceptionHandler(mappings, _logger);

            var exception = new NullReferenceException();
            var mockHttpContext = CreateMockHttpContext(exception);

            await handler.HandleAsync(mockHttpContext);

            Assert.Equal(400, _mockHttpResponse.Object.StatusCode);
            Assert.StartsWith("Debug", _logger.LoggedMessages[0]);
        }

        [Fact]
        private async Task LogsExceptionsOccurredInHandler()
        {
            var mappings = new HttpStatusCodeMappings();
            var handler = new ExceptionHandler(mappings, _logger);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(c => c.Features).Throws<Exception>();

            await handler.HandleAsync(mockHttpContext.Object);

            Assert.StartsWith("Error", _logger.LoggedMessages[0]);
        }

        private HttpContext CreateMockHttpContext(Exception exception)
        {
            _mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpResponse.SetupProperty(r => r.ContentType, "");
            _mockHttpResponse.SetupProperty(r => r.StatusCode, 500);

            var memStream = new MemoryStream();
            _mockHttpResponse.SetupProperty(r => r.Body, memStream);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(c => c.Response).Returns(_mockHttpResponse.Object);

            var exceptionHandlerFeature = new ExceptionHandlerFeature
            {
                Error = exception
            };

            var featureCollection = new FeatureCollection();
            featureCollection.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);

            mockHttpContext.SetupGet(c => c.Features).Returns(featureCollection);

            return mockHttpContext.Object;
        }

        private string GetResponseBodyAsString(HttpResponse httpResponse)
        {
            var stream = httpResponse.Body;
            stream.Position = 0;
            var reader = new StreamReader(stream);
            var bodyContent = reader.ReadToEnd();
            return bodyContent;
        }

        private Error GetResponseBodyAsError(HttpResponse httpResponse)
        {
            var stream = httpResponse.Body;
            stream.Position = 0;
            var reader = new StreamReader(stream);
            var bodyContent = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<Error>(bodyContent);
        }
    }
}
