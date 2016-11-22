using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input.Controllers;
using RED.ViewModels.Modules;
using System;
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
        private StateViewModel _state;

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

        public ObservableCollection<IControllerMode> ControllerModes
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
                _state.CurrentControlMode = ControllerModes[CurrentModeIndex].Name;
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
                _state.ControllerIsConnected = value;
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
                if (value) NextControlMode();
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
                if (value) PreviousControlMode();
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

        public KeyboardInputViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log, StateViewModel state)
        {
            _router = router;
            _idResolver = idResolver;
            _log = log;
            _state = state;

            ControllerModes.Add(new DriveControllerModeViewModel(this, router, idResolver));
            ControllerModes.Add(new ArmControllerModeViewModel(this, router, idResolver, log));
            ControllerModes.Add(new GimbalControllerModeViewModel(this, router, idResolver, log, 0));
            ControllerModes.Add(new GimbalControllerModeViewModel(this, router, idResolver, log, 1));
            if (ControllerModes.Count == 0) throw new ArgumentException("IEnumerable 'modes' must have at least one item");
            CurrentModeIndex = 0;
        }

        public async void Start()
        {
            while (true)
            {
                Update();
                EvaluateCurrentMode();
                await Task.Delay(SerialReadSpeed);
            }
        }

        public void NextControlMode()
        {
            ControllerModes[CurrentModeIndex].ExitMode();
            CurrentModeIndex = (CurrentModeIndex + 1) % ControllerModes.Count;
            ControllerModes[CurrentModeIndex].EnterMode();
        }
        public void PreviousControlMode()
        {
            ControllerModes[CurrentModeIndex].ExitMode();
            CurrentModeIndex = (CurrentModeIndex - 1 + ControllerModes.Count) % ControllerModes.Count;
            ControllerModes[CurrentModeIndex].EnterMode();
        }

        private void Update()
        {
            // Tell RED that this controller is connected
            Connected = true;

            // Set the speed multiplier; 1-9 for 10%-90%; 0 for 100%
            if (Keyboard.IsKeyDown(Key.D1))
                speedMultiplier = 0.1F;
            if (Keyboard.IsKeyDown(Key.D2))
                speedMultiplier = 0.2F;
            if (Keyboard.IsKeyDown(Key.D3))
                speedMultiplier = 0.3F;
            if (Keyboard.IsKeyDown(Key.D4))
                speedMultiplier = 0.4F;
            if (Keyboard.IsKeyDown(Key.D5))
                speedMultiplier = 0.5F;
            if (Keyboard.IsKeyDown(Key.D6))
                speedMultiplier = 0.6F;
            if (Keyboard.IsKeyDown(Key.D7))
                speedMultiplier = 0.7F;
            if (Keyboard.IsKeyDown(Key.D8))
                speedMultiplier = 0.8F;
            if (Keyboard.IsKeyDown(Key.D9))
                speedMultiplier = 0.9F;
            if (Keyboard.IsKeyDown(Key.D0))
                speedMultiplier = 1.0f;

            // Fine tune the speed multiplier
            if (speedMultiplier < 1f && Keyboard.IsKeyDown(Key.OemPlus))
                speedMultiplier += 0.01f;
            // speedMultiplier will have round-off error, so check before it hits zero
            if (speedMultiplier > 0.01f && Keyboard.IsKeyDown(Key.OemMinus))
                speedMultiplier -= 0.01f;

            Console.WriteLine(speedMultiplier);

            // Keys A and Q control the left wheels in drive mode
            // and the elbow bend in arm mode
            if (Keyboard.IsKeyDown(Key.A))
                ElbowBend = GimbalPan = WheelsLeft = -(float)(Math.Sqrt(speedMultiplier));
            else if (Keyboard.IsKeyDown(Key.Q))
                ElbowBend = WheelsLeft = (float)(Math.Sqrt(speedMultiplier));
            else
                ElbowBend = WheelsLeft = 0;

            // Keys D and E control the right wheels in drive mode
            // and the wrist bend in arm mode
            if (Keyboard.IsKeyDown(Key.D))
                WristBend = GimbalPan = WheelsRight = -(float)(Math.Sqrt(speedMultiplier));
            else if (Keyboard.IsKeyDown(Key.E))
                WristBend = WheelsRight = (float)(Math.Sqrt(speedMultiplier));
            else
                WristBend = WheelsRight = 0;

            // Keys D and E control the right wheels in drive mode
            // and the wrist bend in arm mode
            if (Keyboard.IsKeyDown(Key.W))
                ElbowTwist = GimbalTilt = 1;
            else if (Keyboard.IsKeyDown(Key.S))
                ElbowTwist = GimbalTilt = -1;
            else
                ElbowTwist = 0;

            // Keys Z and C control the wrist twist
            if (Keyboard.IsKeyDown(Key.Z))
                WristTwist = 1;
            else if (Keyboard.IsKeyDown(Key.C))
                WristTwist = -1;
            else
                WristTwist = 0;

            // Keys J and K control the gripper
            if (Keyboard.IsKeyDown(Key.J))
                GripperOpen = 1.0f;
            else
                GripperOpen = 0.0f;
            if (Keyboard.IsKeyDown(Key.K))
                GripperClose = 1.0f;
            else
                GripperClose = 0.0f;

            ToolNext = Keyboard.IsKeyDown(Key.T);
            ToolPrev = Keyboard.IsKeyDown(Key.Y);
            ArmReset = Keyboard.IsKeyDown(Key.OemTilde);
            DrillCounterClockwise = Keyboard.IsKeyDown(Key.OemComma);
            DrillClockwise = Keyboard.IsKeyDown(Key.OemPeriod);
            ModeNext = Keyboard.IsKeyDown(Key.RightShift);
            ModePrev = Keyboard.IsKeyDown(Key.LeftShift);
            BaseCounterClockwise = Keyboard.IsKeyDown(Key.Left);
            BaseClockwise = Keyboard.IsKeyDown(Key.Right);
            ActuatorForward = Keyboard.IsKeyDown(Key.Up);
            ActuatorBackward = Keyboard.IsKeyDown(Key.Down);
        }

        private void EvaluateCurrentMode()
        {
            ControllerModes[CurrentModeIndex].EvaluateMode();
        }
    }
}