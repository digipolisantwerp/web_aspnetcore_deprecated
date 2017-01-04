using System;
using System.Xml;
using Newtonsoft.Json;

namespace Digipolis.Web.Api.JsonConverters
{
    public class TimeSpanConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ts = (TimeSpan)value;
            var tsString = XmlConvert.ToString(ts);
            serializer.Serialize(writer, tsString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            TimeSpan result;
            var value = serializer.Deserialize<string>(reader);
            try
            {
                if (reader.TokenType == JsonToken.Null) return null;
                result = XmlConvert.ToTimeSpan(value);
            }
            catch (Exception)
            {
                if(!TimeSpan.TryParse(value, out result)) return null;
            }
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }
    }
}
