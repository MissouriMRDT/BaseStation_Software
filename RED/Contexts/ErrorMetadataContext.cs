using RED.Interfaces;
using System;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "ErrorMetadata")]
    [Serializable]
    public class ErrorMetadataContext : IMetadata
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Subsystem { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string ServerAddress { get; set; }
        public string Units { get; set; }

        public ErrorMetadataContext()
        {

        }
    }
}
