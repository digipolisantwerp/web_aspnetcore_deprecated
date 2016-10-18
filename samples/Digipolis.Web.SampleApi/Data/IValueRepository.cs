using System.Collections.Generic;
using Digipolis.Web.Api;
using Digipolis.Web.SampleApi.Data.Entiteiten;

namespace Digipolis.Web.SampleApi.Data
{
    public interface IValueRepository
    {
        IEnumerable<Value> GetAll(PageOptions queryOptions, out int total);
        Value GetById(int id);
        Value Add(Value value);
        Value Update(int id, Value value);
        void Delete(int id);
    }
}
