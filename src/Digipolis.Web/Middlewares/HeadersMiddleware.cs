using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Digipolis.Web.Headers
{
    public class HeadersMiddleware
    {
        private ILogger<HeadersMiddleware> _logger;
        private readonly RequestDelegate _next;
        private IEnumerable<KeyValuePair<string, IHeaderHandler>> _handlers;

        public HeadersMiddleware(RequestDelegate next, ILogger<HeadersMiddleware> logger)
        {
            if ( next == null ) throw new ArgumentNullException(nameof(next), $"{nameof(next)} cannot be null.");
            if ( logger == null ) throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null.");

            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            // te verwerken headers aflopen

            foreach ( var handler in _handlers )
            {
                var key = handler.Key;
                if ( context.Request.Headers.ContainsKey(key) )
                {
                    var values = context.Request.Headers[key];
                    handler.Value.Handle(values);
                }
            }

            








            return _next.Invoke(context);
        }
    }
}
