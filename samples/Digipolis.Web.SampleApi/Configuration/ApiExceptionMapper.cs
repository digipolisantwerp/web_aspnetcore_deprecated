using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Digipolis.Errors;

namespace Digipolis.Web.SampleApi.Configuration
{
    public class ApiExceptionMapper : ExceptionMapper
    {
        protected override void Configure()
        {
            CreateMap<UnauthorizedAccessException>((error, ex) =>
            {
                error.Title = "Access denied.";
                error.Code = "UNAUTH001";
                error.Status = (int)HttpStatusCode.Forbidden;
            });
        }

        protected override void CreateDefaultMap(Error error, Exception exception)
        {
            error.Status = (int)HttpStatusCode.InternalServerError;
            error.Title = "We are experiencing some technical difficulties.";
        }
    }
}
