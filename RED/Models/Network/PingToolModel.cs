using RED.ViewModels.Tools;
using System.Collections.ObjectModel;

namespace RED.Models.Network
{
    internal class PingToolModel
    {
        internal int AutoRate;
        internal int Timeout;
        internal ObservableCollection<PingToolViewModel.PingServer> PingServers;
    }
}
