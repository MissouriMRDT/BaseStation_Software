namespace RED.ViewModels.ControlCenter
{
    using Annotations;
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using SharpDX.XInput;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Timers;
    using System.Threading.Tasks;

    public class XboxControllerInputViewModel : PropertyChangedBase, IInputDevice
    {
        private readonly XboxControllerInputModel Model = new XboxControllerInputModel();
        private readonly ControlCenterViewModel _controlCenter;
        [CanBeNull]
        public readonly Controller ControllerOne = new Controller(UserIndex.One);

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
                _controlCenter.StateManager.CurrentControlMode = ControllerModes[CurrentModeIndex].Name;
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
                _controlCenter.StateManager.ControllerIsConnected = value;
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
        public bool ButtonA
        {
            get
            {
                return Model.ButtonA;
            }
            set
            {
                DebouncedButtonA = !Model.ButtonA && value;
                Model.ButtonA = value;
                NotifyOfPropertyChange(() => ButtonA);
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
        public bool ButtonRs
        {
            get
            {
                return Model.ButtonRs;
            }
            set
            {
                DebouncedButtonRs = !Model.ButtonRs && value;
                Model.ButtonRs = value;
                NotifyOfPropertyChange(() => ButtonRs);
            }
        }
        public bool ButtonLs
        {
            get
            {
                return Model.ButtonLs;
            }
            set
            {
                DebouncedButtonLs = !Model.ButtonLs && value;
                Model.ButtonLs = value;
                NotifyOfPropertyChange(() => ButtonLs);
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
        public bool DebouncedButtonA
        {
            get
            {
                return Model.ButtonADebounced;
            }
            set
            {
                Model.ButtonADebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonA);
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
        public bool DebouncedButtonRs
        {
            get
            {
                return Model.ButtonRsDebounced;
            }
            set
            {
                Model.ButtonRsDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonRs);
            }
        }
        public bool DebouncedButtonLs
        {
            get
            {
                return Model.ButtonLsDebounced;
            }
            set
            {
                Model.ButtonLsDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonLs);
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
        #endregion

        public XboxControllerInputViewModel(ControlCenterViewModel cc)
        {
            _controlCenter = cc;

            ControllerModes.Add(new DriveControllerModeViewModel(this, _controlCenter));
            ControllerModes.Add(new ArmControllerModeViewModel(this, _controlCenter));
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
            if (ControllerOne == null || !ControllerOne.IsConnected)
            {
                Connected = false;
                return;
            }
            var currentGamepad = ControllerOne.GetState().Gamepad;
            Connected = true;

            var deadzone = AutoDeadzone ? Math.Max(Gamepad.LeftThumbDeadZone, Gamepad.RightThumbDeadZone) : ManualDeadzone;
            ElbowBend = WheelsLeft = currentGamepad.LeftThumbY < deadzone && currentGamepad.LeftThumbY > -deadzone ? 0 : ((currentGamepad.LeftThumbY + (currentGamepad.LeftThumbY < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));
            WristBend = WheelsRight = currentGamepad.RightThumbY < deadzone && currentGamepad.RightThumbY > -deadzone ? 0 : ((currentGamepad.RightThumbY + (currentGamepad.RightThumbY < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));
            ElbowTwist = currentGamepad.LeftThumbX < deadzone && currentGamepad.LeftThumbX > -deadzone ? 0 : ((currentGamepad.LeftThumbX + (currentGamepad.LeftThumbX < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));
            ElbowBend = currentGamepad.RightThumbX < deadzone && currentGamepad.RightThumbX > -deadzone ? 0 : ((currentGamepad.RightThumbX + (currentGamepad.RightThumbX < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));

            GripperOpen = (float)currentGamepad.LeftTrigger / 255;
            GripperClose = (float)currentGamepad.RightTrigger / 255;
            ButtonA = (currentGamepad.Buttons & GamepadButtonFlags.A) != 0;
            ToolNext = (currentGamepad.Buttons & GamepadButtonFlags.B) != 0;
            ToolPrev = (currentGamepad.Buttons & GamepadButtonFlags.X) != 0;
            ArmReset = (currentGamepad.Buttons & GamepadButtonFlags.Y) != 0;
            DrillCounterClockwise = (currentGamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
            DrillClockwise = (currentGamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0;
            ButtonLs = (currentGamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0;
            ButtonRs = (currentGamepad.Buttons & GamepadButtonFlags.RightThumb) != 0;
            ModeNext = (currentGamepad.Buttons & GamepadButtonFlags.Start) != 0;
            ModePrev = (currentGamepad.Buttons & GamepadButtonFlags.Back) != 0;
            BaseCounterClockwise = (currentGamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
            ActuatorForward = (currentGamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
            BaseClockwise = (currentGamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;
            ActuatorBackward = (currentGamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
        }

        private void EvaluateCurrentMode()
        {
            if (ControllerOne != null && !ControllerOne.IsConnected) return;
            ControllerModes[CurrentModeIndex].EvaluateMode();
        }
    }
}