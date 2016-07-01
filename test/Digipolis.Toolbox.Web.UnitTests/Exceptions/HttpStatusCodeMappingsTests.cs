using System;
using System.Collections.Generic;
using Digipolis.Toolbox.Errors.Exceptions;
using Digipolis.Toolbox.Web.Exceptions;
using Xunit;

namespace Digipolis.Toolbox.Web.UnitTests.Exceptions
{
    public class HttpStatusCodeMappingsTests
    {
        [Fact]
        public void InitializeDefaults()
        {
            var mappings = new HttpStatusCodeMappings();

            Assert.Equal(404, mappings.GetStatusCode(typeof(NotFoundException)));
            Assert.Equal(400, mappings.GetStatusCode(typeof(ValidationException)));
            Assert.Equal(403, mappings.GetStatusCode(typeof(UnauthorizedException)));
        }

        [Fact]
        public void OverrideExisting()
        {
            var mappings = new HttpStatusCodeMappings();

            mappings.Add(typeof(NotFoundException), 500);

            Assert.Equal(500, mappings.GetStatusCode(typeof(NotFoundException)));
        }

        [Fact]
        public void OverrideExistingGeneric()
        {
            var mappings = new HttpStatusCodeMappings();

            mappings.Add<NotFoundException>(500);

            Assert.Equal(500, mappings.GetStatusCode(typeof(NotFoundException)));
        }

        [Fact]
        public void ContainsKeyReturnsTrue()
        {
            var mappings = new HttpStatusCodeMappings();

            Assert.True(mappings.ContainsKey(typeof(NotFoundException)));
        }

        [Fact]
        public void ContainsKeyReturnsFalse()
        {
            var mappings = new HttpStatusCodeMappings();

            Assert.False(mappings.ContainsKey(typeof(ArgumentNullException)));
        }

        [Fact]
        public void AddNewMapping()
        {
            var mappings = new HttpStatusCodeMappings();

            mappings.Add(typeof(ArgumentNullException), 400);

            Assert.Equal(400, mappings.GetStatusCode(typeof(ArgumentNullException)));
        }

        [Fact]
        public void AddNewMappingGeneric()
        {
            var mappings = new HttpStatusCodeMappings();

            mappings.Add<ArgumentNullException>(400);

            Assert.Equal(400, mappings.GetStatusCode(typeof(ArgumentNullException)));
        }

        [Fact]
        public void AddNewMappingsRange()
        {
            var newRange = new Dictionary<Type, int>()
            {
                { typeof(ArgumentNullException), 400 },
                { typeof(InvalidOperationException), 500 }
            };

            var mappings = new HttpStatusCodeMappings();

            mappings.AddRange(newRange);

            Assert.Equal(400, mappings.GetStatusCode(typeof(ArgumentNullException)));
            Assert.Equal(500, mappings.GetStatusCode(typeof(InvalidOperationException)));
        }

    }
}
