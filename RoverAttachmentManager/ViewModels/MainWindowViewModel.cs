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

namespace RoverAttachmentManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;
        private XboxControllerInputViewModel XboxController1;
        private XboxControllerInputViewModel XboxController2;
        private XboxControllerInputViewModel XboxController3;
        private XboxControllerInputViewModel XboxController4;
        private FlightStickViewModel FlightStickController;
        private KeyboardInputViewModel KeyboardController;

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
        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Attachment Manager";
            _model = new MainWindowModel();

            Console = CommonLog.Instance;
            ConfigManager = new XMLConfigManager(Console);
            MetadataManager = new MetadataManager(Console, ConfigManager);
            
            Rovecomm = Rovecomm.Instance;
            ResubscribeAll();

            XboxController1 = new XboxControllerInputViewModel(1);
            XboxController2 = new XboxControllerInputViewModel(2);
            XboxController3 = new XboxControllerInputViewModel(3);
            XboxController4 = new XboxControllerInputViewModel(4);
            FlightStickController = new FlightStickViewModel();
            KeyboardController = new KeyboardInputViewModel();

            Arm = new ArmViewModel(Rovecomm, MetadataManager, Console, ConfigManager);

            InputManager = new InputManagerViewModel(Console, ConfigManager,
                new IInputDevice[] { XboxController1, XboxController2, XboxController3, XboxController4, FlightStickController, KeyboardController },
                new MappingViewModel[0],
                new IInputMode[] {Arm});



        }
        public void ResubscribeAll()
        {
            Rovecomm.SubscribeMyPCToAllDevices();
        }
    }
}