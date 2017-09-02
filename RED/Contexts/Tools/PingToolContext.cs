using System.Xml.Serialization;

namespace RED.Contexts.Tools
{
    [XmlType(TypeName = "PingToolConfig")]
    public class PingToolContext : ConfigurationFile
    {
        public int AutoRate;
        public int Timeout;
        public PingServerContext[] Servers;

        private PingToolContext()
        { }

        public PingToolContext(int autoRate, int timeout, PingServerContext[] servers)
            : this()
        {
            AutoRate = autoRate;
            Timeout = timeout;
            Servers = servers;
        }
    }

    [XmlType(TypeName = "Server")]
    public class PingServerContext : ConfigurationFile
    {
        public string Name;
        public string Address;
        public bool SupportsICMP;
        public bool SupportsRoveComm;

        private PingServerContext()
        { }

        public PingServerContext(string name, string address, bool supportsICMP, bool supportsRoveComm)
            : this()
        {
            Name = name;
            Address = address;
            SupportsICMP = supportsICMP;
            SupportsRoveComm = supportsRoveComm;
        }
    }
}
