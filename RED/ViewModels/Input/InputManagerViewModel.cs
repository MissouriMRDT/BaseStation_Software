using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input;
using RED.ViewModels.Input.Controllers;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RED.ViewModels.Input
{
    public class InputManagerViewModel : PropertyChangedBase
    {
        InputManagerModel _model;
        ILogger _log;
        IConfigurationManager _configManager;

        private const string MappingsConfigName = "InputMappings";
        private const string SelectionsConfigName = "InputSelections";

        public ObservableCollection<IInputDevice> Devices
        {
            get
            {
                return _model.Devices;
            }
            private set
            {
                _model.Devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }
        public ObservableCollection<MappingViewModel> Mappings
        {
            get
            {
                return _model.Mappings;
            }
            private set
            {
                _model.Mappings = value;
                NotifyOfPropertyChange(() => Mappings);
            }
        }
        public ObservableCollection<IInputMode> Modes
        {
            get
            {
                return _model.Modes;
            }
            private set
            {
                _model.Modes = value;
                NotifyOfPropertyChange(() => Modes);
            }
        }

        public ObservableCollection<InputSelectorViewModel> Selectors
        {
            get
            {
                return _model.Selectors;
            }
            private set
            {
                _model.Selectors = value;
                NotifyOfPropertyChange(() => Selectors);
            }
        }

        public InputManagerViewModel(ILogger log, IConfigurationManager configs, IInputDevice[] devices, MappingViewModel[] mappings, IInputMode[] modes)
        {
            _model = new InputManagerModel();
            _log = log;
            _configManager = configs;

            Devices = new ObservableCollection<IInputDevice>(devices);
            Mappings = new ObservableCollection<MappingViewModel>(mappings);
            Modes = new ObservableCollection<IInputMode>(modes);
            Selectors = new ObservableCollection<InputSelectorViewModel>();

            _configManager.AddRecord(MappingsConfigName, DefaultInputMappings);
            _configManager.AddRecord(SelectionsConfigName, DefaultInputSelections);

            InitializeMappings(_configManager.GetConfig<InputMappingsContext>(MappingsConfigName));
            InitializeSelections(_configManager.GetConfig<InputSelectionsContext>(SelectionsConfigName));
        }

        private void SelectorSwitchDevice(object sender, IInputDevice device)
        {
            foreach (var selector in Selectors.Where(x => x.SelectedDevice == device && x != sender))
                selector.Stop();
        }
        private void SelectorCycleMode(object sender, IInputDevice device)
        {
            Selectors.Concat(Selectors).SkipWhile(x => x != sender).Skip(1).First(x => x.SelectedDevice == device).Start();
        }

        public void Start()
        {
        }

        public void Stop()
        {
            foreach (var selector in Selectors)
                selector.Disable();
        }

        public void AddDevice(IInputDevice device)
        {
            Devices.Add(device);
        }

        public void SaveConfigurations()
        {
            _configManager.SetConfig(MappingsConfigName, new InputMappingsContext(Mappings.Select(x => x.ToContext()).ToArray()));
            _configManager.SetConfig(SelectionsConfigName, new InputSelectionsContext(Selectors.Select(x => x.GetContext()).ToArray()));
        }

        private void InitializeMappings(InputMappingsContext config)
        {
            foreach (InputMappingContext mapping in config.Mappings)
                Mappings.Add(new MappingViewModel(mapping));

            _log.Log("Input Mappings loaded");
        }

        private void InitializeSelections(InputSelectionsContext config)
        {
            foreach (IInputMode mode in Modes)
                Selectors.Add(new InputSelectorViewModel(_log, mode, Devices, Mappings));

            foreach (var selector in Selectors)
            {
                selector.SwitchDevice += SelectorSwitchDevice;
                selector.CycleMode += SelectorCycleMode;
            }

            foreach (InputSelectionContext selection in config.Selections)
                foreach (var selector in Selectors.Where(x => x.Mode.Name == selection.ModeName))
                    selector.SetContext(selection);
        }

        public static InputMappingsContext DefaultInputMappings = new InputMappingsContext(new[] {
            new InputMappingContext("Tank Drive (Traditional)", "Xbox", "Drive", 30, new[] { 
                new InputChannelContext("JoyStick1Y", "WheelsLeft"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WheelsRight"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Diagonal Drive", "FlightStick", "Drive", 30, new[] { 
                new InputChannelContext("X", "WheelsLeft"){ Parabolic = true },
                new InputChannelContext("Y", "WheelsRight"){ Parabolic = true },
                new InputChannelContext("Button7Debounced", "ModeCycle") }),
            new InputMappingContext("Vector Drive", "FlightStick", "Drive", 30, new[] { 
                new InputChannelContext("X", "VectorX"),
                new InputChannelContext("Y", "VectorY"),
                new InputChannelContext("Slider0", "Throttle"),
                new InputChannelContext("Button7Debounced", "ModeCycle") }),
            new InputMappingContext("Arm (Traditional)", "Xbox", "Arm", 200, new[] { 
                new InputChannelContext("JoyStick1Y", "ElbowBend"){ Parabolic = true },
                new InputChannelContext("JoyStick1X", "ElbowTwist"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WristTwist"){ Parabolic = true, LinearScaling = -1 },
                new InputChannelContext("JoyStick2X", "WristBend"){ Parabolic = true },
                new InputChannelContext("DPadU", "ShoulderBendForward"),
                new InputChannelContext("DPadD", "ShoulderBendBackward"),
                new InputChannelContext("DPadR", "ShoulderTwistForward"),
                new InputChannelContext("DPadL", "ShoulderTwistBackward"),
                new InputChannelContext("ButtonY", "DebouncedArmReset"),
                new InputChannelContext("LeftTrigger", "GripperOpen"),
                new InputChannelContext("RightTrigger", "GripperClose"),
                new InputChannelContext("ButtonRb", "ServoClockwise"),
                new InputChannelContext("ButtonLb", "ServoCounterClockwise"),
                new InputChannelContext("ButtonB", "TowRopeOut"),
                new InputChannelContext("ButtonX", "TowRopeIn"),
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Science Arm", "Xbox", "ScienceArm", 30, new[] { 
                new InputChannelContext("JoyStick1Y", "Arm"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "Drill"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Gimbal", "Xbox", "Gimbal", 30, new[] { 
                new InputChannelContext("JoyStick1X", "Pan"){ Parabolic = true },
                new InputChannelContext("JoyStick1Y", "Tilt"){ Parabolic = true },
                new InputChannelContext("ButtonY", "ZoomIn"){ Parabolic = true },
                new InputChannelContext("ButtonA", "ZoomOut"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Key Drive", "Keyboard", "Drive", 30, new[] {
                new InputChannelContext("WS", "WheelsLeft"),
                new InputChannelContext("IK", "WheelsRight"),
                new InputChannelContext("RDebounced", "ModeCycle") }),
            new InputMappingContext("Key Gimbal", "Keyboard", "Gimbal", 30, new[] { 
                new InputChannelContext("AD", "Pan"),
                new InputChannelContext("WS", "Tilt"),
                new InputChannelContext("I", "ZoomIn"),
                new InputChannelContext("K", "ZoomOut"),
                new InputChannelContext("RDebounced", "ModeCycle") }),
        });

        public static InputSelectionsContext DefaultInputSelections = new InputSelectionsContext(new[] {
            new InputSelectionContext("Drive", "Xbox 1", "Tank Drive (Traditional)", true),
            new InputSelectionContext("Arm", "Xbox 1", "Arm (Traditional)", false),
            new InputSelectionContext("Gimbal 1", "Xbox 1", "Xbox Gimbal",false),
            new InputSelectionContext("Gimbal 2", "Xbox 1", "Xbox Gimbal",false),
            new InputSelectionContext("Science Arm", "Xbox 1", "Xbox Science Arm", false)
        });
    }
}
