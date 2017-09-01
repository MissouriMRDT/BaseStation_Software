using RED.Contexts.Modules;

namespace RED.Configurations.Modules
{
    internal static class PowerConfig
    {
        internal static PowerSettingsContext DefaultConfig = new PowerSettingsContext()
        {
            AutoStartLog = false
        };
    }
}
