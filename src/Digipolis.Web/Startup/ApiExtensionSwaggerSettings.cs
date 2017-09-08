using System.IO;
using Digipolis.Web.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Digipolis.Web.Api;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Startup
{
    public class ApiExtensionSwaggerSettings : SwaggerSettings<SwaggerResponseDefaults>
    {
        protected override void Configuration(SwaggerGenOptions options)
        {
            options.DescribeAllEnumsAsStrings();
        }


    }
}
