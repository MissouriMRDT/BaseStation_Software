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

        private int _defaultSerialReadSpeed = 30;
        public int DefaultSerialReadSpeed
        {
            get
            {
                return _defaultSerialReadSpeed;
            }
            set
            {
                _defaultSerialReadSpeed = value;
                NotifyOfPropertyChange(() => DefaultSerialReadSpeed);
            }
        }

        public InputManagerViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log, StateViewModel state)
        {

        }

        public void Start()
        {

        }
    }
}
