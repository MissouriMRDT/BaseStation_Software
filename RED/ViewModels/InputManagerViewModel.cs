using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels.ControlCenter;

namespace RED.ViewModels
{
    public class InputManagerViewModel
    {
        enum DeviceType
        {
            Keyboard,
            XboxController,
            FlightStick
        };

        private IInputDevice Input;

        public InputManagerViewModel(ControlCenterViewModel cc)
        {
            // Set default input device as the keyboard
            Input = new KeyboardInputViewModel(cc);
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
    }
}
