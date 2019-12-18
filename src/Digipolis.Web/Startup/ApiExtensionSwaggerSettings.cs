using Digipolis.Web.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Digipolis.Web.Startup
{
    public class ApiExtensionSwaggerSettings : SwaggerSettings<SwaggerResponseDefaults>
    {
        protected override void Configuration(SwaggerGenOptions options)
        {
        }
    }
}
