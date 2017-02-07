using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Digipolis.Web.Api
{
    public class PagedResult<T> where T : class, new()
    {
        [JsonProperty(PropertyName = "_links")]
        public PagedResultLinks Links { get; set; }

        public IEnumerable<T> Data { get; set; }

        [JsonProperty(PropertyName = "_page")]
        public Page Page { get; set; }

        public PagedResult()
        {

        }

        public PagedResult(int page, int pageSize, int totalElements, IEnumerable<T> data)
        {
            Data = data ?? new List<T>();
            Page = new Page
            {
                Number = page,
                Size = data.Count(),
                TotalElements = totalElements,
                TotalPages = (int)Math.Ceiling((double)totalElements / (double)pageSize)
            };
            Links = new PagedResultLinks();
        }
    }

    internal class DeserializationPagedResult<T> : PagedResult<T> where T : class, new()
    {
        [JsonProperty(PropertyName = "_embedded")]
        public Embedded EmbeddedData
        {
            set
            {
                Data = value?.ResourceList;
            }
        }

        public DeserializationPagedResult()
        {

        }

        public DeserializationPagedResult(int page, int pageSize, int totalElements, IEnumerable<T> data)
            : base(page, pageSize, totalElements, data)
        {

        }

        public class Embedded
        {
            public IEnumerable<T> ResourceList { get; set; }
        }
    }
}
