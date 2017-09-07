using System;
using System.Linq;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Digipolis.Web.Swagger
{
    public class SwaggerResponseDefaults : SwaggerResponseDefinitions
    {
        protected override void ConfigureResponses(Operation operation, OperationFilterContext context)
        {
            UnauthorizedResponse(operation, context);
            InternalServerErrorResponse(operation, context);
            BadRequestResponse(operation, context);
            NoContentResponse(operation, context);
            CreatedResponse(operation, context);
            OkResponse(operation, context);
            NotFoundResponse(operation, context);
            //AddReturnSchemaToVersionEndpoint(operation, context);
        }

        protected virtual void UnauthorizedResponse(Operation operation, OperationFilterContext context)
        {
            var allowsAnonymous = ActionAttributes.OfType<AllowAnonymousAttribute>().Any();
            if (operation.Responses.ContainsKey("401"))
            {
                if (allowsAnonymous) operation.Responses.Remove("401");
                else operation.Responses["401"].Description = "Unauthorized";
                return;
            };

            if (!CombinedAttributes.OfType<AuthorizeAttribute>().Any() || allowsAnonymous) return;
            operation.Responses.Add("401", new Response
            {
                Description = "Unauthorized",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error))
            });
        }

        protected virtual void BadRequestResponse(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("400")) return;
            if (!CombinedAttributes.Any(x => x is ValidateModelStateAttribute || x is HttpPostAttribute || x is HttpPutAttribute || x is HttpPatchAttribute))
                return;

            operation.Responses.Add("400", new Response
            {
                Description = "Validation error",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error))
            });
        }

        protected virtual void InternalServerErrorResponse(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("500")) return;
            operation.Responses.Add("500", new Response
            {
                Description = "Technical error",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error)),
            });
        }

        protected virtual void NoContentResponse(Operation operation, OperationFilterContext context)
        {
            if (!CombinedAttributes.OfType<HttpDeleteAttribute>().Any()) return;
            operation.Responses.Remove("200");
            if (operation.Responses.ContainsKey("204")) return;

            operation.Responses.Add("204", new Response
            {
                Description = "Removed"
            });
        }

        protected virtual void CreatedResponse(Operation operation, OperationFilterContext context)
        {
            if (!CombinedAttributes.OfType<HttpPostAttribute>().Any()) return;
            if (!operation.Responses.ContainsKey("201")) return;
            var response = operation.Responses["201"];
            if (response.Description.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                response.Description = "Created";
        }

        protected virtual void OkResponse(Operation operation, OperationFilterContext contexts)
        {
            if (!operation.Responses.ContainsKey("200")) return;
            var response = operation.Responses["200"];
            if (response.Description.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                response.Description = "Ok";
        }

        protected virtual void NotFoundResponse(Operation operation, OperationFilterContext context)
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

            operation.Responses.Add("404", new Response
            {
                Description = "Not found",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error))
            });
        }

        //protected virtual void AddReturnSchemaToVersionEndpoint(Operation operation, OperationFilterContext context)
        //{
        //    var operationId = context.ApiDescription.RelativePath.Contains("apiVersion") ? "ByApiVersionStatusVersionGet" : "StatusVersionGet";
        //    if (!operation.OperationId.Equals(operationId)) return;
        //    operation.Responses["200"].Schema = context.SchemaRegistry.GetOrRegister(typeof(AppVersion));
        //    operation.Summary = "Get the version of the application";
        //    operation.Produces.Add("application/json");
        //}

       
    }
}