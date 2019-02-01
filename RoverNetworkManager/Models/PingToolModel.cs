using RoverNetworkManager.ViewModels;
using System.Collections.ObjectModel;
using System.Net;

namespace RoverNetworkManager.Models
{
    internal class PingToolModel
    {
        internal int AutoRate;
        internal int Timeout;
        internal ObservableCollection<PingToolViewModel.PingServer> PingServers;

        internal class PingServerModel
        {
            internal string Name;
            internal IPAddress Address;
            internal bool SupportsICMP;
            internal bool SupportsRoveComm;
            internal long Result;
            internal bool IsSendingICMP;
            internal bool IsSendingRoveComm;
            internal bool AutoModeEnabled;
        }

    }
}
