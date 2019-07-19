using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digipolis.Web.Api
{

    /// <summary>
    /// Using this PagedResult, serialization will generate a json string with a property named 'resourceList' on the _embedded HAL object,
    /// provided for backwards compatibility.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> where T : class
    {
        [JsonProperty(PropertyName = "_links")]
        public PagedResultLinks Links { get; set; }

        [JsonProperty("_embedded")]
        public Embedded<T> Embedded { get; set; }

        [JsonProperty(PropertyName = "_page")]
        public Page Page { get; set; }

        public PagedResult()
        {

        }

        public PagedResult(int page, int pageSize, int totalElements, IEnumerable<T> data)
        {
            Links = new PagedResultLinks();
            Embedded = new Embedded<T>(data ?? new List<T>());
            Page = new Page
            {
                Number = page,
                Size = data.Count(),
                TotalElements = totalElements,
                TotalPages = (int)Math.Ceiling((double)totalElements / (double)pageSize)
            };
        }
    }

    /// <summary>
    /// Using this PagedResult, serialization will generate a json string where the _embedded HAL object an object will have a property with a name 
    /// that can be set with the JsonPropertyAttribute on the overridden ResourceList property of the inherited <see cref="Embedded{T}"/> that used as EmbeddedT.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="EmbeddedT">The type of the mbedded t.</typeparam>
    public class PagedResult<T, EmbeddedT> : PagedResult<T>
        where T : class
        where EmbeddedT : Embedded<T>, new()
    {
        public PagedResult(int page, int pageSize, int? totalElements, IEnumerable<T> data)
        {
            Links = new PagedResultLinks();
            Embedded = new EmbeddedT { ResourceList = data ?? new List<T>() };
            Page = new Page
            {
                Number = page,
                Size = data?.Count() ?? 0,
                TotalElements = totalElements,
                TotalPages = (int)Math.Ceiling((double)totalElements / (double)pageSize)
            };
        }

        [JsonProperty("_embedded")]
        public new EmbeddedT Embedded { get; set; }
    }


    /// <summary>
    /// Inherit from this class, providing a type for T, to use as EmbeddedT for <see cref="PagedResult{T, EmbeddedT}"/>,
    /// override the ResourceList property and set the json propertyName with the JsonPropertyAttribute on the ResourceList property in the inherited class.
    /// If this class is not inherited, use <see cref="PagedResult{T}"/> for backwards compatibility.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Embedded<T>
    {
        public Embedded() { }
        public Embedded(IEnumerable<T> data)
        {
            ResourceList = data;
        }
        [JsonProperty("resourceList")]
        public virtual IEnumerable<T> ResourceList { get; set; }
    }

}
