//using System;
//using System.Linq;
//using Microsoft.AspNetCore.Mvc;
//using Xunit;
//using Digipolis.Errors;
//using Digipolis.Web.Api;
//using Digipolis.Web.Api.Filters;

//namespace Digipolis.Web.UnitTests.Filters
//{
//    public class ValidateModelStateAttributeTests
//    {
//        [Fact]
//        private void ValidModelStateDoesNothing()
//        {
//            var context = ActionFilterFactory.CreateActionExecutingContext();

//            var filter = new ValidateModelStateAttribute();
//            filter.OnActionExecuting(context);

//            Assert.Null(context.Result);
//        }

//        [Fact]
//        private void InvalidModelStateSetsBadRequestResult()
//        {
//            var context = ActionFilterFactory.CreateActionExecutingContext();
//            context.ModelState.AddModelError("key1", "error1");
            
//            var filter = new ValidateModelStateAttribute();
//            filter.OnActionExecuting(context);

//            Assert.NotNull(context.Result);
//            Assert.IsType<BadRequestObjectResult>(context.Result);
//            Assert.IsType<Error>(((BadRequestObjectResult)context.Result).Value);

//            var error = ((BadRequestObjectResult)context.Result).Value as Error;
//            var guid = Guid.Empty;
//            Assert.True(Guid.TryParse(error.Id, out guid));
//            Assert.Equal("key1", error.Messages.First().Key);
//            Assert.Equal("error1", error.Messages.First().Message);
//        }

//        [Fact]
//        private void InvalidModelStateSetsBadRequestResultForMultipleErrors()
//        {
//            var context = ActionFilterFactory.CreateActionExecutingContext();
//            context.ModelState.AddModelError("key1", "error1");
//            context.ModelState.AddModelError("key2", "error2");
//            context.ModelState.AddModelError("key3", "error3");

//            var filter = new ValidateModelStateAttribute();
//            filter.OnActionExecuting(context);

//            Assert.NotNull(context.Result);
//            Assert.IsType<BadRequestObjectResult>(context.Result);
//            Assert.IsType<Error>(((BadRequestObjectResult)context.Result).Value);

//            var error = ((BadRequestObjectResult)context.Result).Value as Error;
//            Assert.Equal(3, error.Messages.Count());
//            Assert.Equal("error1", error.Messages.Where(m => m.Key == "key1").Single().Message);
//            Assert.Equal("error2", error.Messages.Where(m => m.Key == "key2").Single().Message);
//            Assert.Equal("error3", error.Messages.Where(m => m.Key == "key3").Single().Message);
//        }
//    }
//}
