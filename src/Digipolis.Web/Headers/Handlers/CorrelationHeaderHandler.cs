using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Digipolis.Web.Headers
{
    public class CorrelationHeaderHandler : IHeaderHandler
    {
        private readonly ILogger<CorrelationHeaderHandler> _logger;

        public CorrelationHeaderHandler(ILogger<CorrelationHeaderHandler> logger)
        {
            if ( logger == null ) throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null.");
            _logger = logger;
        }

        public void Handle(StringValues values)
        {
            throw new NotImplementedException();
        }
    }
}
