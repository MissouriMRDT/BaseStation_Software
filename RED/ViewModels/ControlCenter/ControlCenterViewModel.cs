namespace RED.ViewModels.ControlCenter
{
	using System.Threading;
	using System.Threading.Tasks;
	using Addons;
	using Contexts;
	using Interfaces;
	using Models.ControlCenter;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Windows.Input;
	using System.Xml.Serialization;
	using Properties;

	public class ControlCenterViewModel : BaseViewModel
	{
		internal SaveModuleStateVm SaveModuleStateVm;
		internal RemoveModuleStateVm RemoveModuleStateVm;

		// Don't move this _model.  Execution order is vital.
		private readonly ControlCenterModel _model = new ControlCenterModel();

		//private LexiconEngine LexiconEngine { get; set; }

		public IEnumerable<IModule> Modules
		{
			get
			{
				return _model.AllModules;
			}
		}

		public string LeftSelection
		{
			get
			{
				return _model.LeftSelection;
			}
			set
			{
				if (value == String.Empty)
					_model.LeftSelection = null;
				else
				{
					var module = Modules.Single(t => t.Title == value);
					if (module.InUse)
						ModuleManager.RemoveModule(module);
					_model.LeftSelection = value;
					LeftModule = module;
				}
				NotifyOfPropertyChange();
			}
		}
		public string RightSelection
		{
			get
			{
				return _model.RightSelection;
			}
			set
			{
				if (value == String.Empty)
					_model.RightSelection = null;
				else
				{
					var module = Modules.Single(t => t.Title == value);
					if (module.InUse)
						ModuleManager.RemoveModule(module);
					_model.RightSelection = value;
					RightModule = module;
				}
				NotifyOfPropertyChange();
			}
		}
		public string TopSelection
		{
			get
			{
				return _model.TopSelection;
			}
			set
			{
				if (value == String.Empty)
					_model.TopSelection = null;
				else
				{
					var module = Modules.Single(t => t.Title == value);
					if (module.InUse)
						ModuleManager.RemoveModule(module);
					_model.TopSelection = value;
					TopModule = module;
				}
				NotifyOfPropertyChange();
			}
		}
		public string MiddleSelection
		{
			get
			{
				return _model.MiddleSelection;
			}
			set
			{
				if (value == String.Empty)
					_model.MiddleSelection = null;
				else
				{
					var module = Modules.Single(t => t.Title == value);
					if (module.InUse)
						ModuleManager.RemoveModule(module);
					_model.MiddleSelection = value;
					MiddleModule = module;
				}
				NotifyOfPropertyChange();
			}
		}
		public string BottomSelection
		{
			get
			{
				return _model.BottomSelection;
			}
			set
			{
				if (value == String.Empty)
					_model.BottomSelection = null;
				else
				{
					var module = Modules.Single(t => t.Title == value);
					if (module.InUse)
						ModuleManager.RemoveModule(module);
					_model.BottomSelection = value;
					BottomModule = module;
				}
				NotifyOfPropertyChange();
			}
		}

		public IModule LeftModule
		{
			get
			{
				return _model.LeftModule;
			}
			set
			{
				if (value != null)
					value.InUse = true;
				_model.LeftModule = value;
				NotifyOfPropertyChange();
			}
		}
		public IModule RightModule
		{
			get
			{
				return _model.RightModule;
			}
			set
			{
				if (value != null)
					value.InUse = true;
				_model.RightModule = value;
				NotifyOfPropertyChange();
			}
		}
		public IModule TopModule
		{
			get
			{
				return _model.TopModule;
			}
			set
			{
				if (value != null)
					value.InUse = true;
				_model.TopModule = value;
				NotifyOfPropertyChange();
			}
		}
		public IModule MiddleModule
		{
			get
			{
				return _model.MiddleModule;
			}
			set
			{
				if (value != null)
					value.InUse = true;
				_model.MiddleModule = value;
				NotifyOfPropertyChange();
			}
		}
		public IModule BottomModule
		{
			get
			{
				return _model.BottomModule;
			}
			set
			{
				if (value != null)
					value.InUse = true;
				_model.BottomModule = value;
				NotifyOfPropertyChange();
			}
		}

		public StateManager StateManager
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
		public ConsoleVm Console
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
		public ModuleManager ModuleManager
		{
			get
			{
				return _model.ModuleManager;
			}
			set
			{
				_model.ModuleManager = value;
				NotifyOfPropertyChange();
			}
		}

		public string Column1Width
		{
			get { return _model.Column1Width; }
			set
			{
				_model.Column1Width = value;
				NotifyOfPropertyChange();
			}
		}
		public string Column3Width
		{
			get { return _model.Column3Width; }
			set
			{
				_model.Column3Width = value;
				NotifyOfPropertyChange();
			}
		}
		public string Column5Width
		{
			get { return _model.Column5Width; }
			set
			{
				_model.Column5Width = value;
				NotifyOfPropertyChange();
			}
		}
		public string Row1Height
		{
			get { return _model.Row1Height; }
			set
			{
				_model.Row1Height = value;
				NotifyOfPropertyChange();
			}
		}
		public string Row3Height
		{
			get { return _model.Row3Height; }
			set
			{
				_model.Row3Height = value;
				NotifyOfPropertyChange();
			}
		}
		public string Row5Height
		{
			get { return _model.Row5Height; }
			set
			{
				_model.Row5Height = value;
				NotifyOfPropertyChange();
			}
		}

		public ControlCenterViewModel()
		{
			RemoveModuleStateVm = new RemoveModuleStateVm(this);
			SaveModuleStateVm = new SaveModuleStateVm(this);

			// Populate the side panel on the right
			_model.StateManager = new StateManager();
			_model.Console = new ConsoleVm();
			_model.ModuleManager = new ModuleManager(this);

			// Inform Hazel that the system is online so that she can speak
			// Hazel.SystemInitialized = true;
			// Initialize the Lexicon Engine - IoC
			// LexiconEngine = new LexiconEngine(this);

			ReloadModuleButtonContexts();

			ResetGridProportionsCommand = new RelayCommand(c => ResetGridProportions());
		}

		#region Save/Load Module States
		public ObservableCollection<ButtonContext> ButtonContexts
		{
			get
			{
				return _model.ButtonContexts;
			}
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
					ButtonContexts.Add(new ButtonContext(new RelayCommand(o => LoadModuleSave(name1)), name));
				}
			}
			catch (Exception ex)
			{
				Console.WriteToConsole(ex.Message);
			}
		}
		public void LoadModuleSave(string name)
		{
			var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));
			var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +
				Settings.Default.ModuleStateSaveFileName);
			var existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
			var save = existingSaves.Single(s => s.Name == name);

			ModuleManager.ClearModules();
			LeftSelection = save.LeftSelection ?? String.Empty;
			RightSelection = save.RightSelection ?? String.Empty;
			TopSelection = save.TopSelection ?? String.Empty;
			MiddleSelection = save.MiddleSelection ?? String.Empty;
			BottomSelection = save.BottomSelection ?? String.Empty;
			Column1Width = save.Column1Width;
			Column3Width = save.Column3Width;
			Column5Width = save.Column5Width;
			Row1Height = save.Row1Height;
			Row3Height = save.Row3Height;
			Row5Height = save.Row5Height;
		}
		public ICommand ResetGridProportionsCommand { get; set; }
		private void ResetGridProportions()
		{
			Column1Width = "1*";
			Column3Width = "2*";
			Column5Width = "1*";
			Row1Height = "1*";
			Row3Height = "1*";
			Row5Height = "1*";
		}
		#endregion
	}
}
