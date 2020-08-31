using System.Xml.Serialization;

namespace Core.Contexts
{
    [XmlType(TypeName = "CoreMetadataSaveFile")]
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
        public string TCPPort;
        public MetadataRecordContext[] Commands;
        public MetadataRecordContext[] Telemetry;

        private MetadataServerContext()
        { }

        public MetadataServerContext(string name, string address, string tcpPort)
            : this()
        {
            Name = name;
            Address = address;
            TCPPort = tcpPort;
            Commands = new MetadataRecordContext[0];
            Telemetry = new MetadataRecordContext[0];
        }

        public MetadataServerContext(string name, string address, string tcpPort, MetadataRecordContext[] commands, MetadataRecordContext[] telemetry)
            : this()
        {
            Name = name;
            Address = address;
            TCPPort = tcpPort;
            Commands = commands;
            Telemetry = telemetry;
        }
    }
}