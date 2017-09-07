using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Monitoring
{
    public class Component
    {
        /// <summary>
        /// The status is ok when the component is functionally working. The status is warning when the component is still functionally working, but needs your attention. The status is error when the component is no longer able to provide the required functionality to the API.
        /// </summary>
        [Required]
        public Status Status { get; set; }

        /// <summary>
        /// The logical name of the component.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The type of the component. Examples are API, Database, SAP, ...
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Details about the component that can be displayed in the check-mk dashboard
        /// </summary>
        [Required]
        public string Details { get; set; }

        /// <summary>
        /// Optional field describing the error when the component is in status warning or error
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
