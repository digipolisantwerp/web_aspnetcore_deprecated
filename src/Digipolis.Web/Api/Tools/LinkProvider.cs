using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Digipolis.Web.Api.Tools
{
    internal static class LinkProvider
    {
        private static IActionContextAccessor _httpContextAccessor;

        internal static void Configure(IActionContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null) throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContextAccessor = httpContextAccessor;
        }

        internal static string AbsoluteAction(string actionName, string controllerName, object routeValues = null)
        {
            //string scheme = _httpContextAccessor.ActionContext.HttpContext.Request.Scheme;
            var helper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(_httpContextAccessor.ActionContext);
            return helper.Action(actionName, controllerName, routeValues);
        }

        internal static string AbsoluteRoute(string routeName, object routeValues = null)
        {
            var helper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(_httpContextAccessor.ActionContext);
            return helper.RouteUrl(routeName, routeValues);
        }
    }
}
