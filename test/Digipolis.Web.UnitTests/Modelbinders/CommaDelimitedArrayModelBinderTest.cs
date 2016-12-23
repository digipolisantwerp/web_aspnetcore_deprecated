using Digipolis.Web.Modelbinders;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Modelbinders
{
    public class CommaDelimitedArrayModelBinderTest 
    {
        [Fact]
        public void ShouldParseArrayOfInt()
        {
            var binder = new CommaDelimitedArrayModelBinder();

            var result = binder.ParseArray("1,2,3", typeof(int[]));

            Assert.NotNull(result);
            Assert.True(result is int[]);

            var intArray = (int[])result;

            Assert.True(intArray.Count() == 3);
            Assert.Contains(intArray, (o) => o == 1);
            Assert.Contains(intArray, (o) => o == 2);
            Assert.Contains(intArray, (o) => o == 3);
        }

        [Fact]
        public void ShouldParseArrayOfString()
        {
            var binder = new CommaDelimitedArrayModelBinder();

            var result = binder.ParseArray("1,two,3", typeof(string[]));

            Assert.NotNull(result);
            Assert.True(result is string[]);

            var intArray = (string[])result;

            Assert.True(intArray.Count() == 3);
            Assert.Contains(intArray, (o) => o == "1");
            Assert.Contains(intArray, (o) => o == "two");
            Assert.Contains(intArray, (o) => o == "3");
        }
      
        [Fact]
        public void ShouldParseListOfString()
        {
            var binder = new CommaDelimitedArrayModelBinder();

            var result = binder.ParseArray("1,two,3", typeof(List<string>));

            Assert.NotNull(result);
            Assert.True(result is List<string>);

            var intArray = (List<string>)result;

            Assert.True(intArray.Count() == 3);
            Assert.Contains(intArray, (o) => o == "1");
            Assert.Contains(intArray, (o) => o == "two");
            Assert.Contains(intArray, (o) => o == "3");
        }

        [Fact]
        public void ShouldParseListOfDateTime()
        {
            var binder = new CommaDelimitedArrayModelBinder();

            var result = binder.ParseArray("2016-01-01,1981-10-26,2012-12-21", typeof(List<DateTime>));

            Assert.NotNull(result);
            Assert.True(result is List<DateTime>);

            var intArray = (List<DateTime>)result;

            Assert.True(intArray.Count() == 3);
        }

        [Fact]
        public void ShouldThrowNotSupportedExceptionIfNotArrayOrIEnumerable()
        {
            var binder = new CommaDelimitedArrayModelBinder();

            Assert.Throws(typeof(NotSupportedException),() => binder.ParseArray("coords(12.4,12.2),454564.454,C#", typeof(ulong)));
        }

        [Fact]
        public void ShouldThrowNotSupportedExceptionIfArrayOfReferenceType()
        {
            var binder = new CommaDelimitedArrayModelBinder();

            Assert.Throws(typeof(NotSupportedException), () => binder.ParseArray("coords(12.4,12.2),454564.454,C#", typeof(List<object>)));
        }

    }
}
