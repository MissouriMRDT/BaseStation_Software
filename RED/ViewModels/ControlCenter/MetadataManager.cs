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
        public List<CommandMetadataContext> Commands { get; private set; }
        public List<TelemetryMetadataContext> Telemetry { get; private set; }
        public List<ErrorMetadataContext> Errors { get; private set; }

        public MetadataManager()
        {
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
            switch (DataType)
            {
                case "int8": return 1;
                case "int16": return 2;
                case "int32": return 4;
                case "int64": return 8;
                default: throw new ArgumentException("Unsupported Data Type");
            }
        }
        public int GetDataTypeByteLength(byte DataId)
        {
            return GetByteLength(GetMetadata(DataId).Datatype);
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
            return data == null ? data.Id : (byte)0;
        }
        public string GetName(byte DataId)
        {
            var data = GetMetadata(DataId);
            return data == null ? data.Name : String.Empty;
        }
    }
}