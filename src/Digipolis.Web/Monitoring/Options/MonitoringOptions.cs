using System;

namespace Digipolis.Web.Monitoring
{
    public class MonitoringOptions
    {
        /// <summary>
        /// The route where the ping can be requested (default = '/monitoring/ping').
        /// </summary>
        public string Route { get; set; } = "/monitoring/ping";

    }
}