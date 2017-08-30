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
        public float SpeedMultiplier
        {
            get
            {
                return Model.SpeedMultiplier;
            }
            set
            {
                Model.SpeedMultiplier = value;
                NotifyOfPropertyChange(() => SpeedMultiplier);
            }
        }

        private Dictionary<string, bool> DebounceStates;

        public KeyboardInputViewModel()
        {
            Name = "Keyboard";
            DeviceType = "Keyboard";
            InitializeDebounce();
            Connected = true;
        }

        public Dictionary<string, float> GetValues()
        {
            var mappingValues = new Dictionary<string, float>(AllKeys.Count * 2);

            foreach (var item in AllKeys)
            {
                mappingValues.Add(item.Key, Keyboard.IsKeyDown(item.Value) ? SpeedMultiplier : 0f);
                mappingValues.Add(item.Key + "Debounced", Debounce(item.Key, Keyboard.IsKeyDown(item.Value)));
            }

            mappingValues.Add("ArrowUpDown", twoButtonTransform(Keyboard.IsKeyDown(Key.Up), Keyboard.IsKeyDown(Key.Down), SpeedMultiplier, -SpeedMultiplier, 0f));
            mappingValues.Add("ArrowLeftRight", twoButtonTransform(Keyboard.IsKeyDown(Key.Left), Keyboard.IsKeyDown(Key.Right), SpeedMultiplier, -SpeedMultiplier, 0f));
            mappingValues.Add("WS", twoButtonTransform(Keyboard.IsKeyDown(Key.W), Keyboard.IsKeyDown(Key.S), SpeedMultiplier, -SpeedMultiplier, 0f));
            mappingValues.Add("AD", twoButtonTransform(Keyboard.IsKeyDown(Key.A), Keyboard.IsKeyDown(Key.D), SpeedMultiplier, -SpeedMultiplier, 0f));
            mappingValues.Add("IK", twoButtonTransform(Keyboard.IsKeyDown(Key.I), Keyboard.IsKeyDown(Key.K), SpeedMultiplier, -SpeedMultiplier, 0f));
            mappingValues.Add("JL", twoButtonTransform(Keyboard.IsKeyDown(Key.J), Keyboard.IsKeyDown(Key.L), SpeedMultiplier, -SpeedMultiplier, 0f));
            
            SpeedMultiplier = GetSpeedMultiplier(mappingValues);

            return mappingValues;
        }

        public void StartDevice()
        { }

        public void StopDevice()
        { }

        public bool IsReady()
        {
            return Connected;
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
        private T twoButtonTransform<T>(bool bool1, bool bool2, T val1, T val2, T val0)
        {
            return bool1 ? val1 : (bool2 ? val2 : val0);
        }

        private float GetSpeedMultiplier(Dictionary<string, float> mappingValues)
        {
            if (mappingValues["D0Debounced"] > 0) return 1.0f;
            if (mappingValues["D9Debounced"] > 0) return 0.9f;
            if (mappingValues["D8Debounced"] > 0) return 0.8f;
            if (mappingValues["D7Debounced"] > 0) return 0.7f;
            if (mappingValues["D6Debounced"] > 0) return 0.6f;
            if (mappingValues["D5Debounced"] > 0) return 0.5f;
            if (mappingValues["D4Debounced"] > 0) return 0.4f;
            if (mappingValues["D3Debounced"] > 0) return 0.3f;
            if (mappingValues["D2Debounced"] > 0) return 0.2f;
            if (mappingValues["D1Debounced"] > 0) return 0.1f;
            return SpeedMultiplier;
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