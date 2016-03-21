using RED.Interfaces;
using RED.JSON.Contexts;
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
        public ErrorMetadataContext(JsonErrorMetadataContext json)
        {
            Id = json.Error_ID;
            Name = json.Name;
            Subsystem = json.Subsystem;
            Description = json.Error_String;
            Datatype = json.Param_Type;
            Units = json.Param_Units;
        }

        public JsonErrorMetadataContext ToJsonContext()
        {
            return new JsonErrorMetadataContext()
            {
                Error_ID = (byte)Id,
                Name = Name,
                Subsystem = Subsystem,
                Error_String = Description,
                Param_Type = Datatype,
                Param_Units = Units
            };
        }
    }
}
