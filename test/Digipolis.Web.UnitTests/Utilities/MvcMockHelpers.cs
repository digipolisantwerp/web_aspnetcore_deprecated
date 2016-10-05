using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Api;
using Digipolis.Web.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;

namespace Digipolis.Web.UnitTests.Utilities
{
    public static class MvcMockHelpers
    {
        public static HttpContext MoqHttpContext(Action<Mock<HttpContext>> httpContext = null)
        {
            Mock<HttpContext> ctx = new Mock<HttpContext>();
            httpContext?.Invoke(ctx);
            return ctx.Object;
        }

        public static ActionConstraintContext MoqActionConstraintContext(this HttpContext httpContext)
        {
            return new ActionConstraintContext()
            {
                RouteContext = new RouteContext(httpContext)
            };
        }

        public static ActionConstraintContext ActionConstraintContext(Action<Mock<HttpContext>> httpContext = null)
        {
            Mock<HttpContext> ctx = new Mock<HttpContext>();
            httpContext?.Invoke(ctx);

            return new ActionConstraintContext()
            {
                RouteContext = new RouteContext(ctx.Object)
                {
                    RouteData = new RouteData
                    {
                        Values = { }
                    }
                }
            };
        }

        public static ExceptionHandler ExceptionHandler()
        {
            //var mapper = 
            var logger = TestLogger<ExceptionHandler>.CreateLogger();
            var mvcOptions = new Mock<IOptions<MvcJsonOptions>>();
            var apiExtOptions = new Mock<IOptions<ApiExtensionOptions>>();

            //return new ExceptionHandler();


            return null;
        }
    }
}
