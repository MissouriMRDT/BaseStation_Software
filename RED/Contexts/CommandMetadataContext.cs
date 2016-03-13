using RED.Interfaces;
using RED.JSON.Contexts;
using System;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "CommandMetadata")]
    [Serializable]
    public class CommandMetadataContext : IMetadata
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string ServerAddress { get; set; }

        public CommandMetadataContext()
        {

        }
        public CommandMetadataContext(JsonCommandMetadataContext json)
        {
            Id = json.Cmd_ID;
            Name = json.Name;
            Description = json.Description;
            Datatype = json.Datatype;
        }

        public JsonCommandMetadataContext ToJsonContext()
        {
            return new JsonCommandMetadataContext()
            {
                Cmd_ID = (byte)Id,
                Name = Name,
                Datatype = Datatype,
                Description = Description
            };
        }
    }
}