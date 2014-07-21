namespace RED.ViewModels.ControlCenter
{
	using Addons;
	using Interfaces;
	using Models.ControlCenter;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Input;

	public class ModuleManager : BaseViewModel
	{
		private readonly ModuleManagerModel _model = new ModuleManagerModel();

		public ControlCenterViewModel ControlCenter { get; set; }
		public string SelectedModule
		{
			get
			{
				return _model.SelectedModule;
			}
			set
			{
				_model.SelectedModule = value;
				NotifyOfPropertyChange();
			}
		}
		public IEnumerable<string> ManagedModulesTitles
		{
			get
			{
				return ControlCenter.Modules.Select(t => t.Title).ToList().OrderBy(t => t);
			}
		}

		private const string title = "Module Manager";
		public string Title
		{
			get { return title; }
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

		public ModuleManager(ControlCenterViewModel controlCenter)
		{
			ControlCenter = controlCenter;
			LoadLeftCommand = new RelayCommand(c => LoadModule(ModulePosition.Left), b => SelectedModule != null);
			LoadRightCommand = new RelayCommand(c => LoadModule(ModulePosition.Right), b => SelectedModule != null);
			LoadTopCommand = new RelayCommand(c => LoadModule(ModulePosition.Top), b => SelectedModule != null);
			LoadMiddleCommand = new RelayCommand(c => LoadModule(ModulePosition.Middle), b => SelectedModule != null);
			LoadBottomCommand = new RelayCommand(c => LoadModule(ModulePosition.Bottom), b => SelectedModule != null);
		}

		public void LoadModule(ModulePosition position)
		{
			switch (position)
			{
				case ModulePosition.Left:
					ControlCenter.LeftSelection = SelectedModule;
					break;
				case ModulePosition.Right:
					ControlCenter.RightSelection = SelectedModule;
					break;
				case ModulePosition.Top:
					ControlCenter.TopSelection = SelectedModule;
					break;
				case ModulePosition.Middle:
					ControlCenter.MiddleSelection = SelectedModule;
					break;
				case ModulePosition.Bottom:
					ControlCenter.BottomSelection = SelectedModule;
					break;
			}
		}
		public void RemoveModule(IModule module)
		{
			if (ControlCenter.LeftSelection == module.Title)
			{
				ControlCenter.LeftModule = null;
				ControlCenter.LeftSelection = String.Empty;
			}
			else if (ControlCenter.RightSelection == module.Title)
			{
				ControlCenter.RightModule = null;
				ControlCenter.RightSelection = String.Empty;
			}
			else if (ControlCenter.TopSelection == module.Title)
			{
				ControlCenter.TopModule = null;
				ControlCenter.TopSelection = String.Empty;
			}
			else if (ControlCenter.MiddleSelection == module.Title)
			{
				ControlCenter.MiddleModule = null;
				ControlCenter.MiddleSelection = String.Empty;
			}
			else if (ControlCenter.BottomSelection == module.Title)
			{
				ControlCenter.BottomModule = null;
				ControlCenter.BottomSelection = String.Empty;
			}
		}
		public void ClearModules()
		{
			ControlCenter.LeftModule = null;
			ControlCenter.LeftSelection = String.Empty;
			ControlCenter.RightModule = null;
			ControlCenter.RightSelection = String.Empty;
			ControlCenter.TopModule = null;
			ControlCenter.TopSelection = String.Empty;
			ControlCenter.MiddleModule = null;
			ControlCenter.MiddleSelection = String.Empty;
			ControlCenter.BottomModule = null;
			ControlCenter.BottomSelection = String.Empty;
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
