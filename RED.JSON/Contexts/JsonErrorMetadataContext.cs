namespace RED.JSON.Contexts
{
   public class JsonErrorMetadataContext
    {
        public byte Error_ID { get; set; }
        public string Name { get; set; }
        public string Subsystem { get; set; }
        public string Error_String { get; set; }
        public string Param_Type { get; set; }
        public string Param_Units { get; set; }
    }
}