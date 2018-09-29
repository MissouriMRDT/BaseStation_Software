using Caliburn.Micro;
using RoverNetworkManager.Models;
using RoverNetworkManager.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverNetworkManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

        public RoveCommCustomPacketViewModel CustomPacket
        {
            get
            {
                return _model._customPacket;
            }
            set
            {
                _model._customPacket = value;
                NotifyOfPropertyChange(() => CustomPacket);
            }
        }

        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Network Manager";
            _model = new MainWindowModel();

            CustomPacket = new RoveCommCustomPacketViewModel();
        }
    }
}
