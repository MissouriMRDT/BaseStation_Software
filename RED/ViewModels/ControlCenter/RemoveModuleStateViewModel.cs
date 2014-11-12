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
        private readonly ControlCenterViewModel _controlCenter;

        private string _selectedItem = string.Empty;
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
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
                catch (Exception e)
                {
                    _controlCenter.Console.WriteToConsole(e.Message);
                }
                return existingSaves;
            }
        }

        public RelayCommand RemoveStateCommand { get; set; }

        public RemoveModuleStateViewModel()
        {
            
        }
        public RemoveModuleStateViewModel(ControlCenterViewModel controlCenter)
        {
            _controlCenter = controlCenter;
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
                existingSaves = (List<ModuleStateSave>) serializer.Deserialize(fileReader);
                fileReader.Close();
            }
            catch (Exception e)
            {
                _controlCenter.Console.WriteToConsole(e.Message);
            }

            // Remove existing save.
            existingSaves.Remove(existingSaves.Single(s => s.Name == _selectedItem));

            // Overwrite file.
            var fileWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory +
                Settings.Default.ModuleStateSaveFileName);
            serializer.Serialize(fileWriter, existingSaves);

            fileWriter.Close();

            _controlCenter.GridManager.ReloadModuleButtonContexts();

            //Write to console "'" + selectedItem + "' removed."
        }
    }
}
