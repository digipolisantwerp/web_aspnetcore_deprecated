using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Monitoring
{
    public class Monitoring
    {
        /// <summary>
        /// The global status of the API. It is determined by the status of each of the components. The status is ok when every component is ok and the API is functionally working. The status is warning when at least one component has warning as status. The API is functionally still up and running, but a non critical component needs your attention. The status is error when at least one component has error as status. The API is functionally no longer working.
        /// </summary>
        [Required]
        public Status Status { get; set; }


        /// <summary>
        /// The components this API functionally depends on. At least one component needs to provide the functionality of the API.
        /// </summary>
        [Required]

        public Component[] Components { get; set; }

    }
}
