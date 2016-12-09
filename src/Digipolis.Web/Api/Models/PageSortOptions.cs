using Digipolis.Web.UnitTests.Modelbinders;
using Microsoft.AspNetCore.Mvc;

namespace Digipolis.Web.Api
{
    /// <summary>
    /// Options for sorting and paging Api results
    /// </summary>
    public class PageSortOptions : PageOptions
    {
        /// <summary>
        /// Sort by fields. Multiple values can be specified by comma seperation.
        /// Default sorting is ascending, for descending add a -(minus) sign before the field.
        /// </summary>
        [ModelBinder(BinderType = typeof(CommaDelimitedArrayModelBinder))]
        public string[] Sort { get; set; } = new string[0];
    }
}