using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Api;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.UnitTests._TestObjects
{
    public class TestApiExtensionOptions : IOptions<ApiExtensionOptions>
    {
        public TestApiExtensionOptions(ApiExtensionOptions options)
        {
            Value = options;
        }

        public ApiExtensionOptions Value { get; }
    }
}
