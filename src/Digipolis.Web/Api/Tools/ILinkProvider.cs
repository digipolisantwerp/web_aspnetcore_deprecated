using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Api.Tools
{
    public interface ILinkProvider
    {
        string AbsoluteAction(string actionName, string controllerName, object routeValues = null);

        string AbsoluteRoute(string routeName, object routeValues = null);
    }
}