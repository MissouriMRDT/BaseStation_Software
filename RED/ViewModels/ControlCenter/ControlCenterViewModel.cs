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

        public ObservableCollection<ButtonContext> ButtonContexts
        {
            get
            {
                return Model.ButtonContexts;
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
        public DataRouterVM DataRouter
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
        public TCPAsyncServerVM TcpAsyncServer
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
            DataRouter = new DataRouterVM();
            TcpAsyncServer = new TCPAsyncServerVM(11000, this);
            GridManager = new ModuleManagerViewModel(this);

            RemoveModuleState = new RemoveModuleStateViewModel(this);
            SaveModuleState = new SaveModuleStateViewModel(GridManager.ModuleGrid, this);

            ReloadModuleButtonContexts();
        }

        public void ReloadModuleButtonContexts()
        {
            var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));

            // Get existing saves if there are any.
            try
            {
                ButtonContexts.Clear();
                var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +
                    Settings.Default.ModuleStateSaveFileName);
                var existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
                fileReader.Close();
                foreach (var name in existingSaves.Select(save => save.Name))
                {
                    var name1 = name;
                    ButtonContexts.Add(new ButtonContext(new RelayCommand(o => GridManager.LoadModuleSave(name1)), name));
                }
            }
            catch (Exception ex)
            {
                Console.WriteToConsole(ex.Message);
            }
        }
    }
}
