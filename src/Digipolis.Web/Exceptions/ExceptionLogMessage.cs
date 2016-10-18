using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;

namespace Digipolis.Web.Exceptions
{
    public class ExceptionLogMessage
    {
        public Error Error { get; set; }

        public Exception Exception { get; set; }
    }
}
