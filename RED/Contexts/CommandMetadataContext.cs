using RED.Interfaces;
using RED.JSON.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Contexts
{
    public class CommandMetadataContext : IMetadata
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }

        public JsonCommandMetadataContext ToJsonContext()
        {
            return new JsonCommandMetadataContext()
            {
                Cmd_ID = Id,
                Name = Name,
                Datatype = Datatype,
                Description = Description
            };
        }
    }
}