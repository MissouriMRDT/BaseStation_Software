using Caliburn.Micro;
using RED.Interfaces.Input;
using RED.Models.Input.Controllers;
using System.Collections.Generic;
using System.Windows.Input;

namespace RED.ViewModels.Input.Controllers
{
    public class KeyboardInputViewModel : PropertyChangedBase, IInputDevice
    {
        private readonly KeyboardInputModel Model = new KeyboardInputModel();

        public string Name { get; private set; }
        public string DeviceType { get; private set; }

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

        public float speedMultiplier
        {
            get
            {
                return Model.speedMultiplier;
            }
            set
            {
                Model.speedMultiplier = value;
                NotifyOfPropertyChange(() => speedMultiplier);
            }
        }

        public KeyboardInputViewModel()
        {
            Name = "Keyboard";
            DeviceType = "Keyboard";
        }

        public Dictionary<string, float> GetValues()
        {
            Connected = true;

            return new Dictionary<string, float>()
            {
                {"A", Keyboard.IsKeyDown(Key.A) ? 1 : 0},
                {"B", Keyboard.IsKeyDown(Key.B) ? 1 : 0},
                {"C", Keyboard.IsKeyDown(Key.C) ? 1 : 0},
                {"D", Keyboard.IsKeyDown(Key.D) ? 1 : 0},
                {"E", Keyboard.IsKeyDown(Key.E) ? 1 : 0},
                {"F", Keyboard.IsKeyDown(Key.F) ? 1 : 0},
                {"G", Keyboard.IsKeyDown(Key.G) ? 1 : 0},
                {"H", Keyboard.IsKeyDown(Key.H) ? 1 : 0},
                {"I", Keyboard.IsKeyDown(Key.I) ? 1 : 0},
                {"J", Keyboard.IsKeyDown(Key.J) ? 1 : 0},
                {"K", Keyboard.IsKeyDown(Key.K) ? 1 : 0},
                {"L", Keyboard.IsKeyDown(Key.L) ? 1 : 0},
                {"M", Keyboard.IsKeyDown(Key.M) ? 1 : 0},
                {"N", Keyboard.IsKeyDown(Key.N) ? 1 : 0},
                {"O", Keyboard.IsKeyDown(Key.O) ? 1 : 0},
                {"P", Keyboard.IsKeyDown(Key.P) ? 1 : 0},
                {"Q", Keyboard.IsKeyDown(Key.Q) ? 1 : 0},
                {"R", Keyboard.IsKeyDown(Key.R) ? 1 : 0},
                {"S", Keyboard.IsKeyDown(Key.S) ? 1 : 0},
                {"T", Keyboard.IsKeyDown(Key.T) ? 1 : 0},
                {"U", Keyboard.IsKeyDown(Key.U) ? 1 : 0},
                {"V", Keyboard.IsKeyDown(Key.V) ? 1 : 0},
                {"W", Keyboard.IsKeyDown(Key.W) ? 1 : 0},
                {"X", Keyboard.IsKeyDown(Key.X) ? 1 : 0},
                {"Y", Keyboard.IsKeyDown(Key.Y) ? 1 : 0},
                {"Z", Keyboard.IsKeyDown(Key.Z) ? 1 : 0},
                {"D1", Keyboard.IsKeyDown(Key.D1) ? 1 : 0},
                {"D2", Keyboard.IsKeyDown(Key.D2) ? 1 : 0},
                {"D3", Keyboard.IsKeyDown(Key.D3) ? 1 : 0},
                {"D4", Keyboard.IsKeyDown(Key.D4) ? 1 : 0},
                {"D5", Keyboard.IsKeyDown(Key.D5) ? 1 : 0},
                {"D6", Keyboard.IsKeyDown(Key.D6) ? 1 : 0},
                {"D7", Keyboard.IsKeyDown(Key.D7) ? 1 : 0},
                {"D8", Keyboard.IsKeyDown(Key.D8) ? 1 : 0},
                {"D9", Keyboard.IsKeyDown(Key.D9) ? 1 : 0},
                {"D0", Keyboard.IsKeyDown(Key.D0) ? 1 : 0},
                {"Tilde", Keyboard.IsKeyDown(Key.OemTilde) ? 1 : 0},
                {"Comma", Keyboard.IsKeyDown(Key.OemComma) ? 1 : 0},
                {"Period", Keyboard.IsKeyDown(Key.OemPeriod) ? 1 : 0},
                {"Plus", Keyboard.IsKeyDown(Key.OemPlus) ? 1 : 0},
                {"Minus", Keyboard.IsKeyDown(Key.OemMinus) ? 1 : 0},
                {"RightShift", Keyboard.IsKeyDown(Key.RightShift) ? 1 : 0},
                {"ShiftLeft", Keyboard.IsKeyDown(Key.LeftShift) ? 1 : 0},
                {"ArrowLeft", Keyboard.IsKeyDown(Key.Left) ? 1 : 0},
                {"ArrowRight", Keyboard.IsKeyDown(Key.Right) ? 1 : 0},
                {"ArrowUp", Keyboard.IsKeyDown(Key.Up) ? 1 : 0},
                {"ArrowDown", Keyboard.IsKeyDown(Key.Down) ? 1 : 0}
            };
        }

        public void StartDevice()
        { }

        public void StopDevice()
        { }

        public bool IsReady()
        {
            return true;
        }
    }
}