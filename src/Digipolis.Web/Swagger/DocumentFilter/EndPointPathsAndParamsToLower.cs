using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Web.Api.Tools;
using Microsoft.OpenApi.Models;

namespace Digipolis.Web.Swagger
{
    internal class EndPointPathsAndParamsToLower : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var newPaths = new Dictionary<string, OpenApiPathItem>();
            foreach (var path in swaggerDoc.Paths)
            {
                var res = HandlePath(path.Value);
                newPaths.Add(path.Key.ToLowerInvariant(), res);
            }

            var dict = new OpenApiPaths();
            dict.AddRangeIfNotExist(newPaths);

            swaggerDoc.Paths = dict;
        }

        private OpenApiPathItem HandlePath(OpenApiPathItem value)
        {
            value.Parameters = HandleParameters(value.Parameters);
            foreach (var operation in value.Operations)
            {
                operation.Value.Parameters = HandleParameters(operation.Value.Parameters);
            }

            return value;
        }

        private static IList<OpenApiParameter> HandleParameters(IList<OpenApiParameter> parameters)
        {
            if (parameters == null) return null;
            foreach (var item in parameters)
            {
                item.Name = item.Name?.ToLowerInvariant();
            }

            return parameters;
        }
    }
}