using System;

namespace Digipolis.Web.Api
{
    public class Link
    {
        public Uri Href { get; set; }

        public Link()
        { }

        public Link(string url)
            :this(new Uri(url))
        {}

        public Link(Uri href)
        {
            Href = href;
        }
    }
}