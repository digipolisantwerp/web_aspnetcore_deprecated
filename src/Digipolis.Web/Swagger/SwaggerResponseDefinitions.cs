using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace Digipolis.Web.Swagger
{
    public abstract class SwaggerResponseDefinitions : IOperationFilter
    {
        protected IEnumerable<Attribute> ActionAttributes { get; private set; }

        protected IEnumerable<Attribute> ControllerAttributes { get; private set; }

        protected IEnumerable<Attribute> CombinedAttributes { get; private set; }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ActionAttributes = context.MethodInfo.GetCustomAttributes(true).OfType<Attribute>();
            ControllerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<Attribute>();
            CombinedAttributes = ActionAttributes.Union(ControllerAttributes);
            ConfigureResponses(operation, context);
            ExcludeSwaggerResonse(operation);
        }

        protected abstract void ConfigureResponses(OpenApiOperation operation, OperationFilterContext context);

        private void ExcludeSwaggerResonse(OpenApiOperation operation)
        {
            var excludes = CombinedAttributes.OfType<ExcludeSwaggerResonseAttribute>();
            if (!excludes.Any()) return;

            var codes = excludes.SelectMany(x => x.HttpCodes).Distinct().Select(x => x.ToString());
            foreach (var code in codes)
            {
                if (!operation.Responses.ContainsKey(code)) continue;
                operation.Responses.Remove(code);
            }
        }
    }
}