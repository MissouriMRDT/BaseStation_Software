using RED.Contexts;
using System;
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

        public int GetDataTypeByteLength(string DataType)
        {
            switch (DataType)
            {
                case "int8": return 1;
                case "int16": return 2;
                case "int32": return 4;
                case "int64": return 8;
                default: throw new ArgumentException("Unsupported Data Type");
            }
        }
    }
}