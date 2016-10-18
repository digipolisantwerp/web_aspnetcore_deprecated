using System;
using System.Threading.Tasks;
using Digipolis.Web.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digipolis.Web.Api.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IExceptionHandler _handler;

        public GlobalExceptionFilter(IExceptionHandler handler)
        {
            if(handler == null) throw new ArgumentNullException(nameof(handler));
            _handler = handler;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context == null || _handler == null) return;
            await _handler.HandleAsync(context.HttpContext, context.Exception);
            context.ExceptionHandled = true;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context == null || _handler == null) return;
            _handler.Handle(context.HttpContext, context.Exception);
            context.ExceptionHandled = true;
        }
    }
}
