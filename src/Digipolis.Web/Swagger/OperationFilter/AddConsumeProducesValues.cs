using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace Digipolis.Web.Swagger
{
    public class AddConsumeProducesValues : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var produces = context.ApiDescription.GetActionAttributes().OfType<ProducesAttribute>().LastOrDefault();
            var consumes = context.ApiDescription.GetActionAttributes().OfType<ConsumesAttribute>().LastOrDefault();

            if (produces != null) {
                foreach (var responseType in produces.ContentTypes)
                {
                    if (!operation.Produces.Contains(responseType))
                        operation.Produces.Add(responseType);
                }
            };

            if (consumes != null)
            {
                foreach (var responseType in consumes.ContentTypes)
                {
                    if (!operation.Consumes.Contains(responseType))
                        operation.Consumes.Add(responseType);
                }
            };
        }
    }
}