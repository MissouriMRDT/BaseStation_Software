namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.ControlCenter;

    public class ModuleGridViewModel : PropertyChangedBase
    {
        private ModuleGridModel Model;

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
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.LeftSelection = value;
                    Model.LeftModule = module;
                }
                NotifyOfPropertyChange();
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
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.RightSelection = value;
                    Model.RightModule = module;
                }
                NotifyOfPropertyChange();
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
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.TopSelection = value;
                    Model.TopModule = module;
                }
                NotifyOfPropertyChange();
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
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.MiddleSelection = value;
                    Model.MiddleModule = module;
                }
                NotifyOfPropertyChange();
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
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.BottomSelection = value;
                    Model.BottomModule = module;
                }
                NotifyOfPropertyChange();
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
                Model.LeftModule = value;
                NotifyOfPropertyChange();
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
                Model.RightModule = value;
                NotifyOfPropertyChange();
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
                Model.TopModule = value;
                NotifyOfPropertyChange();
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
                Model.MiddleModule = value;
                NotifyOfPropertyChange();
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
                Model.BottomModule = value;
                NotifyOfPropertyChange();
            }
        }

        public string Column1Width
        {
            get { return Model.Column1Width; }
            set
            {
                Model.Column1Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column3Width
        {
            get { return Model.Column3Width; }
            set
            {
                Model.Column3Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column5Width
        {
            get { return Model.Column5Width; }
            set
            {
                Model.Column5Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row1Height
        {
            get { return Model.Row1Height; }
            set
            {
                Model.Row1Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row3Height
        {
            get { return Model.Row3Height; }
            set
            {
                Model.Row3Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row5Height
        {
            get { return Model.Row5Height; }
            set
            {
                Model.Row5Height = value;
                NotifyOfPropertyChange();
            }
        }

        public ModuleManagerViewModel Manager { get; set; }
        public IEnumerable<IModule> Modules { get; set; }

        public ModuleGridViewModel(ModuleManagerViewModel manager)
        {
            Model = new ModuleGridModel();
            Manager = manager;
            Modules = new List<IModule>();
        }
    }
}
