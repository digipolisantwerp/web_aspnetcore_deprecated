using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Headers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Digipolis.Web.UnitTests.Headers
{
    public class CorrelationHeaderHandlerTests
    {
        [Fact]
        private void LoggerNullRaisesArgumentNullException()
        {
            ILogger<CorrelationHeaderHandler> nullLogger = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new CorrelationHeaderHandler(nullLogger));
            Assert.Equal("logger", ex.ParamName);
        }
    }
}
