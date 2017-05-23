using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace Digipolis.Web.Swagger
{
    public abstract class SwaggerResponseDefinitions : IOperationFilter
    {
        protected IEnumerable<Attribute> ActionAttributes { get; private set; }

        protected IEnumerable<Attribute> ControllerAttributes { get; private set; }

        protected IEnumerable<Attribute> CombinedAttributes { get; private set; }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            ActionAttributes = context.ApiDescription.GetActionAttributes().OfType<Attribute>();
            ControllerAttributes = context.ApiDescription.GetControllerAttributes().OfType<Attribute>();
            CombinedAttributes = ActionAttributes.Union(ControllerAttributes);
            ConfigureResponses(operation, context);
            ExcludeSwaggerResonse(operation);
        }

        protected abstract void ConfigureResponses(Operation operation, OperationFilterContext context);

        private void ExcludeSwaggerResonse(Operation operation)
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