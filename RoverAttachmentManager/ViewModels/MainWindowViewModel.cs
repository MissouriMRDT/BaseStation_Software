using Caliburn.Micro;
using Core;
using Core.Configurations;
using Core.Interfaces;
using Core.Interfaces.Input;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
using RoverAttachmentManager.Models;
using RoverAttachmentManager.ViewModels.Arm;
using System;
using RED.Roveprotocol;
using RED.ViewModels;
using RED.ViewModels.Network;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using RoverAttachmentManager.ViewModels.Autonomy;

namespace RoverAttachmentManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

        public override void CanClose(Action<bool> callback)
        {
            callback(false);
        }

        public ArmViewModel Arm
        {
            get
            {
                return _model._arm;
            }
            set
            {
                _model._arm = value;
                NotifyOfPropertyChange(() => Arm);
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

        public CommonLog Console
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
        public InputManagerViewModel InputManager
        {
            get
            {
                return _model._input;
            }
            set
            {
                _model._input = value;
                NotifyOfPropertyChange(() => InputManager);
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
        public XboxControllerInputViewModel XboxController
        {
            get
            {
                return _model._xboxController;
            }
            set
            {
                _model._xboxController = value;
                NotifyOfPropertyChange(() => XboxController);
            }
        }
        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Attachment Manager";
            _model = new MainWindowModel();

            Console = CommonLog.Instance;
            ConfigManager = new XMLConfigManager(Console);
            MetadataManager = new MetadataManager(Console, ConfigManager);
            
            Rovecomm = Rovecomm.Instance;
            ResubscribeAll();

            Arm = new ArmViewModel(Rovecomm, MetadataManager, Console, ConfigManager);

            XboxController = new XboxControllerInputViewModel(1);

            // Programatic instanciation of InputManager view, vs static like everything else in a xaml 
            InputManager = new InputManagerViewModel(Console, ConfigManager,
                new IInputDevice[] { XboxController },
                new MappingViewModel[0],
                new IInputMode[] { Arm });


        }
        public void ResubscribeAll()
        {
            Rovecomm.SubscribeMyPCToAllDevices();
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

        public Autonomy.AutonomyViewModel Autonomy
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