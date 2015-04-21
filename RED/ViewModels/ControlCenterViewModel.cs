namespace RED.ViewModels
{
    using Caliburn.Micro;
    using ControlCenter;
    using Models;

    public class ControlCenterViewModel : Screen
    {
        private readonly ControlCenterModel _model;

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

        public ControlCenterViewModel()
        {
            base.DisplayName = "Rover Engagement Display";
            _model = new ControlCenterModel();
            StateManager = new StateViewModel(this);
            Console = new ConsoleViewModel();
            DataRouter = new DataRouter();
            MetadataManager = new MetadataManager(this);
            TcpAsyncServer = new AsyncTcpServerViewModel(11000, this);
            ModuleManager = new ModuleManagerViewModel(this);
            Input = new InputViewModel(this);

            RemoveModuleState = new RemoveModuleStateViewModel(this);
            SaveModuleState = new SaveModuleStateViewModel(ModuleManager.ModuleGrid, this);

            ModuleManager.ReloadModuleButtonContexts();
        }
    }
}
