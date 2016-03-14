using RED.Interfaces;
using System;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "TelemetryMetadata")]
    [Serializable]
    public class TelemetryMetadataContext : IMetadata
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string ServerAddress { get; set; }
        public string Units { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }

        public TelemetryMetadataContext()
        {

        }
    }
}
