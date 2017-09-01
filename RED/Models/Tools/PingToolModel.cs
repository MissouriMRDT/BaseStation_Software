using RED.ViewModels.Tools;
using System.Collections.ObjectModel;
using System.Net;

namespace RED.Models.Tools
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
