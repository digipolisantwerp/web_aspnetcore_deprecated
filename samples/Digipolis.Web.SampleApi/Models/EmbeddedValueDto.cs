using Digipolis.Web.Api;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digipolis.Web.SampleApi.Models
{
    public class EmbeddedValueDto : Embedded<ValueDto>
    {
        [JsonProperty("valueDtos")]
        public override IEnumerable<ValueDto> ResourceList { get; set; }
    }
}
