using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;

namespace Digipolis.Web.Swagger
{
    internal class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var originalPaths = swaggerDoc.Paths;
            var newPaths = new OpenApiPaths();

            foreach (var path in originalPaths)
            {
                var key = path.Key.Replace("{apiVersion}", swaggerDoc.Info.Version).Replace("{apiversion}", swaggerDoc.Info.Version);
                var pathItem = path.Value;

                foreach (var op in pathItem.Operations)
                {
                    RemoveVersionParamFrom(op.Value);
                }

                newPaths.Add(key, pathItem);
            }

            swaggerDoc.Paths = newPaths;
        }

        private static void RemoveVersionParamFrom(OpenApiOperation operation)
        {
            var versionParam = operation?.Parameters?.FirstOrDefault(param => param.Name.Equals("apiVersion", StringComparison.CurrentCultureIgnoreCase));

            if (versionParam == null)
                return;

            operation.Parameters.Remove(versionParam);
        }
    }
}