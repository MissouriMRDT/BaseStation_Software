namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Models.ControlCenter;

    public class ControlCenterViewModel : Screen
    {
        private readonly ControlCenterModel _model;

        public RemoveModuleStateViewModel RemoveModuleState
        {
            get
            {
                return _model.RemoveModuleState;
            }
            set
            {
                _model.RemoveModuleState = value;
                NotifyOfPropertyChange(() => RemoveModuleState);
            }
        }
        public SaveModuleStateViewModel SaveModuleState
        {
            get
            {
                return _model.SaveModuleState;
            }
            set
            {
                _model.SaveModuleState = value;
                NotifyOfPropertyChange(() => SaveModuleState);
            }
        }

        public StateViewModel StateManager
        {
            get
            {
                return _model.StateManager;
            }
            set
            {
                _model.StateManager = value;
                NotifyOfPropertyChange();
            }
        }
        public ConsoleViewModel Console
        {
            get
            {
                return _model.Console;
            }
            set
            {
                _model.Console = value;
                NotifyOfPropertyChange();
            }
        }
        public DataRouter DataRouter
        {
            get
            {
                return _model.DataRouter;
            }
            set
            {
                _model.DataRouter = value;
                NotifyOfPropertyChange(() => DataRouter);
            }
        }
        public AsyncTcpServer TcpAsyncServer
        {
            get
            {
                return _model.TcpAsyncServer;
            }
            set
            {
                _model.TcpAsyncServer = value;
                NotifyOfPropertyChange(() => TcpAsyncServer);
            }
        }
        public ModuleManagerViewModel GridManager
        {
            get
            {
                return _model.GridManager;
            }
            set
            {
                _model.GridManager = value;
                NotifyOfPropertyChange(() => GridManager);
            }
        }

        public ControlCenterViewModel()
        {
            _model = new ControlCenterModel();
            StateManager = new StateViewModel(this);
            Console = new ConsoleViewModel();
            DataRouter = new DataRouter();
            TcpAsyncServer = new AsyncTcpServer(11000, this);
            GridManager = new ModuleManagerViewModel(this);

            RemoveModuleState = new RemoveModuleStateViewModel(this);
            SaveModuleState = new SaveModuleStateViewModel(GridManager.ModuleGrid, this);

            GridManager.ReloadModuleButtonContexts();
        }
    }
}
