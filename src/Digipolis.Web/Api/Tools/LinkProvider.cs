using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Headers;

namespace Digipolis.Web.Api.Tools
{
    public class LinkProvider : ILinkProvider
    {
        private readonly IActionContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;


        public LinkProvider(IActionContextAccessor httpContextAccessor, IUrlHelper urlHelper)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        public string AbsoluteAction(string actionName, string controllerName, object routeValues = null)
        {
            var relativeUrl = _urlHelper.Action(actionName, controllerName, routeValues);
            return GetFullUrlBuilder(relativeUrl).ToString();
        }

        public string AbsoluteRoute(string routeName, object routeValues = null)
        {
            var relativeUrl = _urlHelper.RouteUrl(routeName, routeValues);
            return GetFullUrlBuilder(relativeUrl).ToString();
        }

        public UriBuilder GetFullUrlBuilder(string relativeUrl)
        {
            var result = GetAbsoluteUrlBuilder();

            var indexQ = relativeUrl.IndexOf('?');

            if (indexQ > 0)
            {
                result.Path = relativeUrl.Substring(0, indexQ);
                result.Query = relativeUrl.Substring(indexQ, relativeUrl.Length - indexQ);
            }
            else
                result.Path = relativeUrl;

            return result;
        }

        public UriBuilder GetAbsoluteUrlBuilder()
        {
            var request = _httpContextAccessor.ActionContext.HttpContext.Request;
            var headers = new RequestHeaders(request.Headers);
            var host = headers.Host.HasValue ? headers.Host.Host : request.Host.Value;
            var port = headers.Host.Port ?? 80;
            var builder = new UriBuilder(request.Scheme, host, port);
            return builder;
        }
    }
}