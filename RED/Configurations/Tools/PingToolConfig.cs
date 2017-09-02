using RED.Contexts.Tools;

namespace RED.Configurations.Tools
{
    internal static class PingToolConfig
    {
        internal static PingToolContext DefaultPingToolConfig = new PingToolContext(1000, 1000, new[] {
            new PingServerContext("Base Rocket", "192.168.1.83", true, false),
            new PingServerContext("Rover Rocket", "192.168.1.82", true, false),
            new PingServerContext("Drive Board", "192.168.1.130", true, true),
            new PingServerContext("Arm Board", "192.168.1.131", true, true),
            new PingServerContext("Power Board", "192.168.1.132", true, true),
            new PingServerContext("Nav Board", "192.168.1.133", true, true),
            new PingServerContext("External Controls Board", "192.168.1.134", true, true),
            new PingServerContext("Science Board", "192.168.1.135", true, true),
            new PingServerContext("Autonomy Board", "192.168.1.138", true, true),
            new PingServerContext("Science Arm Board", "192.168.1.139", true, true),
            new PingServerContext("Camera Board", "192.168.1.140", true, true),
            new PingServerContext("Grandstream", "192.168.1.226", true, false)
        });
    }
}
