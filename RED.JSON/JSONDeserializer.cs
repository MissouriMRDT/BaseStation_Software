using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RED.JSON
{
    public static class JSONDeserializer
    {
        public static async T Deserialize<T>(string value)
        {
            return await JsonConvert.DeserializeObjectAsync<T>(value);
        }
    }
}
