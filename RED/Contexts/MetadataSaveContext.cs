using System.Collections.Generic;

namespace RED.Contexts
{
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