using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Headers;

namespace Digipolis.Web.Api.Tools
{
    public class LinkProvider : ILinkProvider
    {
        private IActionContextAccessor _httpContextAccessor;
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
            var relativeUrl =  _urlHelper.Action(actionName, controllerName, routeValues);

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
            HttpRequest request = _httpContextAccessor.ActionContext.HttpContext.Request;
            RequestHeaders headers = new RequestHeaders(request.Headers);
            var host = headers.Host.HasValue ? headers.Host.Host: request.Host.Value;
            //var port = headers.Host.Port ?? 80;

            if (headers.Host.Port.HasValue)
            {
                return new UriBuilder(request.Scheme, host, headers.Host.Port.Value);
            }
            else
            {
                return new UriBuilder(request.Scheme, host);
            }
        }
    }
}
