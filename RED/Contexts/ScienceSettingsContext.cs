using System.Net;

namespace RED.Contexts
{
    public class ScienceSettingsContext : ConfigurationFile
    {
        public IPAddress SpectrometerIPAddress;
        public ushort SpectrometerPortNumber;
        public string SpectrometerFilePath;
    }
}
