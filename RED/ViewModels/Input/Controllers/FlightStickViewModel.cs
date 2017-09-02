using RED.Interfaces.Input;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Input.Controllers
{
    public class FlightStickViewModel : ControllerBase, IInputDevice
    {
        private readonly DirectInput directInput;
        private Joystick joystick;

        private const int Deadzone = 32768 * 10 / 1000;

        public string Name { get; }
        public string DeviceType { get; }

        public FlightStickViewModel()
        {
            Name = "Flight Stick";
            DeviceType = "FlightStick";

            directInput = new DirectInput();

            InitializeDebounce(new[] {
                "Button0", "Button1", "Button2",
                "Button3", "Button4", "Button5",
                "Button6", "Button7", "Button8",
                "Button9", "Button10", "Button11" });
            EstablishJoystick();
        }

        public Dictionary<string, float> GetValues()
        {
            joystick.Acquire();
            JoystickState state = joystick.GetCurrentState();

            return new Dictionary<string, float>()
            {
                ["X"] = DeadzoneTransform(state.X - 32768, Deadzone),
                ["Y"] = -DeadzoneTransform(state.Y - 32768, Deadzone),
                ["RotationZ"] = (state.RotationZ - 32768) / 32768f,

                ["POVX"] = POVtoX(state.PointOfViewControllers[0]),
                ["POVY"] = POVtoY(state.PointOfViewControllers[0]),

                ["Slider0"] = 1f - state.Sliders[0] / 65535f,

                ["Button0"] = (state.Buttons[0]) ? 1f : 0f,
                ["Button1"] = (state.Buttons[1]) ? 1f : 0f,
                ["Button2"] = (state.Buttons[2]) ? 1f : 0f,
                ["Button3"] = (state.Buttons[3]) ? 1f : 0f,
                ["Button4"] = (state.Buttons[4]) ? 1f : 0f,
                ["Button5"] = (state.Buttons[5]) ? 1f : 0f,
                ["Button6"] = (state.Buttons[6]) ? 1f : 0f,
                ["Button7"] = (state.Buttons[7]) ? 1f : 0f,
                ["Button8"] = (state.Buttons[8]) ? 1f : 0f,
                ["Button9"] = (state.Buttons[9]) ? 1f : 0f,
                ["Button10"] = (state.Buttons[10]) ? 1f : 0f,
                ["Button11"] = (state.Buttons[11]) ? 1f : 0f,
                ["Button0Debounced"] = Debounce("Button0", state.Buttons[0]),
                ["Button1Debounced"] = Debounce("Button1", state.Buttons[1]),
                ["Button2Debounced"] = Debounce("Button2", state.Buttons[2]),
                ["Button3Debounced"] = Debounce("Button3", state.Buttons[3]),
                ["Button4Debounced"] = Debounce("Button4", state.Buttons[4]),
                ["Button5Debounced"] = Debounce("Button5", state.Buttons[5]),
                ["Button6Debounced"] = Debounce("Button6", state.Buttons[6]),
                ["Button7Debounced"] = Debounce("Button7", state.Buttons[7]),
                ["Button8Debounced"] = Debounce("Button8", state.Buttons[8]),
                ["Button9Debounced"] = Debounce("Button9", state.Buttons[9]),
                ["Button10Debounced"] = Debounce("Button10", state.Buttons[10]),
                ["Button11Debounced"] = Debounce("Button11", state.Buttons[11])
            };
        }

        private float POVtoX(int povValue)
        {
            switch (povValue)
            {
                case 31500:
                case 27000:
                case 22500:
                    return -1f;
                case 0:
                case -1:
                case 18000:
                    return 0f;
                case 4500:
                case 9000:
                case 13500:
                    return 1f;
                default:
                    return 0f;
            }
        }
        private float POVtoY(int povValue)
        {
            switch (povValue)
            {
                case 31500:
                case 0:
                case 4500:
                    return 1f;
                case 27000:
                case -1:
                case 9000:
                    return 0f;
                case 22500:
                case 18000:
                case 13500:
                    return -1f;
                default:
                    return 0f;
            }
        }

        public void StartDevice()
        { }

        public void StopDevice()
        { }

        public bool IsReady()
        {
            if (joystick == null) return EstablishJoystick();

            try
            {
                joystick.Acquire();
                return true;
            }
            catch (SharpDX.SharpDXException)
            {
                return EstablishJoystick();
            }
        }

        private bool EstablishJoystick()
        {
            Guid joystickGuid = Guid.Empty;

            var device = directInput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);

            if (device.Count <= 0) return false;

            joystickGuid = device[0].InstanceGuid;
            joystick = new Joystick(directInput, joystickGuid);
            joystick.Properties.BufferSize = 128;
            return true;
        }
    }
}
