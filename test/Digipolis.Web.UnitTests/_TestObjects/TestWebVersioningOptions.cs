using System;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.UnitTests
{
    public class TestWebVersioningOptions : IOptions<WebVersioningOptions>
    {
        public TestWebVersioningOptions(WebVersioningOptions options)
        {
            Value = options;
        }

        public WebVersioningOptions Value { get; private set; }
    }
}
