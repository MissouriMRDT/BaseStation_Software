using RED.Contexts.Network;

namespace RED.Configurations.Network
{
    internal static class NetworkManagerConfig
    {
        internal static NetworkManagerSettingsContext DefaultConfig = new NetworkManagerSettingsContext()
        {
            EnableReliablePackets = false
        };
    }
}
