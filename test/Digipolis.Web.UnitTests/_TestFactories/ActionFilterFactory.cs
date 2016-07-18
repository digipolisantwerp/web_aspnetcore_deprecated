using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Digipolis.Web.UnitTests
{
    public static class ActionFilterFactory
    {
        public static ActionExecutingContext CreateActionExecutingContext()
        {
            var mockFilter = new Mock<ActionFilterAttribute>();

            return new ActionExecutingContext(
                CreateActionContext(),
                new IFilterMetadata[] { mockFilter.As<IFilterMetadata>().Object, },
                new Dictionary<string, object>(),
                controller: new object());
        }

        public static ActionContext CreateActionContext()
        {
            return new ActionContext(Mock.Of<HttpContext>(), new RouteData(), new ActionDescriptor());
        }
    }
}
