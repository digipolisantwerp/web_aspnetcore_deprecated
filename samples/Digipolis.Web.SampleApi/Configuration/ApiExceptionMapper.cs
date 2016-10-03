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
        }

        protected override void CreateDefaultMap(Error error, Exception exception)
        {
            error.Status = (int)HttpStatusCode.InternalServerError;
            error.Title = "We are experiencing some technical difficulties.";
        }
    }
}
