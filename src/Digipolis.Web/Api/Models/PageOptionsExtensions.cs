using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Api.Tools;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Digipolis.Web.Api.Models
{
    public static class PageOptionsExtensions
    {
        private static IActionContextAccessor _actionContextAccessor;

        private static ILinkProvider GetLinkProvider()
        {
            if (_actionContextAccessor == null)
                throw new TypeInitializationException(nameof(PageOptionsExtensions), new Exception(@"PageOptionsExtensions._actionContextAccessor was not initialized, did you Configure using ""UseApiExtensions"" in Startup?"));

            return (ILinkProvider)_actionContextAccessor.ActionContext.HttpContext.RequestServices.GetService(typeof(ILinkProvider));
        }


        internal static void Configure(IActionContextAccessor actionContextAccessor)
        {
            if (actionContextAccessor == null)
                throw new ArgumentNullException(nameof(actionContextAccessor));

            _actionContextAccessor = actionContextAccessor;
        }


        public static PagedResult<T> ToPagedResult<T>(this PageOptions pageOptions, IEnumerable<T> data, int total, string actionName, string controllerName, object routeValues = null) where T : class, new()
        {

            if (string.IsNullOrWhiteSpace(actionName)) throw new ArgumentNullException(nameof(actionName));
            if (string.IsNullOrWhiteSpace(controllerName)) throw new ArgumentNullException(nameof(controllerName));

            var result = new PagedResult<T>(pageOptions.Page, pageOptions.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(pageOptions, 1, actionName, controllerName, routeValues),
                    Self = GenerateLink(pageOptions, pageOptions.Page, actionName, controllerName, routeValues)
                }
            };
            result.Links.Last = GenerateLink(pageOptions, result.Page.TotalPages, actionName, controllerName, routeValues);

            if (pageOptions.Page - 1 > 0) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, actionName, controllerName, routeValues);
            if (pageOptions.Page + 1 < result.Page.TotalPages) result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, actionName, controllerName, routeValues);

            return result;
        }

        public static PagedResult<T> ToPagedResult<T>(this PageOptions pageOptions, IEnumerable<T> data, int total, string routeName, object routeValues = null) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

            var result = new PagedResult<T>(pageOptions.Page, pageOptions.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(pageOptions, 1, routeName, routeValues),
                    Self = GenerateLink(pageOptions, pageOptions.Page, routeName, routeValues)
                }
            };
            result.Links.Last = GenerateLink(pageOptions, result.Page.TotalPages, routeName, routeValues);

            if (pageOptions.Page - 1 > 0) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, routeName, routeValues);
            if (pageOptions.Page + 1 < result.Page.TotalPages) result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, routeName, routeValues);

            return result;
        }

        public static PagedResult<T> ToPagedResult<T>(this PageSortOptions pageSortOptions, IEnumerable<T> data, int total, string actionName, string controllerName, object routeValues = null) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(actionName)) throw new ArgumentNullException(nameof(actionName));
            if (string.IsNullOrWhiteSpace(controllerName)) throw new ArgumentNullException(nameof(controllerName));

            var result = new PagedResult<T>(pageSortOptions.Page, pageSortOptions.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(pageSortOptions, 1, actionName, controllerName, routeValues),
                    Self = GenerateLink(pageSortOptions, pageSortOptions.Page, actionName, controllerName, routeValues)
                }
            };
            result.Links.Last = GenerateLink(pageSortOptions, result.Page.TotalPages, actionName, controllerName, routeValues);

            if (pageSortOptions.Page - 1 > 0) result.Links.Previous = GenerateLink(pageSortOptions, pageSortOptions.Page - 1, actionName, controllerName, routeValues);
            if (pageSortOptions.Page + 1 < result.Page.TotalPages) result.Links.Next = GenerateLink(pageSortOptions, pageSortOptions.Page + 1, actionName, controllerName, routeValues);

            return result;
        }

        public static PagedResult<T> ToPagedResult<T>(this PageSortOptions pageSortOptions, IEnumerable<T> data, int total, string routeName, object routeValues = null) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

            var result = new PagedResult<T>(pageSortOptions.Page, pageSortOptions.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(pageSortOptions, 1, routeName, routeValues),
                    Self = GenerateLink(pageSortOptions, pageSortOptions.Page, routeName, routeValues)
                }
            };
            result.Links.Last = GenerateLink(pageSortOptions, result.Page.TotalPages, routeName, routeValues);

            if (pageSortOptions.Page - 1 > 0) result.Links.Previous = GenerateLink(pageSortOptions, pageSortOptions.Page - 1, routeName, routeValues);
            if (pageSortOptions.Page + 1 < result.Page.TotalPages) result.Links.Next = GenerateLink(pageSortOptions, pageSortOptions.Page + 1, routeName, routeValues);

            return result;
        }

        internal static Link GenerateLink(PageOptions pageOptions, int page, string actionName, string controllerName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = page,
                ["PageSize"] = pageOptions.PageSize
            };

            var url = GetLinkProvider().AbsoluteAction(actionName, controllerName, values);
            return new Link(url.ToLowerInvariant());
        }

        internal static Link GenerateLink(PageOptions pageOptions, int page, string routeName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = page,
                ["PageSize"] = pageOptions.PageSize
            };

            var url = GetLinkProvider().AbsoluteRoute(routeName, values);
            return new Link(url.ToLowerInvariant());
        }

        internal static Link GenerateLink(PageSortOptions pageSortOptions, int page, string actionName, string controllerName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = page,
                ["PageSize"] = pageSortOptions.PageSize,
                ["Sort"] = pageSortOptions.Sort
            };

            var url = GetLinkProvider().AbsoluteAction(actionName, controllerName, values);
            return new Link(url.ToLowerInvariant());
        }

        internal static Link GenerateLink(PageSortOptions pageSortOptions, int page, string routeName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = page,
                ["PageSize"] = pageSortOptions.PageSize,
                ["Sort"] = pageSortOptions.Sort
            };

            var url = GetLinkProvider().AbsoluteRoute(routeName, values);
            return new Link(url.ToLowerInvariant());
        }
    }
}
