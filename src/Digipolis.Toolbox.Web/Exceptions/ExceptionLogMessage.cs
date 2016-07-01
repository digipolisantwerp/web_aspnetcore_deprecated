using System;
using Digipolis.Toolbox.Errors;

namespace Digipolis.Toolbox.Web.Exceptions
{
    public class ExceptionLogMessage
    {
        public int HttpStatusCode { get; set; }
        public Error Error { get; set; }
        public Exception Exception { get; set; }
    }
}
