using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RED.JSON
{
    public static class JSONDeserializer
    {
        public static async Task<T> Deserialize<T>(string value)
        {
            return await JsonConvert.DeserializeObjectAsync<T>(value);
        }
    }
}