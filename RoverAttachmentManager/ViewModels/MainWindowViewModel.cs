using Caliburn.Micro;
using RoverAttachmentManager.Models;
using RED.Roveprotocol;
using RED.ViewModels;
using RED.ViewModels.Network;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using RoverAttachmentManager.ViewModels.Autonomy;
using AutonomyViewModel = RoverAttachmentManager.ViewModels.Autonomy.AutonomyViewModel;

namespace RoverAttachmentManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Attachment Manager";
            _model = new MainWindowModel();

            Console = new ConsoleViewModel();
            NetworkManager = new NetworkManagerViewModel(Console);
            ConfigManager = new XMLConfigManager(Console);
            MetadataManager = new MetadataManager(Console, ConfigManager);
            Rovecomm = new Rovecomm(NetworkManager, Console, MetadataManager);
            Map = new MapViewModel();
            GPS = new GPSViewModel(Rovecomm, MetadataManager);
            WaypointManager = new WaypointManagerViewModel(Map, GPS);

            Autonomy = new AutonomyViewModel(Rovecomm, MetadataManager, Console, WaypointManager);

            
        }

        public ConsoleViewModel Console
        {
            get
            {
                return _model._console;
            }
            set
            {
                _model._console = value;
                NotifyOfPropertyChange();
            }
        }

        public NetworkManagerViewModel NetworkManager
        {
            get
            {
                return _model._networkManager;
            }
            set
            {
                _model._networkManager = value;
                NotifyOfPropertyChange(() => NetworkManager);
            }
        }

        public XMLConfigManager ConfigManager
        {
            get
            {
                return _model._configManager;
            }
            set
            {
                _model._configManager = value;
                NotifyOfPropertyChange();
            }
        }

        public MetadataManager MetadataManager
        {
            get
            {
                return _model._metadataManager;
            }
            set
            {
                _model._metadataManager = value;
                NotifyOfPropertyChange(() => MetadataManager);
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

        public GPSViewModel GPS
        {
            get
            {
                return _model._GPS;
            }
            set
            {
                _model._GPS = value;
                NotifyOfPropertyChange(() => GPS);
            }
        }

        public MapViewModel Map
        {
            get
            {
                return _model._map;
            }
            set
            {
                _model._map = value;
                NotifyOfPropertyChange(() => Map);
            }
        }

        public WaypointManagerViewModel WaypointManager
        {
            get
            {
                return _model._waypoint;
            }
            set
            {
                _model._waypoint = value;
                NotifyOfPropertyChange(() => WaypointManager);
            }
        }

        internal AutonomyViewModel Autonomy
        {
            get
            {
                return _model._autonomy;
            }
            set
            {
                _model._autonomy = value;
                NotifyOfPropertyChange(() => Autonomy);
            }
        }
    }
}