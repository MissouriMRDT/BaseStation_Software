namespace RED.ViewModels.ControlCenter
{
	using Addons;
	using Caliburn.Micro;
	using Properties;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Serialization;

	public class RemoveModuleStateViewModel : PropertyChangedBase
	{
		private readonly ControlCenterViewModel controlCenterVM;

		private string selectedItem = string.Empty;
		public string SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				selectedItem = value;
				NotifyOfPropertyChange();
			}
		}
		public List<string> SavedModuleStates
		{
			get
			{
				var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));
				var existingSaves = new List<string>();
				try
				{
					var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +
						Settings.Default.ModuleStateSaveFileName);
					existingSaves = ((List<ModuleStateSave>)serializer.Deserialize(fileReader)).Select(s => s.Name).ToList();
					fileReader.Close();
				}
				catch (Exception)
				{
				}
				return existingSaves;
			}
		}

		public RelayCommand RemoveStateCommand { get; set; }

		public RemoveModuleStateViewModel()
		{
			
		}
		public RemoveModuleStateViewModel(ControlCenterViewModel controlCenterVM)
		{
			this.controlCenterVM = controlCenterVM;
			RemoveStateCommand = new RelayCommand(c => Remove());
		}
		
		private void Remove()
		{
			var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));

			// Get existing saves if there are any.
			var existingSaves = new List<ModuleStateSave>();
			try
			{
				var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +
					Settings.Default.ModuleStateSaveFileName);
				existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
				fileReader.Close();
			}
			catch (Exception) { }

			// Remove existing save.
			existingSaves.Remove(existingSaves.Single(s => s.Name == selectedItem));

			// Overwrite file.
			var fileWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory +
				Settings.Default.ModuleStateSaveFileName);
			serializer.Serialize(fileWriter, existingSaves);

			fileWriter.Close();

			controlCenterVM.ReloadModuleButtonContexts();

			//Write to console "'" + selectedItem + "' removed."
		}
	}
}
