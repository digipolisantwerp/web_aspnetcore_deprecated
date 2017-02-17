using Digipolis.Web.Modelbinders;
using Digipolis.Web.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Modelbinders
{
    public class CommaDelimitedArrayModelBinderProviderTest 
    {
        [Fact]
        public void ShouldSupportIEnumerableOfValueType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(List<int>)));
        }

        [Fact]
        public void ShouldSupportIEnumerableOfString()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(List<string>)));
        }

        [Fact]
        public void ShouldNotSupportIEnumerableOfReferenceType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.False(provider.TypeIsSupported(typeof(List<object>)));
        }

        [Fact]
        public void ShouldSupportArrayOfValueType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(DateTime[])));
        }

        [Fact]
        public void ShouldSupportArrayOfString()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(string[])));
        }

        [Fact]
        public void ShouldNotSupportArrayOfReferenceType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.False(provider.TypeIsSupported(typeof(object)));
        }

        [Fact]
        public void ShouldNotSupportString()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.False(provider.TypeIsSupported(typeof(string)));
        }

        [Theory]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(DateTime?))]
        public void ShouldNotSupportNullables(Type testtype)
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.False(provider.TypeIsSupported(testtype));
        }
    }
}
