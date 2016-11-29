using System;

namespace Digipolis.Web.Api
{
    public class Link
    {
        public string Href { get; set; }

        public Link()
        { }

        public Link(string href)
        {
            Href = href;
        }
    }
}