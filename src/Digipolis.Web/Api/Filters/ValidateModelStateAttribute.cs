using Digipolis.Errors.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Api
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            throw new ValidationException(messages: context.ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)));
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                if (next != null)
                    await next();
            }
            else
                throw new ValidationException(messages: context.ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)));
        }
    }
}