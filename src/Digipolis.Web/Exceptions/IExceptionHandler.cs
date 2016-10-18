using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Digipolis.Web.Exceptions
{
    public interface IExceptionHandler
    {
        Task HandleAsync(HttpContext context, Exception ex);

        void Handle(HttpContext context, Exception ex);
    }
}