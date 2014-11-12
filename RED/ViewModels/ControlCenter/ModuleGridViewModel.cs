namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ModuleGridViewModel : PropertyChangedBase
    {
        private readonly ModuleGridModel _model;

        public string LeftSelection
        {
            get
            {
                return _model._leftSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model._leftSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        _moduleManager.RemoveModule(module);
                    _model._leftSelection = value;
                    _model._leftModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string RightSelection
        {
            get
            {
                return _model._rightSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model._rightSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        _moduleManager.RemoveModule(module);
                    _model._rightSelection = value;
                    _model._rightModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string TopSelection
        {
            get
            {
                return _model._topSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model._topSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        _moduleManager.RemoveModule(module);
                    _model._topSelection = value;
                    _model._topModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string MiddleSelection
        {
            get
            {
                return _model._middleSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model._middleSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        _moduleManager.RemoveModule(module);
                    _model._middleSelection = value;
                    _model._middleModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string BottomSelection
        {
            get
            {
                return _model._bottomSelection;
            }
            set
            {
                if (value == String.Empty)
                    _model._bottomSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        _moduleManager.RemoveModule(module);
                    _model._bottomSelection = value;
                    _model._bottomModule = module;
                }
                NotifyOfPropertyChange();
            }
        }

        public IModule LeftModule
        {
            get
            {
                return _model._leftModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model._leftModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule RightModule
        {
            get
            {
                return _model._rightModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model._rightModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule TopModule
        {
            get
            {
                return _model._topModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model._topModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule MiddleModule
        {
            get
            {
                return _model._middleModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model._middleModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule BottomModule
        {
            get
            {
                return _model._bottomModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _model._bottomModule = value;
                NotifyOfPropertyChange();
            }
        }

        public string Column1Width
        {
            get { return _model._column1Width; }
            set
            {
                _model._column1Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column3Width
        {
            get { return _model._column3Width; }
            set
            {
                _model._column3Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column5Width
        {
            get { return _model._column5Width; }
            set
            {
                _model._column5Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row1Height
        {
            get { return _model._row1Height; }
            set
            {
                _model._row1Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row3Height
        {
            get { return _model._row3Height; }
            set
            {
                _model._row3Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row5Height
        {
            get { return _model._row5Height; }
            set
            {
                _model._row5Height = value;
                NotifyOfPropertyChange();
            }
        }

        private readonly ModuleManagerViewModel _moduleManager;
        public IEnumerable<IModule> Modules { get; set; }

        public ModuleGridViewModel(ModuleManagerViewModel manager)
        {
            _model = new ModuleGridModel();
            _moduleManager = manager;
            Modules = new List<IModule>();
        }
    }
}
