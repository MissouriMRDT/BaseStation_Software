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

	public class ControlCenterViewModel : Screen
	{
		private SaveModuleStateVm _saveModuleStateVm;
        private RemoveModuleStateVm _removeModuleStateVm;

        private readonly ObservableCollection<ButtonContext> _buttonContexts = new ObservableCollection<ButtonContext>();

        private StateManager _stateManager;
        private ConsoleVm _console;

		public StateManager StateManager
		{
			get
			{
				return _stateManager;
			}
			set
			{
				_stateManager = value;
				NotifyOfPropertyChange();
			}
		}
		public ConsoleVm Console
		{
			get
			{
				return _console;
			}
			set
			{
				_console = value;
				NotifyOfPropertyChange();
			}
		}
		public ModuleGridManager GridManager { get; set; }

		public ControlCenterViewModel()
		{
			_stateManager = new StateManager();
			_console = new ConsoleVm();
            GridManager = new ModuleGridManager(this);
            
            _removeModuleStateVm = new RemoveModuleStateVm(this);
            _saveModuleStateVm = new SaveModuleStateVm(GridManager.ModuleGrid, this);

			ReloadModuleButtonContexts();
		}

		public ObservableCollection<ButtonContext> ButtonContexts
		{
			get
			{
				return _buttonContexts;
			}
		}

        public void ReloadModuleButtonContexts()
        {
            var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));

            // Get existing saves if there are any.
            try
            {
                _buttonContexts.Clear();
                var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +
                    Settings.Default.ModuleStateSaveFileName);
                var existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
                fileReader.Close();
                foreach (var name in existingSaves.Select(save => save.Name))
                {
                    var name1 = name;
                    _buttonContexts.Add(new ButtonContext(new RelayCommand(o => GridManager.LoadModuleSave(name1)), name));
                }
            }
            catch (Exception ex)
            {
                _console.WriteToConsole(ex.Message);
            }
        }
	}
}
