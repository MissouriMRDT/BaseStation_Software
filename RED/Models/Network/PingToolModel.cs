using System.Collections.ObjectModel;

namespace RED.Models.Network
{
    public class PingToolModel
    {
        internal int AutoRate;
        internal int Timeout;
        internal ObservableCollection<RED.ViewModels.Network.PingToolViewModel.PingServer> PingServers;
    }
}
