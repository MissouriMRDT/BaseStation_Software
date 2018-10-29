using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverNetworkManager.Networking;
using RoverNetworkManager.ViewModels;

namespace RoverNetworkManager.Models
{
    internal class MainWindowModel
    {
        public RoveCommCustomPacketViewModel _customPacket;
        public NetworkMapViewModel _networkMap;
    }
}
