using System;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "REDMetadataSaveFile")]
    [Serializable]
    public class MetadataSaveContext
    {
        public CommandMetadataContext[] Commands;
        public TelemetryMetadataContext[] Telemetry;
        public ErrorMetadataContext[] Errors;

        private MetadataSaveContext() { } //Required for XML Deserialization

        public MetadataSaveContext(CommandMetadataContext[] commands, TelemetryMetadataContext[] telemetry, ErrorMetadataContext[] errors)
        {
            Commands = commands;
            Telemetry = telemetry;
            Errors = errors;
        }
    }
}