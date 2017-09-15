using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Digipolis.Web.Swagger
{
    public class AddFileUploadParams : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                return;

            var formFileParams = context.ApiDescription.ActionDescriptor.Parameters
                .Where(x => x.ParameterType.IsAssignableFrom(typeof(IFormFile)))
                .Select(x => x.Name)
                .ToList();

            var formFileSubParams = context.ApiDescription.ActionDescriptor.Parameters
                .SelectMany(x => x.ParameterType.GetProperties())
                .Where(x => x.PropertyType.IsAssignableFrom(typeof(IFormFile)))
                .Select(x => x.Name)
                .ToList();

            var allFileParamNames = formFileParams.Union(formFileSubParams).ToList();

            if (!allFileParamNames.Any())
                return;

            var paramsToRemove = new List<IParameter>();
            foreach (var param in operation.Parameters)
            {
                paramsToRemove.AddRange(from fileParamName in allFileParamNames
                    where param.Name.StartsWith(fileParamName + ".")
                    select param);
            }
            paramsToRemove.ForEach(x => operation.Parameters.Remove(x));
            foreach (var paramName in allFileParamNames)
            {
                var fileParam = new NonBodyParameter
                {
                    Type = "file",
                    Name = paramName,
                    In = "formData",
                    Required = true
                };
                operation.Parameters.Add(fileParam);
            }
            foreach (
                IParameter param in
                operation.Parameters.Where(x => x.In.Equals("form", StringComparison.CurrentCultureIgnoreCase)))
            {
                param.In = "formData";
            }

            operation.Consumes = new List<string>() {"multipart/form-data"};
        }
    }
}