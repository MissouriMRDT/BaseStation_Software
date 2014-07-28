namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using FirstFloor.ModernUI.Presentation;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class RemoveModuleStateVM : BaseVM
    {
        private readonly ControlCenterVM controlCenterVM;

        private string selectedItem = string.Empty;
        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetField(ref selectedItem, value);
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
                    var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + SavesFileName);
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

        public RemoveModuleStateVM()
        {

        }
        public RemoveModuleStateVM(ControlCenterVM controlCenterVM)
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
                var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + SavesFileName);
                existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
                fileReader.Close();
            }
            catch (Exception) { }

            // Remove existing save.
            existingSaves.Remove(existingSaves.Single(s => s.Name == selectedItem));

            // Overwrite file.
            var fileWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + SavesFileName);
            serializer.Serialize(fileWriter, existingSaves);

            fileWriter.Close();

            controlCenterVM.ReloadModuleButtonContexts();

            ControlCenterVM.ConsoleVM.TelemetryReceiver<object>("'" + selectedItem + "' removed.");
        }
    }
}
