using Caliburn.Micro;
using RED.Interfaces.Input;
using RED.Models;
using RED.ViewModels.Input;
using RED.ViewModels.Input.Controllers;
using RED.ViewModels.Modules;
using RED.ViewModels.Network;
using System.IO;

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
        public ScienceArmViewModel ScienceArm
        {
            get
            {
                return _model._scienceArm;
            }
            set
            {
                _model._scienceArm = value;
                NotifyOfPropertyChange(() => ScienceArm);
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
        public GimbalViewModel Gimbal1
        {
            get
            {
                return _model._gimbal1;
            }
            set
            {
                _model._gimbal1 = value;
                NotifyOfPropertyChange(() => Gimbal1);
            }
        }
        public GimbalViewModel Gimbal2
        {
            get
            {
                return _model._gimbal2;
            }
            set
            {
                _model._gimbal2 = value;
                NotifyOfPropertyChange(() => Gimbal2);
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

        public ControlCenterViewModel()
        {
            base.DisplayName = "Rover Engagement Display";
            _model = new ControlCenterModel();

            Console = new ConsoleViewModel();
            DataRouter = new DataRouter();
            MetadataManager = new MetadataManager(Console);
            MetadataManager.AddFromFile("NoSyncMetadata.xml");

            NetworkManager = new NetworkManagerViewModel(DataRouter, MetadataManager.Commands.ToArray(), Console, MetadataManager);
            SubscriptionManager = new SubscriptionManagerViewModel(Console, MetadataManager.Telemetry.ToArray(), MetadataManager, NetworkManager);
            StateManager = new StateViewModel(SubscriptionManager);

            Science = new ScienceViewModel(DataRouter, MetadataManager, Console);
            GPS = new GPSViewModel(DataRouter, MetadataManager);
            Sensor = new SensorViewModel(DataRouter, MetadataManager, Console);
            DropBays = new DropBaysViewModel(DataRouter, MetadataManager);
            Power = new PowerViewModel(DataRouter, MetadataManager, Console);
            CameraMux = new CameraViewModel(DataRouter, MetadataManager);
            ExternalControls = new ExternalControlsViewModel(DataRouter, MetadataManager);
            Autonomy = new AutonomyViewModel(DataRouter, MetadataManager, Console);
            ScienceArm = new ScienceArmViewModel(null, DataRouter, MetadataManager, Console);

            Drive = new DriveViewModel(null, DataRouter, MetadataManager);
            Arm = new ArmViewModel(null, DataRouter, MetadataManager, Console);
            Gimbal1 = new GimbalViewModel(null, DataRouter, MetadataManager, Console, 0);
            Gimbal2 = new GimbalViewModel(null, DataRouter, MetadataManager, Console, 1);
            XboxController1 = new XboxControllerInputViewModel(1, StateManager);
            XboxController2 = new XboxControllerInputViewModel(2, StateManager);
            XboxController3 = new XboxControllerInputViewModel(3, StateManager);
            XboxController4 = new XboxControllerInputViewModel(4, StateManager);
            FlightStickController = new FlightStickViewModel();

            InputManager = new InputManagerViewModel(Console,
                new IInputDevice[] { XboxController1, XboxController2, XboxController3, XboxController4, FlightStickController },
                new MappingViewModel[0],
                new IInputMode[] { Drive, Arm, Gimbal1, Gimbal2, ScienceArm });
            InputManager.LoadMappingsFromFile("inputmappings.xml");
            if (File.Exists("inputselections.xml"))
                InputManager.LoadSelectionsFromFile("inputselections.xml");

            SettingsManager = new SettingsManagerViewModel(this);

            InputManager.Start();
            //DataRouter.Send(100, new byte[] { 10, 20, 30, 40 });
            //DataRouter.Send(1, new byte[] { 2, 3, 4, 5 });
            //DataRouter.Send(101, new byte[] { 15, 25, 35, 45 });
            //DataRouter.Send(180, new byte[] { 0x23, 0x52, 0x4f, 0x56, 0x45, 0x53, 0x4f, 0x48, 0x41, 0x52, 0x44, 0x00 });
        }

        protected override void OnDeactivate(bool close)
        {
            InputManager.SaveSelectionsToFile("inputselections.xml");
            base.OnDeactivate(close);
        }
    }
}
