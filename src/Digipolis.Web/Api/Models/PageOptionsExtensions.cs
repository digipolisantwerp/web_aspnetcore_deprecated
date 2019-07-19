using Digipolis.Web.Api.Tools;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Digipolis.Web.Api.Models
{
    public static class PageOptionsExtensions
    {
        private static IActionContextAccessor _actionContextAccessor;

        internal static void Configure(IActionContextAccessor actionContextAccessor)
        {
            if (actionContextAccessor == null)
                throw new ArgumentNullException(nameof(actionContextAccessor));

            _actionContextAccessor = actionContextAccessor;
        }

        #region PageOptions extension methods

        public static PagedResult<T> ToPagedResult<T>(this PageOptions pageOptions, IEnumerable<T> data, int total, string actionName, string controllerName, object routeValues = null) where T : class
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
            
            result.Links.Last = null;
            result.Links.Previous = null;
            result.Links.Next = null;

            if (pageOptions.PagingStrategy == PagingStrategy.WithCount)
            {
                // last page
                result.Links.Last = GenerateLink(pageOptions, result.Page.TotalPages, actionName, controllerName, routeValues);

                // previous page: if current page = 1, there is no previous page
                if (pageOptions.Page - 1 >= 1) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, actionName, controllerName, routeValues);

                // next page: if current page is last page, there is no next page
                if (pageOptions.Page + 1 <= result.Page.TotalPages) result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, actionName, controllerName, routeValues);
            }
            else
            {
                // paging strategy NOCOUNT

                // last page: unknown, so stays assigned to null
                // previous page: if current page = 1, there is no previous page
                if (pageOptions.Page - 1 >= 1) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, actionName, controllerName, routeValues);

                // next page: unkwnon if it exists, but will be assigned nevertheless
                result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, actionName, controllerName, routeValues);

                // totalElements and TotalPages is unknown
                result.Page.TotalElements = null;
                result.Page.TotalPages = null;
            }

            return result;
        }

        public static PagedResult<T> ToPagedResult<T>(this PageOptions pageOptions, IEnumerable<T> data, int total, string routeName, object routeValues = null) where T : class
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
            
            result.Links.Last = null;
            result.Links.Previous = null;
            result.Links.Next = null;

            if (pageOptions.PagingStrategy == PagingStrategy.WithCount)
            {
                // last page
                result.Links.Last = GenerateLink(pageOptions, result.Page.TotalPages, routeName, routeValues);

                // previous page: if current page = 1, there is no previous page
                if (pageOptions.Page - 1 >= 1) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, routeName, routeValues);

                // next page: if current page is last page, there is no next page
                if (pageOptions.Page + 1 <= result.Page.TotalPages) result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, routeName, routeValues);
            }
            else
            {
                // paging strategy NOCOUNT

                // last page: unknown, so stays assigned to null
                // previous page: if current page = 1, there is no previous page
                if (pageOptions.Page - 1 >= 1) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, routeName, routeValues);

                // next page: unkwnon if it exists, but will be assigned nevertheless
                result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, routeName, routeValues);

                // totalElements and TotalPages is unknown
                result.Page.TotalElements = null;
                result.Page.TotalPages = null;
            }
            
            return result;
        }

        public static PagedResult<T, EmbeddedT> ToPagedResult<T, EmbeddedT>(this PageOptions pageOptions, IEnumerable<T> data, int? total) where EmbeddedT : Embedded<T>, new() where T : class
        {
            var descriptor = (ControllerActionDescriptor)_actionContextAccessor.ActionContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerName;
            var routeValues = _actionContextAccessor.ActionContext.RouteData.Values;

            var result = new PagedResult<T, EmbeddedT>(pageOptions.Page, pageOptions.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(pageOptions, 1, actionName, controllerName, routeValues),
                    Self = GenerateLink(pageOptions, pageOptions.Page, actionName, controllerName, routeValues)
                }
            };

            result.Links.Last = null;
            result.Links.Previous = null;
            result.Links.Next = null;

            if (pageOptions.PagingStrategy == PagingStrategy.WithCount)
            {
                // last page
                result.Links.Last = GenerateLink(pageOptions, result.Page.TotalPages, actionName, controllerName, routeValues);

                // previous page: if current page = 1, there is no previous page
                if (pageOptions.Page - 1 >= 1) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, actionName, controllerName, routeValues);

                // next page: if current page is last page, there is no next page
                if (pageOptions.Page + 1 <= result.Page.TotalPages) result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, actionName, controllerName, routeValues);
            }
            else
            {
                // paging strategy NOCOUNT

                // last page: unknown, so stays assigned to null
                // previous page: if current page = 1, there is no previous page
                if (pageOptions.Page - 1 >= 1) result.Links.Previous = GenerateLink(pageOptions, pageOptions.Page - 1, actionName, controllerName, routeValues);

                // next page: unkwnon if it exists, but will be assigned nevertheless
                result.Links.Next = GenerateLink(pageOptions, pageOptions.Page + 1, actionName, controllerName, routeValues);

                // totalElements and TotalPages is unknown
                result.Page.TotalElements = null;
                result.Page.TotalPages = null;
            }

            return result;
        }

        #endregion

        #region PageSortOptions extension methods

        public static PagedResult<T> ToPagedResult<T>(this PageSortOptions pageSortOptions, IEnumerable<T> data, int total, string actionName, string controllerName, object routeValues = null) where T : class
        {
            return ((PageOptions)pageSortOptions).ToPagedResult<T>(data, total, actionName, controllerName, routeValues);
        }

        public static PagedResult<T> ToPagedResult<T>(this PageSortOptions pageSortOptions, IEnumerable<T> data, int total, string routeName, object routeValues = null) where T : class
        {
            return ((PageOptions)pageSortOptions).ToPagedResult<T>(data, total, routeName, routeValues);
        }
        
        public static PagedResult<T, EmbeddedT> ToPagedResult<T, EmbeddedT>(this PageSortOptions pageSortOptions, IEnumerable<T> data, int total) where EmbeddedT : Embedded<T>, new() where T : class
        {            
            return ((PageOptions)pageSortOptions).ToPagedResult<T, EmbeddedT>(data, total);
        }

        #endregion

        internal static Link GenerateLink(PageOptions pageOptions, int? page, string actionName, string controllerName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues);
            var query = _actionContextAccessor.ActionContext.HttpContext.Request.Query;
            foreach (var item in query)
            {
                values[item.Key.ToLowerInvariant()] = item.Value;
            }
            values["page"] = page;
            values["pagesize"] = pageOptions.PageSize;
            var url = GetLinkProvider().AbsoluteAction(actionName, controllerName, values);
            return new Link(url.ToLowerInvariant());
        }

        internal static Link GenerateLink(PageOptions pageOptions, int? page, string routeName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues);
            var query = _actionContextAccessor.ActionContext.HttpContext.Request.Query;
            foreach (var item in query)
            {
                values[item.Key.ToLowerInvariant()] = item.Value;
            }
            values["page"] = page;
            values["pagesize"] = pageOptions.PageSize;
            var url = GetLinkProvider().AbsoluteRoute(routeName, values);
            return new Link(url.ToLowerInvariant());
        }

        internal static Link GenerateLink(this PageSortOptions pageSortOptions, int? page, string actionName, string controllerName, object routeValues = null)
        {           
            var values = new RouteValueDictionary(routeValues);
            var query = _actionContextAccessor.ActionContext.HttpContext.Request.Query;
            foreach (var item in query)
            {
                values[item.Key.ToLowerInvariant()] = item.Value;
            }
            values["page"] = page;
            values["pageSize"] = pageSortOptions.PageSize;
            values["sort"] = pageSortOptions.Sort;
            var url = GetLinkProvider().AbsoluteAction(actionName, controllerName, values);
            return new Link(url.ToLowerInvariant());
        }

        internal static Link GenerateLink(this PageSortOptions pageSortOptions, int? page, string routeName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues);
            var query = _actionContextAccessor.ActionContext.HttpContext.Request.Query;
            foreach (var item in query)
            {
                values[item.Key.ToLowerInvariant()] = item.Value;
            }
            values["page"] = page;
            values["pageSize"] = pageSortOptions.PageSize;
            values["sort"] = pageSortOptions.Sort;
            var url = GetLinkProvider().AbsoluteRoute(routeName, values);
            return new Link(url.ToLowerInvariant());
        }

        private static ILinkProvider GetLinkProvider()
        {
            if (_actionContextAccessor == null)
                throw new TypeInitializationException(nameof(PageOptionsExtensions), new Exception(@"PageOptionsExtensions._actionContextAccessor was not initialized, did you Configure using ""UseApiExtensions"" in Startup?"));

            return (ILinkProvider)_actionContextAccessor.ActionContext.HttpContext.RequestServices.GetService(typeof(ILinkProvider));
        }
    }
}
