using System;
using Microsoft.AspNetCore.Mvc;

namespace Digipolis.Web.UnitTests
{
    public class TestController
    {
        public IActionResult Get()
        {
            return new OkResult();
        }
    }
}
