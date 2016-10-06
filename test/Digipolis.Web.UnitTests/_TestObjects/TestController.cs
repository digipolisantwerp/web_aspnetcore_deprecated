using System;
using Microsoft.AspNetCore.Mvc;

namespace Digipolis.Web.UnitTests
{
    public class TestController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get()
        {
            return new OkResult();
        }

        public IActionResult GetSingle()
        {
            return new OkResult();
        }
    }
}
