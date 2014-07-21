namespace RED.ViewModels.ControlCenter
{
	using System.Configuration;
	using Addons;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Windows.Input;
	using System.Xml.Serialization;
	using Properties;
	using SharpDX;

	public class SaveModuleStateVm : BaseViewModel
	{
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
		public SaveModuleStateVm(ControlCenterViewModel controlCenter)
		{
			_controlCenter = controlCenter;
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
				existingSave.LeftSelection = _controlCenter.LeftSelection;
				existingSave.RightSelection = _controlCenter.RightSelection;
				existingSave.TopSelection = _controlCenter.TopSelection;
				existingSave.MiddleSelection = _controlCenter.MiddleSelection;
				existingSave.BottomSelection = _controlCenter.BottomSelection;
				existingSave.Column1Width = _controlCenter.Column1Width;
				existingSave.Column3Width = _controlCenter.Column3Width;
				existingSave.Column5Width = _controlCenter.Column5Width;
				existingSave.Row1Height = _controlCenter.Row1Height;
				existingSave.Row3Height = _controlCenter.Row3Height;
				existingSave.Row5Height = _controlCenter.Row5Height;
			}
			// Doesn't exist, create new save.
			else
			{
				existingSaves.Add(new ModuleStateSave
				{
					Name = _name,
					LeftSelection = _controlCenter.LeftSelection,
					RightSelection = _controlCenter.RightSelection,
					TopSelection = _controlCenter.TopSelection,
					MiddleSelection = _controlCenter.MiddleSelection,
					BottomSelection = _controlCenter.BottomSelection,
					Column1Width = _controlCenter.Column1Width,
					Column3Width = _controlCenter.Column3Width,
					Column5Width = _controlCenter.Column5Width,
					Row1Height = _controlCenter.Row1Height,
					Row3Height = _controlCenter.Row3Height,
					Row5Height = _controlCenter.Row5Height
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
