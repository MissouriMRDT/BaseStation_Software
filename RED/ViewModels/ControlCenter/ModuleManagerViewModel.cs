namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using Caliburn.Micro;
    using Interfaces;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using System.Xml.Serialization;

    public class ModuleManagerViewModel : PropertyChangedBase
	{
	    public ModuleGridViewModel ModuleGrid { get; set; }
	    public ControlCenterViewModel ControlCenter { get; set; }
        private string _selectedModule;
		public string SelectedModule
		{
			get
			{
				return _selectedModule;
			}
			set
			{
				_selectedModule = value;
				NotifyOfPropertyChange();
			}
		}
		public IEnumerable<string> ModuleTitles
		{
			get
			{
				return ModuleGrid.Modules.Select(t => t.Title).ToList().OrderBy(t => t);
			}
		}

		public ICommand LoadLeftCommand
		{
			get;
			private set;
		}
		public ICommand LoadRightCommand
		{
			get;
			private set;
		}
		public ICommand LoadTopCommand
		{
			get;
			private set;
		}
		public ICommand LoadMiddleCommand
		{
			get;
			private set;
		}
		public ICommand LoadBottomCommand
		{
			get;
			private set;
		}

		public ModuleManagerViewModel(ControlCenterViewModel controlCenter)
		{
		    ModuleGrid = new ModuleGridViewModel(this);
			ControlCenter = controlCenter;

			LoadLeftCommand = new RelayCommand(c => LoadModule(ModulePosition.Left), b => _selectedModule != null);
			LoadRightCommand = new RelayCommand(c => LoadModule(ModulePosition.Right), b => _selectedModule != null);
			LoadTopCommand = new RelayCommand(c => LoadModule(ModulePosition.Top), b => _selectedModule != null);
			LoadMiddleCommand = new RelayCommand(c => LoadModule(ModulePosition.Middle), b => _selectedModule != null);
			LoadBottomCommand = new RelayCommand(c => LoadModule(ModulePosition.Bottom), b => _selectedModule != null);

            ResetGridProportionsCommand = new RelayCommand(c => ResetGridProportions());
		}

        public ModuleManagerViewModel()
        {
            ModuleGrid = new ModuleGridViewModel(this);
        }

		public void LoadModule(ModulePosition position)
		{
			switch (position)
			{
				case ModulePosition.Left:
					ModuleGrid.LeftSelection = _selectedModule;
					break;
				case ModulePosition.Right:
                    ModuleGrid.RightSelection = _selectedModule;
					break;
				case ModulePosition.Top:
                    ModuleGrid.TopSelection = _selectedModule;
					break;
				case ModulePosition.Middle:
                    ModuleGrid.MiddleSelection = _selectedModule;
					break;
				case ModulePosition.Bottom:
                    ModuleGrid.BottomSelection = _selectedModule;
					break;
			}
		}
		public void RemoveModule(IModule module)
		{
            if (ModuleGrid.LeftSelection == module.Title)
			{
                ModuleGrid.LeftModule = null;
                ModuleGrid.LeftSelection = String.Empty;
			}
            else if (ModuleGrid.RightSelection == module.Title)
			{
                ModuleGrid.RightModule = null;
                ModuleGrid.RightSelection = String.Empty;
			}
            else if (ModuleGrid.TopSelection == module.Title)
			{
                ModuleGrid.TopModule = null;
                ModuleGrid.TopSelection = String.Empty;
			}
            else if (ModuleGrid.MiddleSelection == module.Title)
			{
                ModuleGrid.MiddleModule = null;
                ModuleGrid.MiddleSelection = String.Empty;
			}
            else if (ModuleGrid.BottomSelection == module.Title)
			{
                ModuleGrid.BottomModule = null;
                ModuleGrid.BottomSelection = String.Empty;
			}
		}
		public void ClearModules()
		{
            ModuleGrid.LeftModule = null;
            ModuleGrid.LeftSelection = String.Empty;
            ModuleGrid.RightModule = null;
            ModuleGrid.RightSelection = String.Empty;
            ModuleGrid.TopModule = null;
            ModuleGrid.TopSelection = String.Empty;
            ModuleGrid.MiddleModule = null;
            ModuleGrid.MiddleSelection = String.Empty;
            ModuleGrid.BottomModule = null;
            ModuleGrid.BottomSelection = String.Empty;
		}

        public void LoadModuleSave(string name)
        {
            var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));
            var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +
                Settings.Default.ModuleStateSaveFileName);
            var existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
            var save = existingSaves.Single(s => s.Name == name);

            ClearModules();
            ModuleGrid.LeftSelection = save.LeftSelection ?? String.Empty;
            ModuleGrid.RightSelection = save.RightSelection ?? String.Empty;
            ModuleGrid.TopSelection = save.TopSelection ?? String.Empty;
            ModuleGrid.MiddleSelection = save.MiddleSelection ?? String.Empty;
            ModuleGrid.BottomSelection = save.BottomSelection ?? String.Empty;
            ModuleGrid.Column1Width = save.Column1Width;
            ModuleGrid.Column3Width = save.Column3Width;
            ModuleGrid.Column5Width = save.Column5Width;
            ModuleGrid.Row1Height = save.Row1Height;
            ModuleGrid.Row3Height = save.Row3Height;
            ModuleGrid.Row5Height = save.Row5Height;
        }
        public ICommand ResetGridProportionsCommand { get; set; }
        private void ResetGridProportions()
        {
            ModuleGrid.Column1Width = "1*";
            ModuleGrid.Column3Width = "2*";
            ModuleGrid.Column5Width = "1*";
            ModuleGrid.Row1Height = "1*";
            ModuleGrid.Row3Height = "1*";
            ModuleGrid.Row5Height = "1*";
        }

		public enum ModulePosition
		{
			Left,
			Right,
			Top,
			Middle,
			Bottom
		}
	}
}
