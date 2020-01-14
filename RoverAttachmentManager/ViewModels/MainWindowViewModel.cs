using Caliburn.Micro;
using Core;
using Core.Configurations;
using Core.Interfaces;
using Core.Interfaces.Input;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
using RoverAttachmentManager.Models;
using RoverAttachmentManager.ViewModels.Arm;
using System;
using RoverAttachmentManager.ViewModels.Autonomy;
using RoverAttachmentManager.ViewModels.Science;

namespace RoverAttachmentManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

        public override void CanClose(Action<bool> callback)
        {
            callback(false);
        }

        public ArmViewModel Arm
        {
            get
            {
                return _model._arm;
            }
            set
            {
                _model._arm = value;
                NotifyOfPropertyChange(() => Arm);
            }
        }

        public Rovecomm Rovecomm
        {
            get
            {
                return _model._rovecomm;
            }
            set
            {
                _model._rovecomm = value;
                NotifyOfPropertyChange((() => Rovecomm));
            }
        }

        public CommonLog Console
        {
            get
            {
                return _model._console;
            }
            set
            {
                _model._console = value;
                NotifyOfPropertyChange();
            }
        }
        public InputManagerViewModel InputManager
        {
            get
            {
                return _model._input;
            }
            set
            {
                _model._input = value;
                NotifyOfPropertyChange(() => InputManager);
            }
        }
        public MetadataManager MetadataManager
        {
            get
            {
                return _model._metadataManager;
            }
            set
            {
                _model._metadataManager = value;
                NotifyOfPropertyChange(() => MetadataManager);
            }
        }

        public XMLConfigManager ConfigManager
        {
            get
            {
                return _model._configManager;
            }
            set
            {
                _model._configManager = value;
                NotifyOfPropertyChange();
            }
        }
        public XboxControllerInputViewModel XboxController1
        {
            get
            {
                return _model._xboxController1;
            }
            set
            {
                _model._xboxController1 = value;
                NotifyOfPropertyChange(() => XboxController1);
            }
        }
        public XboxControllerInputViewModel XboxController2
        {
            get
            {
                return _model._xboxController2;
            }
            set
            {
                _model._xboxController2 = value;
                NotifyOfPropertyChange(() => XboxController2);
            }
        }
        public XboxControllerInputViewModel XboxController3
        {
            get
            {
                return _model._xboxController3;
            }
            set
            {
                _model._xboxController3 = value;
                NotifyOfPropertyChange(() => XboxController3);
            }
        }
        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Attachment Manager";
            _model = new MainWindowModel();

            Console = CommonLog.Instance;
            ConfigManager = new XMLConfigManager(Console);
            MetadataManager = new MetadataManager(Console, ConfigManager);
            
            Rovecomm = Rovecomm.Instance;
            ResubscribeAll();
          

            Arm = new ArmViewModel(Rovecomm, MetadataManager, Console, ConfigManager);
            Autonomy = new AutonomyViewModel(Rovecomm, MetadataManager, Console);
            Science = new ScienceViewModel(Rovecomm, MetadataManager, Console, ConfigManager);

        }

        public void ResubscribeAll()
        {
            Rovecomm.SubscribeMyPCToAllDevices();
        }

        public AutonomyViewModel Autonomy
        {
            get
            {
                return _model._autonomy;
            }
            set
            {
                _model._autonomy = value;
                NotifyOfPropertyChange(() => Autonomy);
            }
        }

        public ScienceViewModel Science
        {
            get
            {
                return _model._science;
            }
            set
            {
                _model._science = value;
                NotifyOfPropertyChange(() => Science);
            }
        }
    }
}