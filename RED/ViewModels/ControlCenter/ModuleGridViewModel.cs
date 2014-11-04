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
                return Model.leftSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.leftSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.leftSelection = value;
                    Model.leftModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string RightSelection
        {
            get
            {
                return Model.rightSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.rightSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.rightSelection = value;
                    Model.rightModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string TopSelection
        {
            get
            {
                return Model.topSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.topSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.topSelection = value;
                    Model.topModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string MiddleSelection
        {
            get
            {
                return Model.middleSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.middleSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.middleSelection = value;
                    Model.middleModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string BottomSelection
        {
            get
            {
                return Model.bottomSelection;
            }
            set
            {
                if (value == String.Empty)
                    Model.bottomSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    Model.bottomSelection = value;
                    Model.bottomModule = module;
                }
                NotifyOfPropertyChange();
            }
        }

        public IModule LeftModule
        {
            get
            {
                return Model.leftModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                Model.leftModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule RightModule
        {
            get
            {
                return Model.rightModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                Model.rightModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule TopModule
        {
            get
            {
                return Model.topModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                Model.topModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule MiddleModule
        {
            get
            {
                return Model.middleModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                Model.middleModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule BottomModule
        {
            get
            {
                return Model.bottomModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                Model.bottomModule = value;
                NotifyOfPropertyChange();
            }
        }

        public string Column1Width
        {
            get { return Model.column1Width; }
            set
            {
                Model.column1Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column3Width
        {
            get { return Model.column3Width; }
            set
            {
                Model.column3Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column5Width
        {
            get { return Model.column5Width; }
            set
            {
                Model.column5Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row1Height
        {
            get { return Model.row1Height; }
            set
            {
                Model.row1Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row3Height
        {
            get { return Model.row3Height; }
            set
            {
                Model.row3Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row5Height
        {
            get { return Model.row5Height; }
            set
            {
                Model.row5Height = value;
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
