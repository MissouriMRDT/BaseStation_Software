using RED.Contexts;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RED.ViewModels.ControlCenter
{
    public class MetadataManager
    {
        private readonly ControlCenterViewModel _controlCenter;

        public List<CommandMetadataContext> Commands { get; private set; }
        public List<TelemetryMetadataContext> Telemetry { get; private set; }
        public List<ErrorMetadataContext> Errors { get; private set; }

        public MetadataManager(ControlCenterViewModel cc)
        {
            _controlCenter = cc;
            Commands = new List<CommandMetadataContext>();
            Telemetry = new List<TelemetryMetadataContext>();
            Errors = new List<ErrorMetadataContext>();
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
            Errors.Add(metadata);
        }

        public void AddFromFile(string url)
        {
            using (var stream = File.OpenRead(url))
            {
                var serializer = new XmlSerializer(typeof(MetadataSaveContext));
                MetadataSaveContext save = (MetadataSaveContext)serializer.Deserialize(stream);

                Commands.AddRange(save.Commands);
                Telemetry.AddRange(save.Telemetry);
                Errors.AddRange(save.Errors);

                _controlCenter.Console.WriteToConsole("Metadata loaded from file \"" + url + "\"");
            }
        }
        public void SaveToFile(string url)
        {
            using (var stream = new FileStream(url, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(MetadataSaveContext));
                serializer.Serialize(stream, new MetadataSaveContext(Commands.ToArray(), Telemetry.ToArray(), Errors.ToArray()));
            }
        }

        public CommandMetadataContext GetCommand(byte DataId)
        {
            return Commands.Find(x => x.Id == DataId);
        }
        public CommandMetadataContext GetCommand(string name)
        {
            return Commands.Find(x => x.Name == name);
        }
        public TelemetryMetadataContext GetTelemetry(byte DataId)
        {
            return Telemetry.Find(x => x.Id == DataId);
        }
        public TelemetryMetadataContext GetTelemetry(string name)
        {
            return Telemetry.Find(x => x.Name == name);
        }
        public ErrorMetadataContext GetError(byte DataId)
        {
            return Errors.Find(x => x.Id == DataId);
        }
        public ErrorMetadataContext GetError(string name)
        {
            return Errors.Find(x => x.Name == name);
        }
        public IMetadata GetMetadata(byte DataId)
        {
            return GetCommand(DataId) ?? GetTelemetry(DataId) ?? (IMetadata)GetError(DataId) ?? null;
        }
        public IMetadata GetMetadata(string name)
        {
            return GetCommand(name) ?? GetTelemetry(name) ?? (IMetadata)GetError(name) ?? null;
        }

        public int GetByteLength(string DataType)
        {
            switch (DataType.ToLowerInvariant())
            {
                case "int8": return 1;
                case "int16": return 2;
                case "int32": return 4;
                case "int64": return 8;
                case "uint8": return 1;
                case "uint16": return 2;
                case "uint32": return 4;
                case "uint64": return 8;
                case "single": return 4;
                case "double": return 8;
                case "ccd": return 25;
                case "gps": return 23;
                case "drillactuator": return 4;
                case "ultrasonic": return 2;
                case "sensorcombine": return 32;
                default: throw new ArgumentException("Unsupported Data Type");
            }
        }
        public int GetDataTypeByteLength(byte DataId)
        {
            var m = GetMetadata(DataId);
            if (m == null) throw new ArgumentException("Invalid DataId");
            return GetByteLength(m.Datatype);
        }
        public int GetDataTypeByteLength(IMetadata item)
        {
            return GetByteLength(item.Datatype);
        }
        public int GetDataTypeByteLength(string name)
        {
            return GetByteLength(GetMetadata(name).Datatype);
        }

        public byte GetId(string name)
        {
            var data = GetMetadata(name);
            return data == null ? (byte)0 : data.Id;
        }
        public string GetName(byte DataId)
        {
            var data = GetMetadata(DataId);
            return data == null ? String.Empty : data.Name;
        }
    }
}