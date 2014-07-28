namespace RED.ViewModels.ControlCenter
{
    using Addons;
    using Contexts;
    using FirstFloor.ModernUI.Presentation;
    using Interfaces;
    using Models.ControlCenter;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using System.Xml.Serialization;

    public class ControlCenterVM : BaseVM
    {
        internal static readonly ConsoleVM ConsoleVM = new ConsoleVM();
        internal static readonly StateVM StateVM = new StateVM();
        internal static ModuleManagerVM ModuleManagerVM { get; set; }
        internal static SaveModuleStateVM SaveModuleStateVM;
        internal static RemoveModuleStateVM RemoveModuleStateVM;

        // Don't move this model.  Execution order is vital.
        internal static readonly ControlCenterModel Model = new ControlCenterModel();

        private static LexiconEngine LexiconEngine { get; set; }

        public static IEnumerable<IModule> ManagedModules
        {
            get
            {
                return ControlCenterModel.AllModules.Where(m => m.IsManageable);
            }
        }
        public static IEnumerable<IModule> UnmanagedModules
        {
            get
            {
                return ControlCenterModel.AllModules.Where(m => !m.IsManageable);
            }
        }

        public string LeftSelection
        {
            get
            {
                return Model.LeftSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.LeftSelection = null;
                else
                {
                    var module = ManagedModules.Single(t => t.Title == value);
                    if (module.InUse)
                        ModuleManagerVM.RemoveModule(module);
                    Model.LeftSelection = value;
                    LeftModule = module;
                }
                RaisePropertyChanged("LeftSelection");
            }
        }
        public string RightSelection
        {
            get
            {
                return Model.RightSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.RightSelection = null;
                else
                {
                    var module = ManagedModules.Single(t => t.Title == value);
                    if (module.InUse)
                        ModuleManagerVM.RemoveModule(module);
                    Model.RightSelection = value;
                    RightModule = module;
                }
                RaisePropertyChanged("RightSelection");
            }
        }
        public string TopSelection
        {
            get
            {
                return Model.TopSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.TopSelection = null;
                else
                {
                    var module = ManagedModules.Single(t => t.Title == value);
                    if (module.InUse)
                        ModuleManagerVM.RemoveModule(module);
                    Model.TopSelection = value;
                    TopModule = module;
                }
                RaisePropertyChanged("TopSelection");
            }
        }
        public string MiddleSelection
        {
            get
            {
                return Model.MiddleSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.MiddleSelection = null;
                else
                {
                    var module = ManagedModules.Single(t => t.Title == value);
                    if (module.InUse)
                        ModuleManagerVM.RemoveModule(module);
                    Model.MiddleSelection = value;
                    MiddleModule = module;
                }
                RaisePropertyChanged("MiddleSelection");
            }
        }
        public string BottomSelection
        {
            get
            {
                return Model.BottomSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.BottomSelection = null;
                else
                {
                    var module = ManagedModules.Single(t => t.Title == value);
                    if (module.InUse)
                        ModuleManagerVM.RemoveModule(module);
                    Model.BottomSelection = value;
                    BottomModule = module;
                }
                RaisePropertyChanged("BottomSelection");
            }
        }

        public IModule LeftModule
        {
            get
            {
                return Model.LeftModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                SetField(ref Model.LeftModule, value);
            }
        }
        public IModule RightModule
        {
            get
            {
                return Model.RightModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                SetField(ref Model.RightModule, value);
            }
        }
        public IModule TopModule
        {
            get
            {
                return Model.TopModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                SetField(ref Model.TopModule, value);
            }
        }
        public IModule MiddleModule
        {
            get
            {
                return Model.MiddleModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                SetField(ref Model.MiddleModule, value);
            }
        }
        public IModule BottomModule
        {
            get
            {
                return Model.BottomModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                SetField(ref Model.BottomModule, value);
            }
        }

        public IModule SidePanelTopModule
        {
            get
            {
                return Model.SidePanelTopModule;
            }
            set
            {
                SetField(ref Model.SidePanelTopModule, value);
            }
        }
        public IModule SidePanelMiddleModule
        {
            get
            {
                return Model.SidePanelMiddleModule;
            }
            set
            {
                SetField(ref Model.SidePanelMiddleModule, value);
            }
        }
        public IModule SidePanelBottomModule
        {
            get
            {
                return Model.SidePanelBottomModule;
            }
            set
            {
                SetField(ref Model.SidePanelBottomModule, value);
            }
        }

        public string Column1Width
        {
            get { return Model.Column1Width; }
            set { SetField(ref Model.Column1Width, value);}
        }
        public string Column3Width
        {
            get { return Model.Column3Width; }
            set { SetField(ref Model.Column3Width, value); }
        }
        public string Column5Width
        {
            get { return Model.Column5Width; }
            set { SetField(ref Model.Column5Width, value); }
        }
        public string Row1Height
        {
            get { return Model.Row1Height; }
            set { SetField(ref Model.Row1Height, value); }
        }
        public string Row3Height
        {
            get { return Model.Row3Height; }
            set { SetField(ref Model.Row3Height, value); }
        }
        public string Row5Height
        {
            get { return Model.Row5Height; }
            set { SetField(ref Model.Row5Height, value); }
        }

        public ControlCenterVM()
        {
            RemoveModuleStateVM = new RemoveModuleStateVM(this);
            SaveModuleStateVM = new SaveModuleStateVM(this);

            // Populate the side panel on the right
            SidePanelTopModule = StateVM;
            SidePanelMiddleModule = ConsoleVM;
            // Initialize the Module Manager - IoC
            ModuleManagerVM = new ModuleManagerVM(this);
            SidePanelBottomModule = ModuleManagerVM;

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
                return ControlCenterModel.ButtonContexts;
            }
        }

        public void ReloadModuleButtonContexts()
        {
            var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));

            // Get existing saves if there are any.
            try
            {
                ButtonContexts.Clear();
                var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + SavesFileName);
                var existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
                fileReader.Close();
                foreach (var save in existingSaves)
                {
                    var name = save.Name;
                    ButtonContexts.Add(new ButtonContext(new RelayCommand(o => LoadModuleSave(name)), name));
                }
            }
            catch (Exception) { }
        }
        public void LoadModuleSave(string name)
        {
            var serializer = new XmlSerializer(typeof(List<ModuleStateSave>));
            var fileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + SavesFileName);
            var existingSaves = (List<ModuleStateSave>)serializer.Deserialize(fileReader);
            var save = existingSaves.Single(s => s.Name == name);
            
            ModuleManagerVM.ClearModules();
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

        public static void UpdateConsole(string msg)
        {
            ConsoleVM.TelemetryReceiver<object>(msg);
        }
    }
}