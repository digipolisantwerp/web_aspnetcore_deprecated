using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Digipolis.Web.Exceptions;
using Digipolis.Web.UnitTests._TestObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;

namespace Digipolis.Web.UnitTests.Utilities
{
    public static class MockHelpers
    {
        public static HttpContext HttpContext(Action<Mock<HttpContext>> httpContext = null)
        {
            Mock<HttpContext> ctx = new Mock<HttpContext>();
            
            var mockReqHeader = new Mock<IHeaderDictionary>();
            var request = new Mock<HttpRequest>();
            request.SetupAllProperties();
            request.SetupGet(x => x.Headers).Returns(mockReqHeader.Object);
            request.SetupGet(x => x.HttpContext).Returns(ctx.Object);

            var response = new Mock<HttpResponse>();
            response.SetupAllProperties();
            var mockResHeader = new Mock<IHeaderDictionary>();
            response.SetupGet(x => x.Headers).Returns(mockResHeader.Object);
            response.SetupGet(x => x.Body).Returns(new MemoryStream());
            response.SetupGet(x => x.HttpContext).Returns(ctx.Object);

            var responseFeature = new Mock<IHttpResponseFeature>();
            var features = new Mock<IFeatureCollection>();
            features.Setup(x => x.Get<IHttpResponseFeature>()).Returns(responseFeature.Object);

            ctx.SetupGet(c => c.Request).Returns(request.Object);
            ctx.SetupGet(c => c.Response).Returns(response.Object);
            ctx.SetupGet(x => x.Features).Returns(features.Object);
            httpContext?.Invoke(ctx);
            
            return ctx.Object;
        }

        public static ActionConstraintContext ActionConstraintContext(this HttpContext httpContext)
        {
            return new ActionConstraintContext()
            {
                RouteContext = new RouteContext(httpContext)
            };
        }

        public static ExceptionHandler ExceptionHandler(Action<Mock<IOptions<MvcJsonOptions>>> mvcOptions = null, Action<Mock<IOptions<ApiExtensionOptions>>> apiOptions = null)
        {
            var logger = TestLogger<ExceptionHandler>.CreateLogger();
            var mvcjsonOptions = new Mock<IOptions<MvcJsonOptions>>();
            mvcOptions?.Invoke(mvcjsonOptions);
            var apiExtOptions = new Mock<IOptions<ApiExtensionOptions>>();
            apiOptions?.Invoke(apiExtOptions);
            var mapper = new ExceptionMapperTest();
            return new ExceptionHandler(mapper, logger, mvcjsonOptions.Object, apiExtOptions.Object);
        }
    }
}
