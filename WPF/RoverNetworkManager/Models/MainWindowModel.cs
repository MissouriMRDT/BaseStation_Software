using Core.RoveProtocol;
using RoverOverviewNetwork.ViewModels;

namespace RoverOverviewNetwork.Models
{
    internal class MainWindowModel
    {
        public RoveCommCustomPacketViewModel _customPacket;
        public NetworkMapViewModel _networkMap;
        public PingToolViewModel _pingTool;
        public Rovecomm _rovecomm;
    }
}
