using Caliburn.Micro;
using RED.Interfaces.Input;
using RED.Roveprotocol;
using RED.ViewModels;
using RED.ViewModels.Input;
using RED.ViewModels.Input.Controllers;
using RED.ViewModels.Network;
using RoverAttachmentManager.Models;
using RoverAttachmentManager.ViewModels.ArmViewModels;

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

        public ConsoleViewModel Console
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
        public NetworkManagerViewModel NetworkManager
        {
            get
            {
                return _model._networkManager;
            }
            set
            {
                _model._networkManager = value;
                NotifyOfPropertyChange(() => NetworkManager);
            }
        }
        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Attachment Manager";
            _model = new MainWindowModel();

            Console = new ConsoleViewModel();
            ConfigManager = new XMLConfigManager(Console);
            MetadataManager = new MetadataManager(Console, ConfigManager);

            NetworkManager = new NetworkManagerViewModel(Console);
            Rovecomm = new Rovecomm(NetworkManager, Console, MetadataManager);
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