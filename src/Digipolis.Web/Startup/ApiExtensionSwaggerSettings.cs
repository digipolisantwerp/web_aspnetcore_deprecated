using System.IO;
using Digipolis.Web.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Startup
{
    public class ApiExtensionSwaggerSettings : SwaggerSettings<SwaggerResponseDefinitions>
    {
        protected override void Configuration(SwaggerGenOptions options)
        {
            options.DescribeAllEnumsAsStrings();
            options.OperationFilter<AddFileUploadParams>();
            options.DocumentFilter<EndPointPathsAndParamsToLower>();
            options.DocumentFilter<SetVersionInPaths>();
            options.SchemaFilter<PagedResultSchemaFilter>();
        }
    }
}
