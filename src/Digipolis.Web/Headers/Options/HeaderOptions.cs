using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Web.Headers;

namespace Digipolis.Web
{
    public class HeaderOptions
    {
        public HeaderOptions()
        {
            Handlers = new List<KeyValuePair<string, IHeaderHandler>>();
        }

        public HeaderOptions(IEnumerable<KeyValuePair<string, IHeaderHandler>> handlers)
        {
            Handlers = handlers.ToList();
        }

        public IEnumerable<KeyValuePair<string, IHeaderHandler>> Handlers { get; } 
    }
}
