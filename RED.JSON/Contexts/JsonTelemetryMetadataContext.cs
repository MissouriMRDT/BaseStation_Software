namespace RED.JSON.Contexts
{
    public class JsonTelemetryMetadataContext
    {
        public byte Telem_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string Units { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
    }
}