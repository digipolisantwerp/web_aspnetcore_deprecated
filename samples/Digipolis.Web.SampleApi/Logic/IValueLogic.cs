using System.Collections.Generic;
using Digipolis.Web.Api;
using Digipolis.Web.SampleApi.Models;

namespace Digipolis.Web.SampleApi.Logic
{
    public interface IValueLogic
    {
        IEnumerable<ValueDto> GetAll(PageOptions queryOptions, out int total);
        ValueDto GetById(int id);
        ValueDto Add(ValueDto value);
        ValueDto Update(int id, ValueDto value);
        void Delete(int id);
    }
}