using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Digipolis.Web.Swagger
{
    public class SwaggerResponseDefaults : SwaggerResponseDefinitions
    {
        protected override void ConfigureResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            UnauthorizedResponse(operation, context);
            InternalServerErrorResponse(operation, context);
            BadRequestResponse(operation, context);
            NoContentResponse(operation, context);
            CreatedResponse(operation, context);
            OkResponse(operation, context);
            NotFoundResponse(operation, context);
        }

        protected virtual void UnauthorizedResponse(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowsAnonymous = ActionAttributes.OfType<AllowAnonymousAttribute>().Any();
            if (operation.Responses.ContainsKey("401"))
            {
                if (allowsAnonymous) operation.Responses.Remove("401");
                else operation.Responses["401"].Description = "Unauthorized";
                return;
            }

            if (!CombinedAttributes.OfType<AuthorizeAttribute>().Any() || allowsAnonymous) return;
            operation.Responses.Add("401", new OpenApiResponse()
            {
                Description = "Unauthorized",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(Error), context.SchemaRepository)
                    }
                }
            });
        }

        protected virtual void BadRequestResponse(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("400")) return;
            if (!CombinedAttributes.Any(x => x is ValidateModelStateAttribute || x is HttpPostAttribute || x is HttpPutAttribute || x is HttpPatchAttribute))
                return;

            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Validation error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(Error), context.SchemaRepository)
                    }
                }
            });
        }

        protected virtual void InternalServerErrorResponse(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("500")) return;
            operation.Responses.Add("500", new OpenApiResponse()
            {
                Description = "Technical error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(Error), context.SchemaRepository)
                    }
                }
            });
        }

        protected virtual void NoContentResponse(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!CombinedAttributes.OfType<HttpDeleteAttribute>().Any()) return;
            operation.Responses.Remove("200");
            if (operation.Responses.ContainsKey("204")) return;

            operation.Responses.Add("204", new OpenApiResponse()
            {
                Description = "Removed"
            });
        }

        protected virtual void CreatedResponse(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!CombinedAttributes.OfType<HttpPostAttribute>().Any()) return;
            if (!operation.Responses.ContainsKey("201")) return;
            var response = operation.Responses["201"];
            if (response.Description.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                response.Description = "Created";
        }

        protected virtual void OkResponse(OpenApiOperation operation, OperationFilterContext contexts)
        {
            if (!operation.Responses.ContainsKey("200")) return;
            var response = operation.Responses["200"];
            if (response.Description.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                response.Description = "Ok";
        }

        protected virtual void NotFoundResponse(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("404"))
            {
                var response = operation.Responses["404"];
                if (response.Description.Equals("Client Error", StringComparison.CurrentCultureIgnoreCase))
                    response.Description = "Not found";
                return;
            }

            if (!context.ApiDescription.ParameterDescriptions.Any(x => !x.Name.Equals("apiVersion") && x.Source.Id.Equals("Path")))
                return;

            operation.Responses.Add("404", new OpenApiResponse()
            {
                Description = "Not found",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(Error), context.SchemaRepository)
                    }
                }
            });
        }
    }
}