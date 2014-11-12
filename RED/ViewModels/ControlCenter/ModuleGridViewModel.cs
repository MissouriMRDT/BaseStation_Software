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
        private readonly ModuleGridModel _model;

        public string LeftSelection
        {
            get
            {
                return _model.LeftSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model.LeftSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _model.LeftSelection = value;
                    _model.LeftModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string RightSelection
        {
            get
            {
                return _model.RightSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model.RightSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _model.RightSelection = value;
                    _model.RightModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string TopSelection
        {
            get
            {
                return _model.TopSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model.TopSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _model.TopSelection = value;
                    _model.TopModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string MiddleSelection
        {
            get
            {
                return _model.MiddleSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model.MiddleSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _model.MiddleSelection = value;
                    _model.MiddleModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string BottomSelection
        {
            get
            {
                return _model.BottomSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model.BottomSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _model.BottomSelection = value;
                    _model.BottomModule = module;
                }
                NotifyOfPropertyChange();
            }
        }

        public IModule LeftModule
        {
            get
            {
                return _model.LeftModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model.LeftModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule RightModule
        {
            get
            {
                return _model.RightModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model.RightModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule TopModule
        {
            get
            {
                return _model.TopModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model.TopModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule MiddleModule
        {
            get
            {
                return _model.MiddleModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model.MiddleModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule BottomModule
        {
            get
            {
                return _model.BottomModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model.BottomModule = value;
                NotifyOfPropertyChange();
            }
        }

        public string Column1Width
        {
            get { return _model.Column1Width; }
            set
            {
                _model.Column1Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column3Width
        {
            get { return _model.Column3Width; }
            set
            {
                _model.Column3Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column5Width
        {
            get { return _model.Column5Width; }
            set
            {
                _model.Column5Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row1Height
        {
            get { return _model.Row1Height; }
            set
            {
                _model.Row1Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row3Height
        {
            get { return _model.Row3Height; }
            set
            {
                _model.Row3Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row5Height
        {
            get { return _model.Row5Height; }
            set
            {
                _model.Row5Height = value;
                NotifyOfPropertyChange();
            }
        }

        public ModuleManagerViewModel Manager { get; set; }
        public IEnumerable<IModule> Modules { get; set; }

        public ModuleGridViewModel(ModuleManagerViewModel manager)
        {
            _model = new ModuleGridModel();
            Manager = manager;
            Modules = new List<IModule>();
        }
    }
}
