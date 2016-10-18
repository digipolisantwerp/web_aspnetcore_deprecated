using System;

namespace Digipolis.Web
{
    public class WebVersioningOptions
    {
        /// <summary>
        /// The route wwhere the version can be requested (default = '/status/version').
        /// </summary>
        public string Route { get; set; } = "/status/version";
    }
}