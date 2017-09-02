using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "Record")]
    public class MetadataRecordContext : ConfigurationFile
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public MetadataRecordContext()
        { }

        public MetadataRecordContext(ushort id, string name, string description)
            : this()
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}