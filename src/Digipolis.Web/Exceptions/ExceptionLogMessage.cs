using System;
using Digipolis.Errors;

namespace Digipolis.Web.Exceptions
{
    public class ExceptionLogMessage
    {
        public int HttpStatusCode { get; set; }
        public Error Error { get; set; }
        public Exception Exception { get; set; }
    }
}
