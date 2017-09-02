using RED.Contexts.Modules;

namespace RED.Configurations.Modules
{
    internal static class ScienceConfig
    {
        internal static ScienceSettingsContext DefaultConfig = new ScienceSettingsContext()
        {
            SpectrometerIPAddress = "192.168.1.135",
            SpectrometerPortNumber = 11001,
            SpectrometerFilePath = System.String.Empty
        };
    }
}
