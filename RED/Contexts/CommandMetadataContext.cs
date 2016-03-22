using RED.Interfaces;
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
    }
}