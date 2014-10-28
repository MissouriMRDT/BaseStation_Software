namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using Caliburn.Micro;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using System.Xml.Serialization;

    public class SaveModuleStateVm : PropertyChangedBase
	{
		private readonly ModuleGridViewModel _grid;
        private readonly ControlCenterViewModel _controlCenter;

		private string _name = string.Empty;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				NotifyOfPropertyChange();
			}
		}

		public ICommand SaveStateCommand { get; set; }

		public SaveModuleStateVm()
		{
			
		}
		public SaveModuleStateVm(ModuleGridViewModel grid, ControlCenterViewModel cc)
		{
			_grid = grid;
		    _controlCenter = cc;
			SaveStateCommand = new RelayCommand(c => Save(), b => Name.Length > 0);
		}
		
		public void Save()
		{
			var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));

			// Get existing saves if there are any.
			var existingSaves = new List<ModuleStateSave>();
			try
			{
				var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + 
					Settings.Default.ModuleStateSaveFileName);
				existingSaves = (List<ModuleStateSave>) serializer.Deserialize(fileReader);
				fileReader.Close();
			}
			catch (Exception ex)
			{
                _controlCenter.Console.WriteToConsole(ex.Message);
			}

			// Save already exists, update values state.
			if (existingSaves.Exists(s => s.Name == _name))
			{
				var existingSave = existingSaves.Single(s => s.Name == _name);
				existingSave.LeftSelection = _grid.LeftSelection;
				existingSave.RightSelection = _grid.RightSelection;
				existingSave.TopSelection = _grid.TopSelection;
				existingSave.MiddleSelection = _grid.MiddleSelection;
				existingSave.BottomSelection = _grid.BottomSelection;
				existingSave.Column1Width = _grid.Column1Width;
				existingSave.Column3Width = _grid.Column3Width;
				existingSave.Column5Width = _grid.Column5Width;
				existingSave.Row1Height = _grid.Row1Height;
				existingSave.Row3Height = _grid.Row3Height;
				existingSave.Row5Height = _grid.Row5Height;
			}
			// Doesn't exist, create new save.
			else
			{
				existingSaves.Add(new ModuleStateSave
				{
					Name = _name,
					LeftSelection = _grid.LeftSelection,
					RightSelection = _grid.RightSelection,
					TopSelection = _grid.TopSelection,
					MiddleSelection = _grid.MiddleSelection,
					BottomSelection = _grid.BottomSelection,
					Column1Width = _grid.Column1Width,
					Column3Width = _grid.Column3Width,
					Column5Width = _grid.Column5Width,
					Row1Height = _grid.Row1Height,
					Row3Height = _grid.Row3Height,
					Row5Height = _grid.Row5Height
				});
			}

			// Overwrite file.
			try
			{
				var fileWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory +
					Settings.Default.ModuleStateSaveFileName);
				serializer.Serialize(fileWriter, existingSaves.OrderBy(o => o.Name).ToList());
				fileWriter.Close();

				_controlCenter.ReloadModuleButtonContexts();
				//Write to console name + " has been saved."
			}
			catch (Exception ex)
			{
                _controlCenter.Console.WriteToConsole(ex.Message);
			}
		}
	}
}
