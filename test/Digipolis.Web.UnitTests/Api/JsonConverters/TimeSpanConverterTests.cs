using Digipolis.Web.Api.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.JsonConverters
{
    public class TimeSpanConverterTests
    {
        [Fact]
        public void NullableTimeSpanWithNullValue()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TimeSpanConverter());

            var original = new DummyType { TimeSpan = null, Text = "someText" };

            var serialized = JsonConvert.SerializeObject(original, settings);

            var deserialized = JsonConvert.DeserializeObject<DummyType>(serialized, settings);

            Assert.NotNull(deserialized);
            Assert.Null(deserialized.TimeSpan);
        }

        [Fact]
        public void NullableTimeSpanWithValue()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TimeSpanConverter());

            var original = new DummyType { TimeSpan = TimeSpan.FromMinutes(1), Text = "someText" };

            var serialized = JsonConvert.SerializeObject(original, settings);

            var deserialized = JsonConvert.DeserializeObject<DummyType>(serialized, settings);

            Assert.NotNull(deserialized);
            Assert.Equal(deserialized.TimeSpan, TimeSpan.FromMinutes(1));
        }

        public class DummyType
        {
            public TimeSpan? TimeSpan { get; set; }
            public string Text { get; set; }
        }
    }
}
