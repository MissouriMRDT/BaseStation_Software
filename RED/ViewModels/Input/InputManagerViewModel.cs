using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces.Input;
using RED.Models.Input;
using RED.ViewModels.Input.Controllers;
using Caliburn.Micro;

namespace RED.ViewModels.Input
{
    public class InputManagerViewModel : Screen
    {
        public enum DeviceType
        {
            Keyboard,
            XboxController,
            FlightStick
        };

        InputManagerModel _model = new InputManagerModel();

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

        public InputManagerViewModel(ControlCenterViewModel cc)
        {
            // Set default input device as the keyboard
            //Input = new KeyboardInputViewModel(cc);
            Input = new XboxControllerInputViewModel(cc);
        }

        public void SwitchDevice(DeviceType newDevice, ControlCenterViewModel cc)
        {
            // Delete old input
            Input = null;

            // Switch on newDevice
            switch (newDevice)
            {
                case DeviceType.Keyboard:
                    Input = new KeyboardInputViewModel(cc);
                    break;
                case DeviceType.XboxController:
                    Input = new XboxControllerInputViewModel(cc);
                    break;
                //case DeviceType.FlightStick:
                //    Input = new FlightStickInputViewModel(cc);
                //    break;
            }
        }

        public void Start()
        {
            Input.Start();
        }
    }
}
