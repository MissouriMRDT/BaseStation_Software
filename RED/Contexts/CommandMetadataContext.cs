using RED.Interfaces;
using RED.JSON.Contexts;

namespace RED.Contexts
{
    public class CommandMetadataContext : IMetadata
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }

        public CommandMetadataContext()
        {

        }
        public CommandMetadataContext(JsonCommandMetadataContext json)
        {
            Id = json.Cmd_ID;
            Name = json.Name;
            Description = json.Description;
            Datatype = json.Datatype;
        }

        public JsonCommandMetadataContext ToJsonContext()
        {
            return new JsonCommandMetadataContext()
            {
                Cmd_ID = Id,
                Name = Name,
                Datatype = Datatype,
                Description = Description
            };
        }
    }
}