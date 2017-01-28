using Caliburn.Micro;
using RED.Models;
using RED.ViewModels.Input;
using RED.ViewModels.Modules;
using RED.ViewModels.Network;

namespace RED.ViewModels
{
    public class ControlCenterViewModel : Screen
    {
        private readonly ControlCenterModel _model;

        public SettingsManagerViewModel SettingsManager
        {
            get
            {
                return _model._settingsManager;
            }
            set
            {
                _model._settingsManager = value;
                NotifyOfPropertyChange(() => SettingsManager);
            }
        }

        public StateViewModel StateManager
        {
            get
            {
                return _model._stateManager;
            }
            set
            {
                _model._stateManager = value;
                NotifyOfPropertyChange();
            }
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
        public DataRouter DataRouter
        {
            get
            {
                return _model._dataRouter;
            }
            set
            {
                _model._dataRouter = value;
                NotifyOfPropertyChange(() => DataRouter);
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
        public SubscriptionManagerViewModel SubscriptionManager
        {
            get
            {
                return _model._subscriptionManager;
            }
            set
            {
                _model._subscriptionManager = value;
                NotifyOfPropertyChange(() => SubscriptionManager);
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
        public ScienceViewModel Science
        {
            get
            {
                return _model._science;
            }
            set
            {
                _model._science = value;
                NotifyOfPropertyChange(() => Science);
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
        public SensorViewModel Sensor
        {
            get
            {
                return _model._sensor;
            }
            set
            {
                _model._sensor = value;
                NotifyOfPropertyChange(() => Sensor);
            }
        }
        public DropBaysViewModel DropBays
        {
            get
            {
                return _model._dropBays;
            }
            set
            {
                _model._dropBays = value;
                NotifyOfPropertyChange(() => DropBays);

            }
        }
        public PowerViewModel Power
        {
            get
            {
                return _model._power;
            }
            set
            {
                _model._power = value;
                NotifyOfPropertyChange(() => Power);
            }
        }
        public CameraMuxViewModel CameraMux
        {
            get
            {
                return _model._cameraMux;
            }
            set
            {
                _model._cameraMux = value;
                NotifyOfPropertyChange(() => CameraMux);
            }
        }
        public ExternalControlsViewModel ExternalControls
        {
            get
            {
                return _model._externalControls;
            }
            set
            {
                _model._externalControls = value;
                NotifyOfPropertyChange(() => ExternalControls);
            }
        }
        public AutonomyViewModel Autonomy
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

        public DriveControllerModeViewModel DriveControllerMode
        {
            get
            {
                return (DriveControllerModeViewModel)InputManager.Input.ControllerModes[0];
            }
        }
        public ArmControllerModeViewModel ArmControllerMode
        {
            get
            {
                return (ArmControllerModeViewModel)InputManager.Input.ControllerModes[1];
            }
        }
        public GimbalControllerModeViewModel GimbalControllerMode
        {
            get
            {
                return (GimbalControllerModeViewModel)InputManager.Input.ControllerModes[2];
            }
        }
        public GimbalControllerModeViewModel Gimbal2ControllerMode
        {
            get
            {
                return (GimbalControllerModeViewModel)InputManager.Input.ControllerModes[3];
            }
        }

        public ControlCenterViewModel()
        {
            base.DisplayName = "Rover Engagement Display";
            _model = new ControlCenterModel();

            Console = new ConsoleViewModel();
            DataRouter = new DataRouter();
            MetadataManager = new MetadataManager(Console);
            MetadataManager.AddFromFile("NoSyncMetadata.xml");

            NetworkManager = new NetworkManagerViewModel(DataRouter, MetadataManager.Commands.ToArray(), Console, MetadataManager);
            SubscriptionManager = new SubscriptionManagerViewModel(MetadataManager.Telemetry.ToArray(), MetadataManager, NetworkManager);
            StateManager = new StateViewModel(SubscriptionManager);
            InputManager = new InputManagerViewModel(DataRouter, MetadataManager, Console, StateManager);

            Science = new ScienceViewModel(DataRouter, MetadataManager, Console);
            GPS = new GPSViewModel(DataRouter, MetadataManager);
            Sensor = new SensorViewModel(DataRouter, MetadataManager, Console);
            DropBays = new DropBaysViewModel(DataRouter, MetadataManager);
            Power = new PowerViewModel(DataRouter, MetadataManager, Console);
            CameraMux = new CameraMuxViewModel(DataRouter, MetadataManager);
            ExternalControls = new ExternalControlsViewModel(DataRouter, MetadataManager);
            Autonomy = new AutonomyViewModel(DataRouter, MetadataManager, Console);

            SettingsManager = new SettingsManagerViewModel(this);

            InputManager.Start();
            //DataRouter.Send(100, new byte[] { 10, 20, 30, 40 });
            //DataRouter.Send(1, new byte[] { 2, 3, 4, 5 });
            //DataRouter.Send(101, new byte[] { 15, 25, 35, 45 });
            //DataRouter.Send(180, new byte[] { 0x23, 0x52, 0x4f, 0x56, 0x45, 0x53, 0x4f, 0x48, 0x41, 0x52, 0x44, 0x00 });
        }
    }
}
