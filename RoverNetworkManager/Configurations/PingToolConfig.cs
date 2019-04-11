namespace RoverNetworkManager.Contexts
{
    public static class PingToolConfig
    {
        public static PingToolContext DefaultPingToolConfig = new PingToolContext(1000, 1000, new[] {
            new PingServerContext("Base Station Switch", "192.168.1.80", true, false),
            new PingServerContext("Base Station 900Mhz Rocket", "192.168.1.83", true, false),
            new PingServerContext("Rover 900Mhz Rocket", "192.168.1.82", true, false),
            new PingServerContext("Base Station 5Ghz Rocket", "192.168.1.85", true, false),
            new PingServerContext("Rover 5Ghz Rocket", "192.168.1.84", true, false),
            new PingServerContext("Rover 2.4Mhz Rocket", "192.168.1.86", true, false),
            new PingServerContext("Drive Board", "192.168.1.134", true, true),
            new PingServerContext("Arm Board", "192.168.1.131", true, true),
            new PingServerContext("Power Board", "192.168.1.132", true, true),
            new PingServerContext("Nav Board", "192.168.1.136", true, true),
            new PingServerContext("Gimbal Board", "192.168.1.135", true, true),
			new PingServerContext("Actuation Science Board", "192.168.1.137", true, true),
			new PingServerContext("Sensor Science Board", "192.168.1.138", true, true),
            new PingServerContext("Autonomy Board", "192.168.1.139", true, true),
            new PingServerContext("Camera Board", "192.168.1.136", true, true),
            new PingServerContext("Grandstream", "192.168.1.226", true, false)
        });
    }
}
