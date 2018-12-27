using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Digipolis.Web.Swagger
{
    public class AddConsumeProducesValues : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var produces = context.MethodInfo.GetCustomAttributes(true).OfType<ProducesAttribute>().LastOrDefault();
            var consumes = context.MethodInfo.GetCustomAttributes(true).OfType<ConsumesAttribute>().LastOrDefault();

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