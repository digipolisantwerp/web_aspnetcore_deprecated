using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Api;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Filters
{
    public class ValidateModelStateAttributeTests
    {
        [Fact]
        public void OnActionExecutingValid()
        {
            var validAttr = new ValidateModelStateAttribute();
            var aec = MockHelpers.ActionExecutingContext();
            validAttr.OnActionExecuting(aec);
            Assert.True(true);
        }

        //[Fact]
        //public void OnActionExecutingInValid()
        //{
        //    var validAttr = new ValidateModelStateAttribute();
        //    var aec = MockHelpers.ActionExecutingContext(x =>
        //    {
        //        x.Object.ModelState.AddModelError("test", "");
        //    }
        //    );
        //    Assert.Throws<ValidationException>(()=> validAttr.OnActionExecuting(aec));
        //}

        [Fact]
        public async Task OnActionExecutingAsyncValid()
        {
            var validAttr = new ValidateModelStateAttribute();
            var aec = MockHelpers.ActionExecutingContext();
            await validAttr.OnActionExecutionAsync(aec, null);
            Assert.True(true);
        }

        //[Fact]
        //public async Task OnActionExecutingAsyncInValid()
        //{
        //    var validAttr = new ValidateModelStateAttribute();
        //    var aec = MockHelpers.ActionExecutingContext(x =>
        //    {
        //        x.Object.ModelState.AddModelError("test", new Exception(), new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), new DefaultMetadataDetails(ModelMetadataIdentity.ForProperty(), )));
        //    });
        //    await Assert.ThrowsAsync<ValidationException>(() => validAttr.OnActionExecutionAsync(aec, null));
        //}
    }
}
