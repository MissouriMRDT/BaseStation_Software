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
        public enum DeviceType
        {
            Keyboard,
            XboxController,
            FlightStick
        };

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

        public InputManagerViewModel(ControlCenterViewModel cc)
        {
            // Set default input device as the keyboard
            Input = new KeyboardInputViewModel(cc);
            _cc = cc;
            //Input = new XboxControllerInputViewModel(cc);
        }

        public void SwitchDevice(DeviceType newDevice)
        {
            // Delete old input
            Input = null;

            // Switch on newDevice
            switch (newDevice)
            {
                case DeviceType.Keyboard:
                    Input = new KeyboardInputViewModel(_cc);
                    break;
                case DeviceType.XboxController:
                    Input = new XboxControllerInputViewModel(_cc);
                    break;
                //case DeviceType.FlightStick:
                //    Input = new FlightStickInputViewModel(_cc);
                //    break;
            }
        }

        public async void Start()
        {
            DeviceType oldDevice, selectedDevice;
            oldDevice = DeviceType.Keyboard;
            Input.Start();
            while (true)
            {
                /*
                 * TODO: get the selected device from the view,
                 *       and check if it's different from the device
                 *       that we're currently using.
                 *       If it is, call the switch device using the newly
                 *       selected device.
                 *
                 *       NOTE: But like seriously, this is an infinite loop,
                 *       so if it isn't working, it needs to be deleted.
                 *       
                 * selectedDevice = // get this from the view
                 * // Call the switch controller function here
                 * if (oldDevice != selectedDevice)
                 * {
                 *     SwitchDevice(selectedDevice);
                 *     oldDevice = selectedDevice;
                 * }
                **/

                await Task.Delay(1000);
            }
        }
    }
}
