using Caliburn.Micro;
using RED.Models;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class NetworkManagerViewModel : PropertyChangedBase, ISubscribe
    {
        private NetworkManagerModel _model;
        private ControlCenterViewModel _cc;

        private INetworkEncoding encoding;

        public NetworkManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new NetworkManagerModel();
            _cc = cc;

            encoding = new RoverProtocol();

            foreach (var command in _cc.MetadataManager.Commands)
                _cc.DataRouter.Subscribe(this, command.Id);
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            //TODO: send data over network
        }
    }
}