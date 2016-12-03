using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input.Controllers;
using RED.ViewModels.Modules;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RED.ViewModels.Input.Controllers
{
    public class XboxControllerInputViewModel : PropertyChangedBase, IInputDevice
    {
        private readonly XboxControllerInputModel Model = new XboxControllerInputModel();
        private StateViewModel _state;
        public readonly Controller ControllerOne = new Controller(UserIndex.One);

        public string Name { get; private set; }
        public string DeviceType { get; private set; }

        public int SerialReadSpeed
        {
            get
            {
                return Model.SerialReadSpeed;
            }
            set
            {
                Model.SerialReadSpeed = value;
                NotifyOfPropertyChange(() => SerialReadSpeed);
            }
        }
        public bool AutoDeadzone
        {
            get
            {
                return Model.AutoDeadzone;
            }
            set
            {
                Model.AutoDeadzone = value;
                NotifyOfPropertyChange(() => AutoDeadzone);
            }
        }
        public int ManualDeadzone
        {
            get
            {
                return Model.ManualDeadzone;
            }
            set
            {
                Model.ManualDeadzone = value;
                NotifyOfPropertyChange(() => ManualDeadzone);
            }
        }

        public bool Connected
        {
            get
            {
                return Model.Connected;
            }
            set
            {
                Model.Connected = value;
                NotifyOfPropertyChange(() => Connected);
                _state.ControllerIsConnected = value;
            }
        }

        public XboxControllerInputViewModel(StateViewModel state)
        {
            _state = state;

            Name = "Xbox Controller 1";
            DeviceType = "Xbox";
        }

        public Dictionary<string, float> GetValues()
        {
            if (ControllerOne == null || !ControllerOne.IsConnected)
            {
                Connected = false;
                throw new Exception("Controller Disconnected");
            }
            Connected = true;

            var currentGamepad = ControllerOne.GetState().Gamepad;

            var deadzone = AutoDeadzone ? Math.Max(Gamepad.LeftThumbDeadZone, Gamepad.RightThumbDeadZone) : ManualDeadzone;

            return new Dictionary<string, float>()
            {
                {"JoyStick1X", currentGamepad.LeftThumbX < deadzone && currentGamepad.LeftThumbX > -deadzone ? 0 : ((currentGamepad.LeftThumbX + (currentGamepad.LeftThumbX < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone))},
                {"JoyStick1Y", currentGamepad.LeftThumbY < deadzone && currentGamepad.LeftThumbY > -deadzone ? 0 : ((currentGamepad.LeftThumbY + (currentGamepad.LeftThumbY < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone))},
                {"JoyStick2X", currentGamepad.RightThumbX < deadzone && currentGamepad.RightThumbX > -deadzone ? 0 : ((currentGamepad.RightThumbX + (currentGamepad.RightThumbX < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone))},
                {"JoyStick2Y", currentGamepad.RightThumbY < deadzone && currentGamepad.RightThumbY > -deadzone ? 0 : ((currentGamepad.RightThumbY + (currentGamepad.RightThumbY < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone))},
                
                {"LeftTrigger", (float)currentGamepad.LeftTrigger / 255},
                {"RightTrigger", (float)currentGamepad.RightTrigger / 255},
                {"ButtonA", ((currentGamepad.Buttons & GamepadButtonFlags.A) != 0) ? 0 : 1},
                {"ButtonB", ((currentGamepad.Buttons & GamepadButtonFlags.B) != 0) ? 0 : 1},
                {"ButtonX", ((currentGamepad.Buttons & GamepadButtonFlags.X) != 0) ? 0 : 1},
                {"ButtonY", ((currentGamepad.Buttons & GamepadButtonFlags.Y) != 0) ? 0 : 1},
                {"ButtonLb", ((currentGamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0) ? 0 : 1},
                {"ButtonRb", ((currentGamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0) ? 0 : 1},
                {"ButtonLs", ((currentGamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0) ? 0 : 1},
                {"ButtonRs", ((currentGamepad.Buttons & GamepadButtonFlags.RightThumb) != 0) ? 0 : 1},
                {"ButtonStart", ((currentGamepad.Buttons & GamepadButtonFlags.Start) != 0) ? 0 : 1},
                {"ButtonBack", ((currentGamepad.Buttons & GamepadButtonFlags.Back) != 0) ? 0 : 1},
                {"DPadL", ((currentGamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0) ? 0 : 1},
                {"DPadU", ((currentGamepad.Buttons & GamepadButtonFlags.DPadUp) != 0) ? 0 : 1},
                {"DPadR", ((currentGamepad.Buttons & GamepadButtonFlags.DPadRight) != 0) ? 0 : 1},
                {"DPadD", ((currentGamepad.Buttons & GamepadButtonFlags.DPadDown) != 0) ? 0 : 1}
            };
        }

        public void StartDevice()
        { }

        public void StopDevice()
        { }
    }
}