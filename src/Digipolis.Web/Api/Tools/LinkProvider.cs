using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Digipolis.Web.Api.Tools
{
    public class LinkProvider : ILinkProvider
    {
        private IActionContextAccessor _httpContextAccessor;
        //private Uri _baseUri;
        private IUrlHelper _urlHelper;


        public LinkProvider(IActionContextAccessor httpContextAccessor, IUrlHelper urlHelper, IOptions<ApiExtensionOptions> options)
        {
            if (httpContextAccessor == null) throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContextAccessor = httpContextAccessor;

            if (urlHelper == null) throw new ArgumentNullException(nameof(urlHelper));
            _urlHelper = urlHelper;
        }

        public string AbsoluteAction(string actionName, string controllerName, object routeValues = null)
        {
            var relativeUrl = _urlHelper.Action(actionName, controllerName, routeValues);
            var builder = GetAbsoluteUrlBuilder();

            builder.Path = relativeUrl;

            return builder.Uri.AbsoluteUri;
        }

        public string AbsoluteRoute(string routeName, object routeValues = null)
        {
            var relativeUrl = _urlHelper.RouteUrl(routeName, routeValues);
            var builder = GetAbsoluteUrlBuilder();

            builder.Path = relativeUrl;

            return relativeUrl;
        }

        public UriBuilder GetAbsoluteUrlBuilder()
        {
            HttpRequest request = _httpContextAccessor.ActionContext.HttpContext.Request;
            UriBuilder builder = new UriBuilder(request.Scheme, request.Host.Host);

            if (request.Host.Port.HasValue)
                builder.Port = request.Host.Port.Value;

            return builder;
        }
    }
}
