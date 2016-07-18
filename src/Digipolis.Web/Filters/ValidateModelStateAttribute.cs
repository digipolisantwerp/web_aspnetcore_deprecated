using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Digipolis.Errors;

namespace Digipolis.Web.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = new Error();

                foreach (var item in context.ModelState)
                {
                    foreach (var err in item.Value.Errors)
                    {
                        error.AddErrorMessage(new ErrorMessage(item.Key, err.ErrorMessage));
                    }
                }

                context.Result = new BadRequestObjectResult(error);
            }
        }
    }
}
