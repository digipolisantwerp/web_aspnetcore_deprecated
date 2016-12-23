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

            Assert.True(provider.TypeIsSupported(typeof(List<int>).GetTypeInfo()));
        }

        [Fact]
        public void ShouldSupportIEnumerableOfString()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(List<string>).GetTypeInfo()));
        }

        [Fact]
        public void ShouldNotSupportIEnumerableOfReferenceType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.False(provider.TypeIsSupported(typeof(List<object>).GetTypeInfo()));
        }

        [Fact]
        public void ShouldSupportArrayOfValueType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(DateTime[]).GetTypeInfo()));
        }

        [Fact]
        public void ShouldSupportArrayOfString()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.True(provider.TypeIsSupported(typeof(string[]).GetTypeInfo()));
        }

        [Fact]
        public void ShouldNotSupportArrayOfReferenceType()
        {
            var provider = new CommaDelimitedArrayModelBinderProvider();

            Assert.False(provider.TypeIsSupported(typeof(object).GetTypeInfo()));
        }
    }
}
