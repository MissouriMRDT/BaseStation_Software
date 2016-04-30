namespace RED.ViewModels
{
    using Caliburn.Micro;
    using ControlCenter;
    using Models;
    using Interfaces;

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
        public InputManagerViewModel IManager
        {
            get
            {
                return _model._input;
            }
            set
            {
                _model._input = value;
                NotifyOfPropertyChange(() => IManager);
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
        public SensorCombinedViewModel SensorCombined
        {
            get
            {
                return _model._sensorCombined;
            }
            set
            {
                _model._sensorCombined = value;
                NotifyOfPropertyChange(() => SensorCombined);

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

        public DriveControllerModeViewModel DriveControllerMode
        {
            get
            {
                return (DriveControllerModeViewModel)IManager.Input.ControllerModes[0];
            }
        }
        public ArmControllerModeViewModel ArmControllerMode
        {
            get
            {
                return (ArmControllerModeViewModel)IManager.Input.ControllerModes[1];
            }
        }
        public GimbalControllerModeViewModel GimbalControllerMode
        {
            get
            {
                return (GimbalControllerModeViewModel)IManager.Input.ControllerModes[2];
            }
        }

        public ControlCenterViewModel()
        {
            base.DisplayName = "Rover Engagement Display";
            _model = new ControlCenterModel();

            StateManager = new StateViewModel(this);
            Console = new ConsoleViewModel();
            DataRouter = new DataRouter();
            MetadataManager = new MetadataManager(this);
            MetadataManager.AddFromFile("NoSyncMetadata.xml");
            IManager = new InputManagerViewModel(this);
            NetworkManager = new NetworkManagerViewModel(this);
            SubscriptionManager = new SubscriptionManagerViewModel(this);
            Science = new ScienceViewModel(this);
            Science.StartCCDListener();
            GPS = new GPSViewModel(this);
            Sensor = new SensorViewModel(this);
            SensorCombined = new SensorCombinedViewModel(this);
            DropBays = new DropBaysViewModel(this);

            SettingsManager = new SettingsManagerViewModel(this);

            IManager.Start();
            //DataRouter.Send(100, new byte[] { 10, 20, 30, 40 });
            //DataRouter.Send(1, new byte[] { 2, 3, 4, 5 });
            //DataRouter.Send(101, new byte[] { 15, 25, 35, 45 });
            //DataRouter.Send(180, new byte[] { 0x23, 0x52, 0x4f, 0x56, 0x45, 0x53, 0x4f, 0x48, 0x41, 0x52, 0x44, 0x00 });
        }
    }
}
