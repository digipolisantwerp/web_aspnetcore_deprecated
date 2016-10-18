using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Swagger
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ExcludeSwaggerResonseAttribute : Attribute
    {
        public int[] HttpCodes { get; set; }

        public ExcludeSwaggerResonseAttribute(params int[] httpCodes)
        {
            HttpCodes = httpCodes;
        }
    }
}
