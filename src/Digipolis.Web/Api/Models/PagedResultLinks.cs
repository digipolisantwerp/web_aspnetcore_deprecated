using Newtonsoft.Json;

namespace Digipolis.Web.Api
{
    public class PagedResultLinks
    {
        [JsonProperty(PropertyName = "first")]
        public Link First { get; set; }

        [JsonProperty(PropertyName = "prev")]
        public Link Previous { get; set; }

        [JsonProperty(PropertyName = "self")]
        public Link Self { get; set; }

        [JsonProperty(PropertyName = "next")]
        public Link Next { get; set; }

        [JsonProperty(PropertyName = "last")]
        public Link Last { get; set; }
    }
}