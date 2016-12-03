using Caliburn.Micro;
using RED.Interfaces.Input;
using RED.Models;
using RED.ViewModels.Input;
using RED.ViewModels.Input.Controllers;
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

        public DriveControllerModeViewModel DriveControllerMode
        {
            get
            {
                return _model._driveControllerMode;
            }
            set
            {
                _model._driveControllerMode = value;
                NotifyOfPropertyChange(() => DriveControllerMode);
            }
        }
        public ArmControllerModeViewModel ArmControllerMode
        {
            get
            {
                return _model._armControllerMode;
            }
            set
            {
                _model._armControllerMode = value;
                NotifyOfPropertyChange(() => ArmControllerMode);
            }
        }
        public GimbalControllerModeViewModel GimbalControllerMode
        {
            get
            {
                return _model._gimbal1ControllerMode;
            }
            set
            {
                _model._gimbal1ControllerMode = value;
                NotifyOfPropertyChange(() => GimbalControllerMode);
            }
        }
        public GimbalControllerModeViewModel Gimbal2ControllerMode
        {
            get
            {
                return _model._gimbal2ControllerMode;
            }
            set
            {
                _model._gimbal2ControllerMode = value;
                NotifyOfPropertyChange(() => Gimbal2ControllerMode);
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

            Science = new ScienceViewModel(DataRouter, MetadataManager, Console);
            GPS = new GPSViewModel(DataRouter, MetadataManager);
            Sensor = new SensorViewModel(DataRouter, MetadataManager, Console);
            DropBays = new DropBaysViewModel(DataRouter, MetadataManager);
            Power = new PowerViewModel(DataRouter, MetadataManager, Console);
            CameraMux = new CameraMuxViewModel(DataRouter, MetadataManager);
            ExternalControls = new ExternalControlsViewModel(DataRouter, MetadataManager);

            DriveControllerMode = new DriveControllerModeViewModel(null, DataRouter, MetadataManager);
            ArmControllerMode = new ArmControllerModeViewModel(null, DataRouter, MetadataManager, Console);
            GimbalControllerMode = new GimbalControllerModeViewModel(null, DataRouter, MetadataManager, Console, 0);
            Gimbal2ControllerMode = new GimbalControllerModeViewModel(null, DataRouter, MetadataManager, Console, 1);
            XboxController = new XboxControllerInputViewModel(DataRouter, MetadataManager, Console, StateManager);

            var XboxToDriveMapping = new MappingViewModel();
            XboxToDriveMapping.Device = XboxController;
            XboxToDriveMapping.Mode = DriveControllerMode;
            XboxToDriveMapping.UpdatePeriod = 30;
            XboxToDriveMapping.Channels.Add(new MappingChannelViewModel()
            {
                Name = "Left Wheels",
                InputKey = "JoyStick1Y",
                OutputKey = "WheelsLeft",
                LinearScaling = 1f,
                Minimum = -1f,
                Maximum = 1f,
                Parabolic = true
            });
            XboxToDriveMapping.Channels.Add(new MappingChannelViewModel()
            {
                Name = "Right Wheels",
                InputKey = "JoyStick2Y",
                OutputKey = "WheelsRight",
                LinearScaling = 1f,
                Minimum = -1f,
                Maximum = 1f,
                Parabolic = true
            });
            XboxToDriveMapping.IsActive = true;

            InputManager = new InputManagerViewModel(
                new IInputDevice[] { XboxController },
                new MappingViewModel[] { XboxToDriveMapping },
                new IInputMode[] { DriveControllerMode, ArmControllerMode, GimbalControllerMode, Gimbal2ControllerMode });

            SettingsManager = new SettingsManagerViewModel(this);

            FlightStickViewModel x = new FlightStickViewModel();

            InputManager.Start();
            //DataRouter.Send(100, new byte[] { 10, 20, 30, 40 });
            //DataRouter.Send(1, new byte[] { 2, 3, 4, 5 });
            //DataRouter.Send(101, new byte[] { 15, 25, 35, 45 });
            //DataRouter.Send(180, new byte[] { 0x23, 0x52, 0x4f, 0x56, 0x45, 0x53, 0x4f, 0x48, 0x41, 0x52, 0x44, 0x00 });
        }
    }
}
