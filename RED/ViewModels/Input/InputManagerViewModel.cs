using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input;
using RED.ViewModels.Input.Controllers;

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

        public InputManagerViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log, StateViewModel state)
        {
            // Set default input device as the keyboard
            //Input = new KeyboardInputViewModel(router, idResolver, log, state);
            Input = new XboxControllerInputViewModel(router, idResolver, log, state);
        }

        public void SwitchDevice(DeviceType newDevice, IDataRouter router, IDataIdResolver idResolver, ILogger log, StateViewModel state)
        {
            // Delete old input
            Input = null;

            // Switch on newDevice
            switch (newDevice)
            {
                case DeviceType.Keyboard:
                    Input = new KeyboardInputViewModel(router, idResolver, log, state);
                    break;
                case DeviceType.XboxController:
                    Input = new XboxControllerInputViewModel(router, idResolver, log, state);
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
