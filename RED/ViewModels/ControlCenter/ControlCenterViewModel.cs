namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using Caliburn.Micro;
    using Contexts;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using RED.Models.ControlCenter;

    public class ControlCenterViewModel : Screen
    {
        private ControlCenterModel Model;

        public RemoveModuleStateVm RemoveModuleState
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
        public SaveModuleStateVm SaveModuleState
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

        public StateManager StateManager
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
        public ConsoleVm Console
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
        public ModuleGridManager GridManager
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
            StateManager = new StateManager();
            Console = new ConsoleVm();
            GridManager = new ModuleGridManager(this);

            RemoveModuleState = new RemoveModuleStateVm(this);
            SaveModuleState = new SaveModuleStateVm(GridManager.ModuleGrid, this);

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
