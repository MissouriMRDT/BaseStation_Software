using RED.Contexts.Modules;

namespace RED.Configurations.Modules
{
    internal static class DriveConfig
    {
        internal static DriveSettingsContext DefaultConfig = new DriveSettingsContext()
        {
            SpeedLimit = 500,
            UseLegacyDataIds = false
        };
    }
}
