using RED.Contexts;
using System.Collections.Generic;

namespace RED.ViewModels.ControlCenter
{
    public class MetadataManager
    {
        public List<CommandMetadataContext> Commands { get; private set; }
        public List<TelemetryMetadataContext> Telemetry { get; private set; }
        public List<ErrorMetadataContext> Error { get; private set; }

        public MetadataManager()
        {
            Commands = new List<CommandMetadataContext>();
            Telemetry = new List<TelemetryMetadataContext>();
            Error = new List<ErrorMetadataContext>();
        }

        public void Add(CommandMetadataContext metadata)
        {
            Commands.Add(metadata);
        }
        public void Add(TelemetryMetadataContext metadata)
        {
            Telemetry.Add(metadata);
        }
        public void Add(ErrorMetadataContext metadata)
        {
            Error.Add(metadata);
        }
    }
}