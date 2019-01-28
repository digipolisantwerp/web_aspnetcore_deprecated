using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace Digipolis.Web.Swagger
{
    public abstract class SwaggerSettings<TSwaggerResponseDefinitions> where TSwaggerResponseDefinitions : SwaggerResponseDefinitions
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<TSwaggerResponseDefinitions>();

            // determine base path for the application.
            var basePath = AppContext.BaseDirectory;                       
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");

            var xmlPath = Path.Combine(basePath, fileName);
            if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);

            options.OperationFilter<AddFileUploadParams>();
            options.OperationFilter<AddConsumeProducesValues>();
            options.OperationFilter<ValidRefUri>();
            options.DocumentFilter<SetVersionInPaths>();
            options.DocumentFilter<EndPointPathsAndParamsToLower>();

            Configuration(options);
        }

        protected abstract void Configuration(SwaggerGenOptions options);
    }
}