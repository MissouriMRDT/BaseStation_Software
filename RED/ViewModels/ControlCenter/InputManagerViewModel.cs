using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces;
using RED.Models;
using Caliburn.Micro;

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
                Input.EvaluateCurrentMode();

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
        }
    }
}
