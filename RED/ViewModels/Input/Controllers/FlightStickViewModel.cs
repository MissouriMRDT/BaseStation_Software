using Caliburn.Micro;
using RED.Interfaces.Input;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Input.Controllers
{
    public class FlightStickViewModel : PropertyChangedBase, IInputDevice
    {
        private DirectInput directInput;
        private Joystick joystick;

        private const int Deadzone = 32768 * 10 / 1000;

        public string Name { get; private set; }
        public string DeviceType { get; private set; }

        public FlightStickViewModel()
        {
            Name = "Flight Stick";
            DeviceType = "FlightStick";

            directInput = new DirectInput();

            EstablishJoystick();
        }

        public Dictionary<string, float> GetValues()
        {
            joystick.Acquire();
            JoystickState state = joystick.GetCurrentState();

            Func<int, float> deadzoneTransform = x => x < Deadzone && x > -Deadzone ? 0 : ((x + (x < 0 ? Deadzone : -Deadzone)) / (float)(32768 - Deadzone));

            return new Dictionary<string, float>()
            {
                {"X", deadzoneTransform(state.X - 32768)},
                {"Y", -deadzoneTransform(state.Y - 32768)},
                {"RotationZ", (state.RotationZ - 32768) / 32768f},

                {"POVX", POVtoX(state.PointOfViewControllers[0])},
                {"POVY", POVtoY(state.PointOfViewControllers[0])},

                {"Slider0", 1f - state.Sliders[0] / 65535f},

                {"Button0", (state.Buttons[0]) ? 1f : 0f},
                {"Button1", (state.Buttons[1]) ? 1f : 0f},
                {"Button2", (state.Buttons[2]) ? 1f : 0f},
                {"Button3", (state.Buttons[3]) ? 1f : 0f},
                {"Button4", (state.Buttons[4]) ? 1f : 0f},
                {"Button5", (state.Buttons[5]) ? 1f : 0f},
                {"Button6", (state.Buttons[6]) ? 1f : 0f},
                {"Button7", (state.Buttons[7]) ? 1f : 0f},
                {"Button8", (state.Buttons[8]) ? 1f : 0f},
                {"Button9", (state.Buttons[9]) ? 1f : 0f},
                {"Button10", (state.Buttons[10]) ? 1f : 0f},
                {"Button11", (state.Buttons[11]) ? 1f : 0f}
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
            try
            {
                joystick.Acquire();
            }
            catch (SharpDX.SharpDXException)
            {
                return EstablishJoystick();
            }
            return true;
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
