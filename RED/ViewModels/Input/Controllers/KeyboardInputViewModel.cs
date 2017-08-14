using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input.Controllers;
using RED.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RED.ViewModels.Input.Controllers
{
    public class KeyboardInputViewModel : PropertyChangedBase, IInputDevice
    {
        private readonly KeyboardInputModel Model = new KeyboardInputModel();
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

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

        public ObservableCollection<IInputMode> ControllerModes
        {
            get
            {
                return Model.ControllerModes;
            }
        }
        public int CurrentModeIndex
        {
            get
            {
                return Model.CurrentModeIndex;
            }
            private set
            {
                Model.CurrentModeIndex = value;
                NotifyOfPropertyChange(() => CurrentModeIndex);
            }
        }

        #region Controller Display Values
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
                NotifyOfPropertyChange(() => ConnectionStatus);
            }
        }
        public string ConnectionStatus
        {
            get
            {
                return !Connected ? "Disconnected" : "Connected";
            }
        }
        public float WheelsLeft
        {
            get
            {
                return Model.WheelsLeft;
            }
            set
            {
                Model.WheelsLeft = value;
                NotifyOfPropertyChange(() => WheelsLeft);
            }
        }
        public float WheelsRight
        {
            get
            {
                return Model.WheelsRight;
            }
            set
            {
                Model.WheelsRight = value;
                NotifyOfPropertyChange(() => WheelsRight);
            }
        }
        public float WristTwist
        {
            get
            {
                return Model.WristTwist;
            }
            set
            {
                Model.WristTwist = value;
                NotifyOfPropertyChange(() => WristTwist);
            }
        }
        public float WristBend
        {
            get
            {
                return Model.WristBend;
            }
            set
            {
                Model.WristBend = value;
                NotifyOfPropertyChange(() => WristBend);
            }
        }
        public float ElbowBend
        {
            get
            {
                return Model.ElbowBend;
            }
            set
            {
                Model.ElbowBend = value;
                NotifyOfPropertyChange(() => ElbowBend);
            }
        }
        public float ElbowTwist
        {
            get
            {
                return Model.ElbowTwist;
            }
            set
            {
                Model.ElbowTwist = value;
                NotifyOfPropertyChange(() => ElbowTwist);
            }
        }
        public float GripperOpen
        {
            get
            {
                return Model.GripperOpen;
            }
            set
            {
                Model.GripperOpen = value;
                NotifyOfPropertyChange(() => GripperOpen);
            }
        }
        public float GripperClose
        {
            get
            {
                return Model.GripperClose;
            }
            set
            {
                Model.GripperClose = value;
                NotifyOfPropertyChange(() => GripperClose);
            }
        }
        public bool ToolNext
        {
            get
            {
                return Model.ToolNext;
            }
            set
            {
                DebouncedToolNext = !Model.ToolNext && value;
                Model.ToolNext = value;
                NotifyOfPropertyChange(() => ToolNext);
            }
        }
        public bool ToolPrev
        {
            get
            {
                return Model.ToolPrev;
            }
            set
            {
                DebouncedToolPrev = !Model.ToolPrev && value;
                Model.ToolPrev = value;
                NotifyOfPropertyChange(() => ToolPrev);
            }
        }
        public bool ArmReset
        {
            get
            {
                return Model.ArmReset;
            }
            set
            {
                DebouncedArmReset = !Model.ArmReset && value;
                Model.ArmReset = value;
                NotifyOfPropertyChange(() => ArmReset);
            }
        }
        public bool DrillClockwise
        {
            get
            {
                return Model.DrillClockwise;
            }
            set
            {
                DebouncedDrillClockwise = !Model.DrillClockwise && value;
                Model.DrillClockwise = value;
                NotifyOfPropertyChange(() => DrillClockwise);
            }
        }
        public bool DrillCounterClockwise
        {
            get
            {
                return Model.DrillCounterClockwise;
            }
            set
            {
                DebouncedDrillCounterClockwise = !Model.DrillCounterClockwise && value;
                Model.DrillCounterClockwise = value;
                NotifyOfPropertyChange(() => DrillCounterClockwise);
            }
        }
        public bool DebouncedToolNext
        {
            get
            {
                return Model.ToolNextDebounced;
            }
            set
            {
                Model.ToolNextDebounced = value;
                NotifyOfPropertyChange(() => DebouncedToolNext);
            }
        }
        public bool DebouncedToolPrev
        {
            get
            {
                return Model.ToolPrevDebounced;
            }
            set
            {
                Model.ToolPrevDebounced = value;
                NotifyOfPropertyChange(() => DebouncedToolPrev);
            }
        }
        public bool DebouncedArmReset
        {
            get
            {
                return Model.ArmResetDebounced;
            }
            set
            {
                Model.ArmResetDebounced = value;
                NotifyOfPropertyChange(() => DebouncedArmReset);

            }
        }
        public bool DebouncedDrillClockwise
        {
            get
            {
                return Model.DrillClockwiseDebounced;
            }
            set
            {
                Model.DrillClockwiseDebounced = value;
                NotifyOfPropertyChange(() => DebouncedDrillClockwise);
            }
        }
        public bool DebouncedDrillCounterClockwise
        {
            get
            {
                return Model.DrillCounterClockwiseDebounced;
            }
            set
            {
                Model.DrillCounterClockwiseDebounced = value;
                NotifyOfPropertyChange(() => DebouncedDrillCounterClockwise);
            }
        }
        public bool DebouncedModeNext
        {
            get
            {
                return Model.ModeNextDebounced;
            }
            set
            {
                Model.ModeNextDebounced = value;
                NotifyOfPropertyChange(() => DebouncedModeNext);
            }
        }
        public bool DebouncedModePrev
        {
            get
            {
                return Model.ModePrevDebounced;
            }
            set
            {
                Model.ModePrevDebounced = value;
                NotifyOfPropertyChange(() => DebouncedModePrev);
            }
        }
        public bool ModeNext
        {
            get
            {
                return Model.ModeNext;
            }
            set
            {
                DebouncedModeNext = !Model.ModeNext && value;
                Model.ModeNext = value;
                NotifyOfPropertyChange(() => ModeNext);
            }
        }
        public bool ModePrev
        {
            get
            {
                return Model.ModePrev;
            }
            set
            {
                DebouncedModePrev = !Model.ModePrev && value;
                Model.ModePrev = value;
                NotifyOfPropertyChange(() => ModePrev);
            }
        }
        public bool BaseCounterClockwise
        {
            get
            {
                return Model.BaseCounterClockwise;
            }
            set
            {
                Model.BaseCounterClockwise = value;
                NotifyOfPropertyChange(() => BaseCounterClockwise);
            }
        }
        public bool ActuatorForward
        {
            get
            {
                return Model.ActuatorForward;
            }
            set
            {
                Model.ActuatorForward = value;
                NotifyOfPropertyChange(() => ActuatorForward);
            }
        }
        public bool BaseClockwise
        {
            get
            {
                return Model.BaseClockwise;
            }
            set
            {
                Model.BaseClockwise = value;
                NotifyOfPropertyChange(() => BaseClockwise);
            }
        }
        public bool ActuatorBackward
        {
            get
            {
                return Model.ActuatorBackward;
            }
            set
            {
                Model.ActuatorBackward = value;
                NotifyOfPropertyChange(() => ActuatorBackward);
            }
        }
        public float GimbalPan
        {
            get
            {
                return Model.GimbalPan;
            }
            set
            {
                Model.GimbalPan = value;
                NotifyOfPropertyChange(() => GimbalPan);
            }
        }
        public float GimbalTilt
        {
            get
            {
                return Model.GimbalTilt;
            }
            set
            {
                Model.GimbalTilt = value;
                NotifyOfPropertyChange(() => GimbalTilt);
            }
        }
        public bool GimbalZoomIn
        {
            get
            {
                return Model.GimbalZoomIn;
            }
            set
            {
                Model.GimbalZoomIn = value;
                NotifyOfPropertyChange(() => GimbalZoomIn);
            }
        }
        public bool GimbalZoomOut
        {
            get
            {
                return Model.GimbalZoomOut;
            }
            set
            {
                Model.GimbalZoomOut = value;
                NotifyOfPropertyChange(() => GimbalZoomOut);
            }
        }
        #endregion

        public KeyboardInputViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log)
        {
            _router = router;
            _idResolver = idResolver;
            _log = log;

            Name = "Keyboard";
            DeviceType = "Keyboard";

            ControllerModes.Add(new DriveViewModel(this, router, idResolver));
            ControllerModes.Add(new ArmViewModel(this, router, idResolver, log, null));
            ControllerModes.Add(new GimbalViewModel(this, router, idResolver, log, 0));
            ControllerModes.Add(new GimbalViewModel(this, router, idResolver, log, 1));
            if (ControllerModes.Count == 0) throw new ArgumentException("IEnumerable 'modes' must have at least one item");
            CurrentModeIndex = 0;
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