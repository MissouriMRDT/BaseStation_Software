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

        private string _leftSelection;
        private string _rightSelection;
        private string _topSelection;
        private string _middleSelection;
        private string _bottomSelection;
        private IModule _leftModule;
        private IModule _rightModule;
        private IModule _topModule;
        private IModule _middleModule;
        private IModule _bottomModule;
        private string _column1Width = "1*";
        private string _column3Width = "2*";
        private string _column5Width = "1*";
        private string _row1Height = "1*";
        private string _row3Height = "1*";
        private string _row5Height = "1*";

        public string LeftSelection
        {
            get
            {
                return _leftSelection;
            }
            set
            {
                if (value == String.Empty)
                    _leftSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _leftSelection = value;
                    _leftModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string RightSelection
        {
            get
            {
                return _rightSelection;
            }
            set
            {
                if (value == String.Empty)
                    _rightSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _rightSelection = value;
                    _rightModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string TopSelection
        {
            get
            {
                return _topSelection;
            }
            set
            {
                if (value == String.Empty)
                    _topSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _topSelection = value;
                    _topModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string MiddleSelection
        {
            get
            {
                return _middleSelection;
            }
            set
            {
                if (value == String.Empty)
                    _middleSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _middleSelection = value;
                    _middleModule = module;
                }
                NotifyOfPropertyChange();
            }
        }
        public string BottomSelection
        {
            get
            {
                return _bottomSelection;
            }
            set
            {
                if (value == String.Empty)
                    _bottomSelection = null;
                else
                {
                    var module = Modules.Single(t => t.Title == value);
                    if (module.InUse)
                        Manager.RemoveModule(module);
                    _bottomSelection = value;
                    _bottomModule = module;
                }
                NotifyOfPropertyChange();
            }
        }

        public IModule LeftModule
        {
            get
            {
                return _leftModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _leftModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule RightModule
        {
            get
            {
                return _rightModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _rightModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule TopModule
        {
            get
            {
                return _topModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _topModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule MiddleModule
        {
            get
            {
                return _middleModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _middleModule = value;
                NotifyOfPropertyChange();
            }
        }
        public IModule BottomModule
        {
            get
            {
                return _bottomModule;
            }
            set
            {
                if (value != null)
                    value.InUse = true;
                _bottomModule = value;
                NotifyOfPropertyChange();
            }
        }

        public string Column1Width
        {
            get { return _column1Width; }
            set
            {
                _column1Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column3Width
        {
            get { return _column3Width; }
            set
            {
                _column3Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Column5Width
        {
            get { return _column5Width; }
            set
            {
                _column5Width = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row1Height
        {
            get { return _row1Height; }
            set
            {
                _row1Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row3Height
        {
            get { return _row3Height; }
            set
            {
                _row3Height = value;
                NotifyOfPropertyChange();
            }
        }
        public string Row5Height
        {
            get { return _row5Height; }
            set
            {
                _row5Height = value;
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
