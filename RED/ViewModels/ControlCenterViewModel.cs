namespace RED.ViewModels
{
    using Caliburn.Micro;
    using ControlCenter;
    using Models;

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

        public RemoveModuleStateViewModel RemoveModuleState
        {
            get
            {
                return _model._removeModuleState;
            }
            set
            {
                _model._removeModuleState = value;
                NotifyOfPropertyChange(() => RemoveModuleState);
            }
        }
        public SaveModuleStateViewModel SaveModuleState
        {
            get
            {
                return _model._saveModuleState;
            }
            set
            {
                _model._saveModuleState = value;
                NotifyOfPropertyChange(() => SaveModuleState);
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
        public AsyncTcpServerViewModel TcpAsyncServer
        {
            get
            {
                return _model._tcpAsyncServer;
            }
            set
            {
                _model._tcpAsyncServer = value;
                NotifyOfPropertyChange(() => TcpAsyncServer);
            }
        }
        public ModuleManagerViewModel ModuleManager
        {
            get
            {
                return _model._gridManager;
            }
            set
            {
                _model._gridManager = value;
                NotifyOfPropertyChange(() => ModuleManager);
            }
        }
        public InputViewModel Input
        {
            get
            {
                return _model._input;
            }
            set
            {
                _model._input = value;
                NotifyOfPropertyChange(() => Input);
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

        public DriveControllerModeViewModel DriveControllerMode
        {
            get
            {
                return (DriveControllerModeViewModel)Input.ControllerModes[0];
            }
        }
        public ArmControllerModeViewModel ArmControllerMode
        {
            get
            {
                return (ArmControllerModeViewModel)Input.ControllerModes[1];
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

            TcpAsyncServer = new AsyncTcpServerViewModel(11000, this);
            ModuleManager = new ModuleManagerViewModel(this);
            Input = new InputViewModel(this);
            Science = new ScienceViewModel(this);
            GPS = new GPSViewModel(this);
            Sensor = new SensorViewModel(this);
            SensorCombined = new SensorCombinedViewModel(this);

            RemoveModuleState = new RemoveModuleStateViewModel(this);
            SaveModuleState = new SaveModuleStateViewModel(ModuleManager.ModuleGrid, this);

            SettingsManager = new SettingsManagerViewModel(this);

            Input.Start();

            ModuleManager.ReloadModuleButtonContexts();
        }
    }
}
