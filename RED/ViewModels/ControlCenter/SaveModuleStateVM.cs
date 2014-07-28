namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using FirstFloor.ModernUI.Presentation;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using System.Xml.Serialization;

    public class SaveModuleStateVM : BaseVM
    {
        private readonly ControlCenterVM controlCenterVM;

        private string name = string.Empty;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetField(ref name, value);
            }
        }

        public ICommand SaveStateCommand { get; set; }

        public SaveModuleStateVM()
        {

        }
        public SaveModuleStateVM(ControlCenterVM controlCenterVM)
        {
            this.controlCenterVM = controlCenterVM;
            SaveStateCommand = new RelayCommand(c => Save(), b => Name.Length > 0);
        }

        public void Save()
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

            // Save already exists, update values state.
            if (existingSaves.Exists(s => s.Name == name))
            {
                var existingSave = existingSaves.Single(s => s.Name == name);
                existingSave.LeftSelection = ControlCenterVM.Model.LeftSelection;
                existingSave.RightSelection = ControlCenterVM.Model.RightSelection;
                existingSave.TopSelection = ControlCenterVM.Model.TopSelection;
                existingSave.MiddleSelection = ControlCenterVM.Model.MiddleSelection;
                existingSave.BottomSelection = ControlCenterVM.Model.BottomSelection;
                existingSave.Column1Width = ControlCenterVM.Model.Column1Width;
                existingSave.Column3Width = ControlCenterVM.Model.Column3Width;
                existingSave.Column5Width = ControlCenterVM.Model.Column5Width;
                existingSave.Row1Height = ControlCenterVM.Model.Row1Height;
                existingSave.Row3Height = ControlCenterVM.Model.Row3Height;
                existingSave.Row5Height = ControlCenterVM.Model.Row5Height;
            }
            // Doesn't exist, create new save.
            else
            {
                existingSaves.Add(new ModuleStateSave
                {
                    Name = name,
                    LeftSelection = ControlCenterVM.Model.LeftSelection,
                    RightSelection = ControlCenterVM.Model.RightSelection,
                    TopSelection = ControlCenterVM.Model.TopSelection,
                    MiddleSelection = ControlCenterVM.Model.MiddleSelection,
                    BottomSelection = ControlCenterVM.Model.BottomSelection,
                    Column1Width = ControlCenterVM.Model.Column1Width,
                    Column3Width = ControlCenterVM.Model.Column3Width,
                    Column5Width = ControlCenterVM.Model.Column5Width,
                    Row1Height = ControlCenterVM.Model.Row1Height,
                    Row3Height = ControlCenterVM.Model.Row3Height,
                    Row5Height = ControlCenterVM.Model.Row5Height
                });
            }

            // Overwrite file.
            try
            {
                var fileWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + SavesFileName);
                serializer.Serialize(fileWriter, existingSaves.OrderBy(o => o.Name).ToList());
                fileWriter.Close();

                controlCenterVM.ReloadModuleButtonContexts();
                ControlCenterVM.ConsoleVM.TelemetryReceiver<object>(name + " has been saved.");
            }
            catch (Exception)
            {
                ControlCenterVM.ConsoleVM.TelemetryReceiver<object>(name + " failed to be saved. Please try again.");
            }
        }
    }
}
