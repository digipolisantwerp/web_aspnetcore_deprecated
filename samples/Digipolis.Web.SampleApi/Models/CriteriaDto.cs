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
        public bool? TestBool { get; set; }
       
        public string[] StringArray { get; set; }

        public List<string> StringList { get; set; }

        public int[] IntArray { get; set; }

        public List<int> IntList { get; set; }

        public ChildDto[] ComplexArray { get; set; }

}
}
