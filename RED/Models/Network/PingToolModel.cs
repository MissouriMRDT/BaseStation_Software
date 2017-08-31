using RED.ViewModels.Tools;
using System.Collections.ObjectModel;

namespace RED.Models.Network
{
    public class PingToolModel
    {
        internal int AutoRate;
        internal int Timeout;
        internal ObservableCollection<PingToolViewModel.PingServer> PingServers;
    }
}
