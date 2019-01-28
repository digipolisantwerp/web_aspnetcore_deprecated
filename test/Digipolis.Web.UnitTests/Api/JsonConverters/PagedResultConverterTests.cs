using Digipolis.Web.Api;
using Digipolis.Web.Api.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.JsonConverters
{
    public class PagedResultConverterTests
    {
        [Fact]
        public void CanBeDeserialized()
        {
            var settings = new JsonSerializerSettings();

            var original = new PagedResult<DummyType>(1, 10, 25, new List<DummyType> { new DummyType { Text = "someText" } });

            var serialized = JsonConvert.SerializeObject(original, settings);

            var deserialized = JsonConvert.DeserializeObject<PagedResult<DummyType>>(serialized, settings);

            var deserialized2 = JsonConvert.DeserializeObject<PagedResult<DummyType>>(serialized, settings);

            Assert.NotNull(deserialized);
            Assert.Equal(1, deserialized.Page.Number);
            Assert.Equal(1, deserialized.Page.Size);
            Assert.Equal(25, deserialized.Page.TotalElements);
            Assert.Equal(3, deserialized.Page.TotalPages);
            Assert.NotNull(deserialized.Embedded?.ResourceList);
            Assert.Equal("someText", deserialized.Embedded.ResourceList.First().Text);
        }

        public class DummyType
        {
            public string Text { get; set; }
        }
    }
}
