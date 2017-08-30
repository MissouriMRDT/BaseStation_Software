using RED.Interfaces.Input;
using RED.Models.Input.Controllers;
using SharpDX.XInput;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Input.Controllers
{
    public class XboxControllerInputViewModel : ControllerBase, IInputDevice
    {
        private readonly XboxControllerInputModel Model = new XboxControllerInputModel();
        public readonly Controller Controller;

        public string Name { get; private set; }
        public string DeviceType { get; private set; }

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
            }
        }

        public XboxControllerInputViewModel(int controllerIndex)
        {
            Name = "Xbox " + controllerIndex.ToString();
            DeviceType = "Xbox";

            Controller = new Controller(IntToUserIndex(controllerIndex));
            InitializeDebounce(new[] { 
                "ButtonA", "ButtonB",
                "ButtonX", "ButtonY", 
                "ButtonLb", "ButtonRb", 
                "ButtonLs", "ButtonRs", 
                "ButtonStart", "ButtonBack", 
                "DPadL", "DPadU", "DPadR", "DPadD" });
        }

        private UserIndex IntToUserIndex(int index)
        {
            switch (index)
            {
                case 1: return UserIndex.One;
                case 2: return UserIndex.Two;
                case 3: return UserIndex.Three;
                case 4: return UserIndex.Four;
                default: return UserIndex.Any;
            }
        }

        public Dictionary<string, float> GetValues()
        {
            if (!IsReady()) throw new Exception("Controller Disconnected");

            var currentGamepad = Controller.GetState().Gamepad;

            int deadzone = AutoDeadzone ? Math.Max(Gamepad.LeftThumbDeadZone, Gamepad.RightThumbDeadZone) : ManualDeadzone;

            return new Dictionary<string, float>()
            {
                {"JoyStick1X", DeadzoneTransform(currentGamepad.LeftThumbX, deadzone)},
                {"JoyStick1Y", DeadzoneTransform(currentGamepad.LeftThumbY, deadzone)},
                {"JoyStick2X", DeadzoneTransform(currentGamepad.RightThumbX, deadzone)},
                {"JoyStick2Y", DeadzoneTransform(currentGamepad.RightThumbY, deadzone)},
                
                {"LeftTrigger", (float)currentGamepad.LeftTrigger / 255},
                {"RightTrigger", (float)currentGamepad.RightTrigger / 255},
                {"ButtonA", ((currentGamepad.Buttons & GamepadButtonFlags.A) != 0) ? 1 : 0},
                {"ButtonB", ((currentGamepad.Buttons & GamepadButtonFlags.B) != 0) ? 1 : 0},
                {"ButtonX", ((currentGamepad.Buttons & GamepadButtonFlags.X) != 0) ? 1 : 0},
                {"ButtonY", ((currentGamepad.Buttons & GamepadButtonFlags.Y) != 0) ? 1 : 0},
                {"ButtonLb", ((currentGamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0) ? 1 : 0},
                {"ButtonRb", ((currentGamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0) ? 1 : 0},
                {"ButtonLs", ((currentGamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0) ? 1 : 0},
                {"ButtonRs", ((currentGamepad.Buttons & GamepadButtonFlags.RightThumb) != 0) ? 1 : 0},
                {"ButtonStart", ((currentGamepad.Buttons & GamepadButtonFlags.Start) != 0) ? 1 : 0},
                {"ButtonBack", ((currentGamepad.Buttons & GamepadButtonFlags.Back) != 0) ? 1 : 0},
                {"DPadL", ((currentGamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0) ? 1 : 0},
                {"DPadU", ((currentGamepad.Buttons & GamepadButtonFlags.DPadUp) != 0) ? 1 : 0},
                {"DPadR", ((currentGamepad.Buttons & GamepadButtonFlags.DPadRight) != 0) ? 1 : 0},
                {"DPadD", ((currentGamepad.Buttons & GamepadButtonFlags.DPadDown) != 0) ? 1 : 0},
                {"ButtonADebounced", Debounce("ButtonA", (currentGamepad.Buttons & GamepadButtonFlags.A) != 0) },
                {"ButtonBDebounced", Debounce("ButtonB", (currentGamepad.Buttons & GamepadButtonFlags.B) != 0) },
                {"ButtonXDebounced", Debounce("ButtonX", (currentGamepad.Buttons & GamepadButtonFlags.X) != 0) },
                {"ButtonYDebounced", Debounce("ButtonY", (currentGamepad.Buttons & GamepadButtonFlags.Y) != 0) },
                {"ButtonLbDebounced", Debounce("ButtonLb", (currentGamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0) },
                {"ButtonRbDebounced", Debounce("ButtonRb", (currentGamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0) },
                {"ButtonLsDebounced", Debounce("ButtonLs", (currentGamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0) },
                {"ButtonRsDebounced", Debounce("ButtonRs", (currentGamepad.Buttons & GamepadButtonFlags.RightThumb) != 0) },
                {"ButtonStartDebounced", Debounce("ButtonStart", (currentGamepad.Buttons & GamepadButtonFlags.Start) != 0) },
                {"ButtonBackDebounced", Debounce("ButtonBack", (currentGamepad.Buttons & GamepadButtonFlags.Back) != 0) },
                {"ButtonDPadLDebounced", Debounce("DPadL", (currentGamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0) },
                {"ButtonDPadUDebounced", Debounce("DPadU", (currentGamepad.Buttons & GamepadButtonFlags.DPadUp) != 0) },
                {"ButtonDPadRDebounced", Debounce("DPadR", (currentGamepad.Buttons & GamepadButtonFlags.DPadRight) != 0) },
                {"ButtonDPadDDebounced", Debounce("DPadD", (currentGamepad.Buttons & GamepadButtonFlags.DPadDown) != 0) }
            };
        }

        public void StartDevice()
        { }

        public void StopDevice()
        { }

        public bool IsReady()
        {
            Connected = Controller != null && Controller.IsConnected;
            return Connected;
        }
    }
}