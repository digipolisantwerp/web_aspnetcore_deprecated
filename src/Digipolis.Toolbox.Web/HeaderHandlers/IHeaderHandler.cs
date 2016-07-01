using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Digipolis.Toolbox.Web.Headers
{
    public interface IHeaderHandler
    {
        Task Handle(StringValues values);
    }
}
