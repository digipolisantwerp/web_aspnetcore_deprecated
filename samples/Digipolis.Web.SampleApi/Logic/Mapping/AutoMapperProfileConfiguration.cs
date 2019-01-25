using AutoMapper;
using Digipolis.Web.Api;
using Digipolis.Web.SampleApi.Data.Entiteiten;
using Digipolis.Web.SampleApi.Models;
using System.Collections.Generic;

namespace Digipolis.Web.SampleApi.Logic.Mapping
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Value, ValueDto>().ReverseMap();
            CreateMap<ValueType, ValueTypeDto>().ReverseMap();
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
            //CreateMap(typeof(IEnumerable<>), typeof(PagedResult<>)).ForMember("Data", x => x.MapFrom(m => m));
        }
               
    }
}