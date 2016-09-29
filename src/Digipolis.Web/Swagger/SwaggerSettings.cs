using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Swagger
{
    public abstract class SwaggerSettings<TSwaggerResponseDefinitions> where TSwaggerResponseDefinitions : SwaggerResponseDefinitions
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<TSwaggerResponseDefinitions>();
            var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml");
            if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);
            Configuration(options);
        }

        protected abstract void Configuration(SwaggerGenOptions options);
    }
}