using System.Collections.Generic;

namespace RED.Contexts
{
    public class MetadataSaveContext
    {
        public CommandMetadataContext[] Commands;
        public TelemetryMetadataContext[] Telemetry;
        public ErrorMetadataContext[] Errors;

        public MetadataSaveContext(CommandMetadataContext[] commands, TelemetryMetadataContext[] telemetry, ErrorMetadataContext[] errors)
        {
            Commands = commands;
            Telemetry = telemetry;
            Errors = errors;
        }
    }
}