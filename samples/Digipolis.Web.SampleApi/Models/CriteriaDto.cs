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
       
        public string[] stringArray { get; set; }

        public List<string> listArray { get; set; }

        public int[] intArray { get; set; }

        public List<int> listInt { get; set; }

    }
}
