using Digipolis.Web.Api;
using Digipolis.Web.Modelbinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.SampleApi.Models
{
    public class CriteriaDto : PageSortOptions
    {
        /// <summary>
        ///     Test bool
        /// </summary>
        public bool? TestBool { get; set; }

        /// <summary>
        ///     Test String array
        /// </summary>
        public string[] StringArray { get; set; }

        /// <summary>
        ///     Test String List
        /// </summary>
        public List<string> StringList { get; set; }

        /// <summary>
        ///     Test Int Array
        /// </summary>
        public int[] IntArray { get; set; }

        /// <summary>
        ///     Test Int List
        /// </summary>
        public List<int> IntList { get; set; }

        /// <summary>
        ///    Test Complex array
        /// </summary>
        public ChildDto[] ComplexArray { get; set; }
    }
}