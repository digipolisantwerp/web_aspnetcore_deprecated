using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;

namespace Digipolis.Web.UnitTests._TestObjects
{
    public class ExceptionMapperTest : ExceptionMapper
    {
        protected override void Configure()
        {
            CreateMap<ArgumentNullException>(300);
            CreateMap<AggregateException>((e, ex) =>
            {
                e.Title = "Error";
                e.Status = 400;
            });
        }

        protected override void CreateDefaultMap(Error error, Exception exception)
        {
            error.Code = "GENERAL";
            error.Status = 500;
            error.Title = "Technical";
            error.Type = new Uri("https://www.google.be");
        }
    }
}
