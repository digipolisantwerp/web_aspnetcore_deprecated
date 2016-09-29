using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Api
{
    /// <summary>
    /// Options for paging Api results
    /// </summary>
    public class PageOptions
    {
        /// <summary>
        /// Retrieve page
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Size of the page
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
