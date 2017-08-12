using System.Net;

namespace RED.Contexts
{
    public class ScienceSettingsContext : ConfigurationFile
    {
        public string SpectrometerIPAddress;
        public ushort SpectrometerPortNumber;
        public string SpectrometerFilePath;
    }
}
