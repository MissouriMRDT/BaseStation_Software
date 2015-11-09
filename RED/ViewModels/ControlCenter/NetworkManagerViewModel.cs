using Caliburn.Micro;
using RED.Models;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class NetworkManagerViewModel : PropertyChangedBase, ISubscribe
    {
        private const ushort DestinationPort = 11000;
        private readonly IPAddress TempDestinationIP = IPAddress.Parse("192.168.1.51");

        private NetworkManagerModel _model;
        private ControlCenterViewModel _cc;

        private INetworkEncoding encoding;
        private INetworkTransportProtocol continuousDataSocket;

        public NetworkManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new NetworkManagerModel();
            _cc = cc;

            encoding = new RoverProtocol();
            continuousDataSocket = new UDPEndpoint(DestinationPort,DestinationPort);

            foreach (var command in _cc.MetadataManager.Commands)
                _cc.DataRouter.Subscribe(this, command.Id);
        }

        public async void ReceiveFromRouter(byte dataId, byte[] data)
        {
            byte[] packet=encoding.EncodePacket(dataId,data);
            await continuousDataSocket.SendMessage(TempDestinationIP, packet);
        }
    }
}