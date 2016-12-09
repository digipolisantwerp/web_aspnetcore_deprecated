using Digipolis.Errors;
using Newtonsoft.Json;

namespace Digipolis.Web.Api
{
    public class ApiExtensionOptions
    {
        /// <summary>
        /// Disable api versioning when enabled in code
        /// </summary>
        public bool DisableVersioning { get; set; }

        /// <summary>
        /// Disable global error handling when enabled in code
        /// </summary>
        public bool DisableGlobalErrorHandling { get; set; }

        /// <summary>
        /// Number of pages to show upon retrieving collections
        /// </summary>
        public int PageSize { get; set; }

        ///// <summary>
        ///// BaseUrl used for generation of HAL-urls
        ///// </summary>
        //public string BaseUrl { get; set; }

        /// <summary>
        /// The route wwhere the version can be requested (default = '/status/version').
        /// </summary>
        //public string Route { get; set; } = "/status/version";
    }
}
