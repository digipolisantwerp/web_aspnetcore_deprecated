using System.Collections.Generic;
using Digipolis.Web.Api;
using Digipolis.Web.SampleApi.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Digipolis.Web.SampleApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [Versions(Versions.V1, Versions.V2)]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Versions(Versions.V1, Versions.V2)]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Versions(Versions.V2)]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Versions(Versions.V2)]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Versions(Versions.V2)]
        public void Delete(int id)
        {
        }
    }
}
