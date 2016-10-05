using System;
using Digipolis.Web.Api.Filters;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Filters
{
    public class GlobalExceptionFilterTests
    {
        [Fact]
        public void CtorErrorNullParameters()
        {
            Assert.Throws<ArgumentNullException>(() => new GlobalExceptionFilter(null));
        }

        [Fact]
        public void ExceptionHandled()
        {

            Assert.Throws<ArgumentNullException>(() => new GlobalExceptionFilter(null));
        }

        //TODO: Implement when ExceptionHandler Tests are complete
    }
}
