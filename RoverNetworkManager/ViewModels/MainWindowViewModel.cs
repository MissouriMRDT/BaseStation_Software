using Caliburn.Micro;
using Core.Roveprotocol;
using Core.Network;
using RoverNetworkManager.Models;
using Core.Configurations;
using System;

namespace RoverNetworkManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

		public override void CanClose(Action<bool> callback) {
			callback(false);
		}

		public RoveCommCustomPacketViewModel RoveCommCustomPacket
        {
            get
            {
                return _model._customPacket;
            }
            set
            {
                _model._customPacket = value;
                NotifyOfPropertyChange(() => RoveCommCustomPacket);
            }
        }

        public NetworkMapViewModel NetworkMap
        {
            get
            {
                return _model._networkMap;
            }
            set
            {
                _model._networkMap = value;
                NotifyOfPropertyChange(() => NetworkMap);
            }
        }

        public PingToolViewModel PingTool
        {
            get
            {
                return _model._pingTool;
            }
            set
            {
                _model._pingTool = value;
                NotifyOfPropertyChange(() => PingTool);
            }
        }

        public Rovecomm Rovecomm
        {
            get
            {
                return _model._rovecomm;
            }
            set
            {
                _model._rovecomm = value;
                NotifyOfPropertyChange((() => Rovecomm));
            }
        }

        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Network Manager";
            _model = new MainWindowModel();

            XMLConfigManager ConfigManager = new XMLConfigManager(Core.CommonLog.Instance);
            MetadataManager MetadataManager = new MetadataManager(Core.CommonLog.Instance, ConfigManager);

            Rovecomm = Rovecomm.Instance;

            RoveCommCustomPacket = new RoveCommCustomPacketViewModel(Rovecomm, ConfigManager);
            PingTool = new PingToolViewModel(Rovecomm, ConfigManager);
            NetworkMap = new NetworkMapViewModel(PingTool);

        }
    }
}
