using Digipolis.Web.Api;
using Digipolis.Web.UnitTests.Modelbinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.SampleApi.Models
{
    public class CriteriaDto : PageSortOptions
    {
       
        public string[] StringArray { get; set; }

        public List<string> StringList { get; set; }

        public int[] IntArray { get; set; }

        public List<int> IntList { get; set; }

        public ChildDto[] ComplexArray { get; set; }

}
}
