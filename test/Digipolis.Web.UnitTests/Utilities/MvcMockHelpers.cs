using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
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
    }
}
