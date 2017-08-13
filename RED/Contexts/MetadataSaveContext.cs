using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "REDMetadataSaveFile")]
    public class MetadataSaveContext : ConfigurationFile
    {
        public MetadataServerContext[] Servers;

        private MetadataSaveContext()
        { }

        public MetadataSaveContext(MetadataServerContext[] servers)
            : this()
        {
            Servers = servers;
        }
    }

    [XmlType(TypeName = "Server")]
    public class MetadataServerContext : ConfigurationFile
    {
        public string Name;
        public string Address;
        public MetadataRecordContext[] Commands;
        public MetadataRecordContext[] Telemetry;

        private MetadataServerContext()
        { }

        public MetadataServerContext(string name, string address)
            : this()
        {
            Name = name;
            Address = address;
            Commands = new MetadataRecordContext[0];
            Telemetry = new MetadataRecordContext[0];
        }

        public MetadataServerContext(string name, string address, MetadataRecordContext[] commands, MetadataRecordContext[] telemetry)
            : this()
        {
            Name = name;
            Address = address;
            Commands = commands;
            Telemetry = telemetry;
        }
    }
}