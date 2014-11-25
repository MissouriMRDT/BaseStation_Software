using RED.Interfaces;
using RED.JSON.Contexts;

namespace RED.Contexts
{
    public class TelemetryMetadataContext : IMetadata
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string Units { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }

        public TelemetryMetadataContext()
        {

        }
        public TelemetryMetadataContext(JsonTelemetryMetadataContext json)
        {
            Id = json.Telem_ID;
            Name = json.Name;
            Description = json.Description;
            Datatype = json.Datatype;
            Units = json.Units;
            Minimum = json.Minimum;
            Maximum = json.Maximum;
        }

        public JsonTelemetryMetadataContext ToJsonContext()
        {
            return new JsonTelemetryMetadataContext()
            {
                Telem_ID = Id,
                Name = Name,
                Description = Description,
                Datatype = Datatype,
                Units = Units,
                Maximum = Maximum,
                Minimum = Minimum
            };
        }
    }
}
