using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Digipolis.Web.SampleApi.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Digipolis.Web.SampleApi.Swagger
{
    [ApiExplorerSettings(IgnoreApi=true)]
    public class SwaggerUiController : Controller
    {
        [HttpGet("swagger/ui/index.html")]
        public IActionResult Index()
        {
            return View("~/Swagger/index.cshtml", GetDiscoveryUrls());
        }

        private IDictionary<string, string> GetDiscoveryUrls()
        {
            var versions = typeof(Versions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .ToDictionary(x=> x.GetRawConstantValue().ToString(), x => string.Format("/swagger/{0}/swagger.json", x.GetRawConstantValue()));

            return versions;
        }
    }
}
