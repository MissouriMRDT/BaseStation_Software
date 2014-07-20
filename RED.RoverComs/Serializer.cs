namespace RED.RoverComs
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Serializer
    {
        public static string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new OrderedContractResolver()
            };
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static int GetDataTypeId(string json)
        {
            var unknownDataType = new { Id = 0 };
            return JsonConvert.DeserializeAnonymousType(json, unknownDataType).Id;
        }

        public static IProtocol<T> GetProtocol<T>(string json)
        {
            return JsonConvert.DeserializeObject<Protocol<T>>(json);
        }

        public static IEnumerable<int> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>();
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static bool ValidateMessage(string json)
        {
            try
            {
                var tempObject = new { };
                JsonConvert.DeserializeAnonymousType(json, tempObject);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        internal class OrderedContractResolver : DefaultContractResolver
        {
            // Ensures that serialization always puts the Id property at the top of the serialization output string.
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var properties = base.CreateProperties(type, memberSerialization).ToList();
                var idIndex = properties.FindIndex(x => x.PropertyName == "Id");
                var id = properties[idIndex];
                properties[idIndex] = properties[0];
                properties[0] = id;
                return properties;
            }
        }
    }
}