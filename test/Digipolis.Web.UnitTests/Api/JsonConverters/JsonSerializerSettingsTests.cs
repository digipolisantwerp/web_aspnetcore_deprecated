using Digipolis.Web.Api.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.JsonConverters
{
    public class JsonSerializerSettingsTests
    {
        /// <summary>
        /// collection should be serialized with collection items
        /// </summary>
        [Fact]
        public void AssignedCollection_should_serialize()
        {
            DummyCollection collection = new DummyCollection();
            collection.Names = new List<string>() { "name1", "name2" };

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(collection, Formatting.None, serializeSettings);

            Assert.Equal(@"{""names"":[""name1"",""name2""]}", json);

            // deserialize
            DummyCollection deserializedCollection = JsonConvert.DeserializeObject<DummyCollection>(json, serializeSettings);

            Assert.Equal(deserializedCollection.Names.Count, collection.Names.Count);
            Assert.Equal(deserializedCollection.Names[0], collection.Names[0]);
        }

        /// <summary>
        /// collection should be serialized as empty json-array
        /// </summary>
        [Fact]
        public void EmptyCollection_should_serialize()
        {
            DummyCollection collection = new DummyCollection();
            collection.Names = new List<string>() {};

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(collection, Formatting.None, serializeSettings);

            Assert.Equal(@"{""names"":[]}", json);

            // deserialize
            DummyCollection deserializedCollection = JsonConvert.DeserializeObject<DummyCollection>(json, serializeSettings);

            Assert.Equal(deserializedCollection.Names.Count, collection.Names.Count);
        }

        /// <summary>
        /// collection shouldn't be serialized
        /// </summary>
        [Fact]
        public void NullCollection_should_not_serialize()
        {
            DummyCollection collection = new DummyCollection();
            collection.Names = null;

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(collection, Formatting.None, serializeSettings);

            Assert.Equal(@"{}", json);
            
            // deserialize
            DummyCollection deserializedCollection = JsonConvert.DeserializeObject<DummyCollection>(json, serializeSettings);

            Assert.Null(deserializedCollection.Names);            
        }

        /// <summary>
        /// empty array should be serialized as empty json-array; null string-property shouldn't be serialized
        /// </summary>
        [Fact]
        public void EmptyArray_should_serialize()
        {
            DummyArray collection = new DummyArray();
            collection.Names = new string[0];

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(collection, Formatting.None, serializeSettings);

            Assert.Equal(@"{""names"":[]}", json);

            // deserialize
            DummyArray deserializedArray = JsonConvert.DeserializeObject<DummyArray>(json, serializeSettings);

            Assert.Equal(deserializedArray.Names.Length, collection.Names.Length);
        }

        /// <summary>
        /// null array shouldn't be serialized
        /// </summary>
        [Fact]
        public void NullArray_should_not_serialize()
        {
            DummyArray collection = new DummyArray();
            collection.Names = null;

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(collection, Formatting.None, serializeSettings);

            Assert.Equal(@"{}", json);

            // deserialize
            DummyArray deserializedArray = JsonConvert.DeserializeObject<DummyArray>(json, serializeSettings);

            Assert.Null(deserializedArray.Names);
        }

        /// <summary>
        /// null string-property shouldn't be serialized
        /// </summary>
        [Fact]
        public void NullString_should_not_serialize()
        {
            DummyString dummyObject = new DummyString();
            dummyObject.Prop1 = null;

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(dummyObject, Formatting.None, serializeSettings);

            Assert.Equal(@"{}", json);
        }

        /// <summary>
        /// assigned string-property should be serialized
        /// </summary>
        [Fact]
        public void AssignedString_should_serialize()
        {
            DummyString collection = new DummyString();
            collection.Prop1 = "prop 1";

            // serialize
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.Initialize();
            string json = JsonConvert.SerializeObject(collection, Formatting.None, serializeSettings);

            Assert.Equal(@"{""prop1"":""prop 1""}", json);
        }

        private class DummyCollection
        {
            public List<string> Names { get; set; }
        }

        private class DummyArray
        {
            public string[] Names { get; set; }
        }

        private class DummyString
        {
            public string Prop1 { get; set; }
        }
    }
}
