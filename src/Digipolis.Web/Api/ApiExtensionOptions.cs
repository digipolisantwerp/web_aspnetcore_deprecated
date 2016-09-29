using Digipolis.Errors;
using Newtonsoft.Json;

namespace Digipolis.Web.Api
{
    public class ApiExtensionOptions
    {
        public bool EnableVersioning { get; set; }

        public bool EnableGlobalErrorHandling { get; set; }

        [JsonIgnore]
        public IExceptionMapper ExceptionMapper { get; set; }

        public int PageSize { get; set; }
    }
}
