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

        private Dictionary<string, bool> DebounceStates;

        public KeyboardInputViewModel()
        {
            Name = "Keyboard";
            DeviceType = "Keyboard";
            InitializeDebounce();
        }

        public Dictionary<string, float> GetValues()
        {
            Connected = true;

            var mappingValues = new Dictionary<string, float>(AllKeys.Count * 2);

            foreach (var item in AllKeys)
            {
                mappingValues.Add(item.Key, Keyboard.IsKeyDown(item.Value) ? 1f : 0f);
                mappingValues.Add(item.Key + "Debounced", Debounce(item.Key, Keyboard.IsKeyDown(item.Value)));
            }

            return mappingValues;
        }

        public void StartDevice()
        { }

        public void StopDevice()
        { }

        public bool IsReady()
        {
            return true;
        }

        private void InitializeDebounce()
        {
            DebounceStates = new Dictionary<string, bool>();
            foreach (var item in AllKeys)
                DebounceStates.Add(item.Key, false);
        }
        private float Debounce(string key, bool newState)
        {
            if (!DebounceStates[key] && newState)
            {
                DebounceStates[key] = true;
                return 1f;
            }
            else if (DebounceStates[key] && !newState)
            {
                DebounceStates[key] = false;
                return 0f;
            }
            else
            {
                return 0f;
            }
        }

        private static Dictionary<string, Key> AllKeys = new Dictionary<string, Key>()
        {
            {"A", Key.A},
            {"B", Key.B},
            {"C", Key.C},
            {"D", Key.D},
            {"E", Key.E},
            {"F", Key.F},
            {"G", Key.G},
            {"H", Key.H},
            {"I", Key.I},
            {"J", Key.J},
            {"K", Key.K},
            {"L", Key.L},
            {"M", Key.M},
            {"N", Key.N},
            {"O", Key.O},
            {"P", Key.P},
            {"Q", Key.Q},
            {"R", Key.R},
            {"S", Key.S},
            {"T", Key.T},
            {"U", Key.U},
            {"V", Key.V},
            {"W", Key.W},
            {"X", Key.X},
            {"Y", Key.Y},
            {"Z", Key.Z},
            {"D1", Key.D1},
            {"D2", Key.D2},
            {"D3", Key.D3},
            {"D4", Key.D4},
            {"D5", Key.D5},
            {"D6", Key.D6},
            {"D7", Key.D7},
            {"D8", Key.D8},
            {"D9", Key.D9},
            {"D0", Key.D0},
            {"Tilde", Key.OemTilde},
            {"Comma", Key.OemComma},
            {"Period", Key.OemPeriod},
            {"Plus", Key.OemPlus},
            {"Minus", Key.OemMinus},
            {"RightShift", Key.RightShift},
            {"ShiftLeft", Key.LeftShift},
            {"ArrowLeft", Key.Left},
            {"ArrowRight", Key.Right},
            {"ArrowUp", Key.Up},
            {"ArrowDown", Key.Down}
        };
    }
}