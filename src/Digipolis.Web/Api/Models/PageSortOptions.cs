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
        public string[] Sort { get; set; }
    }
}