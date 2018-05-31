using Caliburn.Micro;
using RED.Interfaces.Input;
using RED.Models;
using RED.Roveprotocol;
using RED.ViewModels.Input;
using RED.ViewModels.Input.Controllers;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using RED.ViewModels.Network;
using RED.ViewModels.Tools;

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
        public StopwatchToolViewModel StopwatchTool
        {
            get
            {
                return _model._stopwatchTool;
            }
            set
            {
                _model._stopwatchTool = value;
                NotifyOfPropertyChange(() => StopwatchTool);
            }
        }
        public TelemetryLogToolViewModel TelemetryLogTool
        {
            get
            {
                return _model._telemetryLogTool;
            }
            set
            {
                _model._telemetryLogTool = value;
                NotifyOfPropertyChange(() => TelemetryLogTool);
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
        public CameraViewModel CameraMux
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
        public LightingViewModel Lighting
        {
            get
            {
                return _model._lighting;
            }
            set
            {
                _model._lighting = value;
                NotifyOfPropertyChange(() => Lighting);
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

        public DriveViewModel Drive
        {
            get
            {
                return _model._drive;
            }
            set
            {
                _model._drive = value;
                NotifyOfPropertyChange(() => Drive);
            }
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
        public GimbalViewModel Gimbal
        {
            get
            {
                return _model._gimbal;
            }
            set
            {
                _model._gimbal = value;
                NotifyOfPropertyChange(() => Gimbal);
            }
        }
        public XboxControllerInputViewModel XboxController1
        {
            get
            {
                return _model._xboxController1;
            }
            set
            {
                _model._xboxController1 = value;
                NotifyOfPropertyChange(() => XboxController1);
            }
        }
        public XboxControllerInputViewModel XboxController2
        {
            get
            {
                return _model._xboxController2;
            }
            set
            {
                _model._xboxController2 = value;
                NotifyOfPropertyChange(() => XboxController2);
            }
        }
        public XboxControllerInputViewModel XboxController3
        {
            get
            {
                return _model._xboxController3;
            }
            set
            {
                _model._xboxController3 = value;
                NotifyOfPropertyChange(() => XboxController3);
            }
        }
        public XboxControllerInputViewModel XboxController4
        {
            get
            {
                return _model._xboxController4;
            }
            set
            {
                _model._xboxController4 = value;
                NotifyOfPropertyChange(() => XboxController4);
            }
        }
        public FlightStickViewModel FlightStickController
        {
            get
            {
                return _model._flightStickController;
            }
            set
            {
                _model._flightStickController = value;
                NotifyOfPropertyChange(() => FlightStickController);
            }
        }
        public KeyboardInputViewModel KeyboardController
        {
            get
            {
                return _model._keyboardController;
            }
            set
            {
                _model._keyboardController = value;
                NotifyOfPropertyChange(() => KeyboardController);
            }
        }

        public ControlCenterViewModel()
        {
            base.DisplayName = "Rover Engagement Display";
            _model = new ControlCenterModel();

            Console = new ConsoleViewModel();
            ConfigManager = new XMLConfigManager(Console);
            MetadataManager = new MetadataManager(Console, ConfigManager);
       
            NetworkManager = new NetworkManagerViewModel(Console);
            Rovecomm = new Rovecomm(NetworkManager, Console, MetadataManager);
            ResubscribeAll();

            Science = new ScienceViewModel(Rovecomm, MetadataManager, Console);
            GPS = new GPSViewModel(Rovecomm, MetadataManager);
            Sensor = new SensorViewModel(Rovecomm, MetadataManager, Console);
            DropBays = new DropBaysViewModel(Rovecomm, MetadataManager, Console);
            Power = new PowerViewModel(Rovecomm, MetadataManager, Console);
            CameraMux = new CameraViewModel(Rovecomm, MetadataManager);
            ExternalControls = new ExternalControlsViewModel(Rovecomm, MetadataManager);
            Autonomy = new AutonomyViewModel(Rovecomm, MetadataManager, Console);
            Lighting = new LightingViewModel(Rovecomm, MetadataManager, Console);
            Map = new MapViewModel();

            Drive = new DriveViewModel(Rovecomm, MetadataManager);
            Arm = new ArmViewModel(Rovecomm, MetadataManager, Console, ConfigManager);
            Gimbal = new GimbalViewModel(Rovecomm, MetadataManager, Console);
            XboxController1 = new XboxControllerInputViewModel(1);
            XboxController2 = new XboxControllerInputViewModel(2);
            XboxController3 = new XboxControllerInputViewModel(3);
            XboxController4 = new XboxControllerInputViewModel(4);
            FlightStickController = new FlightStickViewModel();
            KeyboardController = new KeyboardInputViewModel();

            // Programatic instanciation of InputManager view, vs static like everything else in a xaml 
            InputManager = new InputManagerViewModel(Console, ConfigManager,
                new IInputDevice[] { XboxController1, XboxController2, XboxController3, XboxController4, FlightStickController, KeyboardController },
                new MappingViewModel[0],
                new IInputMode[] { Drive, Arm, Gimbal, Science });

            WaypointManager = new WaypointManagerViewModel(Map, GPS, Autonomy);
            PingTool = new PingToolViewModel(Rovecomm, ConfigManager);
            StopwatchTool = new StopwatchToolViewModel(ConfigManager);
            TelemetryLogTool = new TelemetryLogToolViewModel(NetworkManager, MetadataManager);

            SettingsManager = new SettingsManagerViewModel(ConfigManager, this);
        }

        protected override void OnDeactivate(bool close)
        {
            InputManager.SaveConfigurations();
            Arm.SaveConfigurations();
            base.OnDeactivate(close);
        }

        public void ResubscribeAll()
        {
            Rovecomm.SubscribeMyPCToAllDevices();
        }
    }
}
