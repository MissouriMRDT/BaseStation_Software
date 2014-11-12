namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using Caliburn.Micro;
    using Contexts;
    using Models.ControlCenter;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class ControlCenterViewModel : Screen
    {
        private ControlCenterModel Model;

        public RemoveModuleStateViewModel RemoveModuleState
        {
            get
            {
                return Model.RemoveModuleState;
            }
            set
            {
                Model.RemoveModuleState = value;
                NotifyOfPropertyChange(() => RemoveModuleState);
            }
        }
        public SaveModuleStateViewModel SaveModuleState
        {
            get
            {
                return Model.SaveModuleState;
            }
            set
            {
                Model.SaveModuleState = value;
                NotifyOfPropertyChange(() => SaveModuleState);
            }
        }

        public StateViewModel StateManager
        {
            get
            {
                return Model.StateManager;
            }
            set
            {
                Model.StateManager = value;
                NotifyOfPropertyChange();
            }
        }
        public ConsoleViewModel Console
        {
            get
            {
                return Model.Console;
            }
            set
            {
                Model.Console = value;
                NotifyOfPropertyChange();
            }
        }
        public DataRouter DataRouter
        {
            get
            {
                return Model.DataRouter;
            }
            set
            {
                Model.DataRouter = value;
                NotifyOfPropertyChange(() => DataRouter);
            }
        }
        public AsyncTcpServer TcpAsyncServer
        {
            get
            {
                return Model.TcpAsyncServer;
            }
            set
            {
                Model.TcpAsyncServer = value;
                NotifyOfPropertyChange(() => TcpAsyncServer);
            }
        }
        public ModuleManagerViewModel GridManager
        {
            get
            {
                return Model.GridManager;
            }
            set
            {
                Model.GridManager = value;
                NotifyOfPropertyChange(() => GridManager);
            }
        }

        public ControlCenterViewModel()
        {
            Model = new ControlCenterModel();
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
