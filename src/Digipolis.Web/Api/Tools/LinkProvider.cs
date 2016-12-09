using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

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

            //if (options?.Value?.BaseUrl != null)
            //    _baseUri = new Uri(options.Value.BaseUrl);
        }

        public string AbsoluteAction(string actionName, string controllerName, object routeValues = null)
        {
            //string scheme = _httpContextAccessor.ActionContext.HttpContext.Request.Scheme;
            //var helper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(_httpContextAccessor.ActionContext);
            var relativeUrl = _urlHelper.Action(actionName, controllerName, routeValues);

            return relativeUrl;

            //if (_baseUri == null) return relativeUrl;

            //Uri uri = new Uri(_baseUri, relativeUrl);
            //return uri.AbsoluteUri;
        }

        public string AbsoluteRoute(string routeName, object routeValues = null)
        {
            var relativeUrl = _urlHelper.RouteUrl(routeName, routeValues);
            return relativeUrl;

            //if (_baseUri == null) return relativeUrl;

            //Uri uri = new Uri(_baseUri, relativeUrl);
            //return uri.AbsoluteUri;
        }
    }
}
