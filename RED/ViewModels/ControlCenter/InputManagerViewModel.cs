using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces;
using RED.Models;
using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace RED.ViewModels.ControlCenter
{
    public class InputManagerViewModel : Screen
    {
        InputManagerModel _model = new InputManagerModel();
        ControlCenterViewModel _cc;

        public IInputDevice Input
        {
            get
            {
                return _model._input;
            }
            set
            {
                _model._input = value;
                NotifyOfPropertyChange(() => Input);
            }
        }

        public int SerialReadSpeed
        {
            get
            {
                return _model.SerialReadSpeed;
            }
            set
            {
                _model.SerialReadSpeed = value;
                NotifyOfPropertyChange(() => SerialReadSpeed);
            }
        }

        public string SelectedDevice
        {
            get
            {
                return _model.SelectedDevice;
            }

            set
            {
                _model.SelectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
            }
        }

        public ObservableCollection<IControllerMode> ControllerModes
        {
            get
            {
                return _model.ControllerModes;
            }
        }

        public int CurrentModeIndex
        {
            get
            {
                return _model.CurrentModeIndex;
            }
            private set
            {
                _model.CurrentModeIndex = value;
                NotifyOfPropertyChange(() => CurrentModeIndex);
                _cc.StateManager.CurrentControlMode = ControllerModes[CurrentModeIndex].Name;
            }
        }

        public void NextControlMode()
        {
            ControllerModes[CurrentModeIndex].ExitMode();
            CurrentModeIndex = (CurrentModeIndex + 1) % ControllerModes.Count;
            ControllerModes[CurrentModeIndex].EnterMode();
        }

        public void PreviousControlMode()
        {
            ControllerModes[CurrentModeIndex].ExitMode();
            CurrentModeIndex = (CurrentModeIndex - 1 + ControllerModes.Count) % ControllerModes.Count;
            ControllerModes[CurrentModeIndex].EnterMode();
        }

        public void EvaluateCurrentMode()
        {
            ControllerModes[CurrentModeIndex].EvaluateMode(Input);
        }

        public void SwitchDevice(string newDevice)
        {
            // Switch on newDevice
            switch (newDevice)
            {
                case "Keyboard":
                    Input = new KeyboardInputViewModel(_cc);
                    break;
                case "Xbox":
                    Input = new XboxControllerInputViewModel(_cc);
                    break;
                case "Joystick":
                    Input = new JoystickInputViewModel(_cc);
                    break;
                //case "FlightStick":
                //    Input = new FlightStickInputViewModel(_cc);
                //    break;
            }
        }

        public async void Start()
        {
            string oldDevice = "Keyboard";
            while (true)
            {
                Input.Update();

                if (Input.DebouncedModeNext)
                    NextControlMode();
                if (Input.DebouncedModePrev)
                    PreviousControlMode();

                EvaluateCurrentMode();

                // Check if the input device has changed
                if (oldDevice != SelectedDevice)
                {
                    SwitchDevice(SelectedDevice);
                    oldDevice = SelectedDevice;
                }

                await Task.Delay(SerialReadSpeed);
            }
        }

        public InputManagerViewModel(ControlCenterViewModel cc)
        {
            // Set default input device as the keyboard
            Input = new KeyboardInputViewModel(cc);
            _cc = cc;

            // Add Keyboard's default controller modes
            ControllerModes.Add(new DriveControllerModeViewModel(_cc));
            ControllerModes.Add(new ArmControllerModeViewModel(_cc));
            if (ControllerModes.Count == 0) throw new ArgumentException("IEnumerable 'modes' must have at least one item");
            CurrentModeIndex = 0;
        }
    }
}
