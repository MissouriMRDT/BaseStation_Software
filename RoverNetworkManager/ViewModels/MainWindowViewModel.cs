using Caliburn.Micro;
using RED.Roveprotocol;
using RED.ViewModels;
using RED.ViewModels.Network;
using RoverNetworkManager.Models;

namespace RoverNetworkManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

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


            ConsoleViewModel Console = new ConsoleViewModel();
            XMLConfigManager ConfigManager = new XMLConfigManager(Console);
            MetadataManager MetadataManager = new MetadataManager(Console, ConfigManager);
            NetworkManagerViewModel NetworkManager = new NetworkManagerViewModel(Console);

            Rovecomm = new Rovecomm(NetworkManager, Console, MetadataManager);

            RoveCommCustomPacket = new RoveCommCustomPacketViewModel(Rovecomm, ConfigManager);
            PingTool = new PingToolViewModel(Rovecomm, ConfigManager);
            NetworkMap = new NetworkMapViewModel(PingTool);

        }
    }
}
