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

    public class InputViewModel : PropertyChangedBase
    {
        private readonly InputModel Model = new InputModel();
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
        public float JoyStick1X
        {
            get
            {
                return Model.JoyStick1X;
            }
            set
            {
                Model.JoyStick1X = value;
                NotifyOfPropertyChange(() => JoyStick1X);
            }
        }
        public float JoyStick1Y
        {
            get
            {
                return Model.JoyStick1Y;
            }
            set
            {
                Model.JoyStick1Y = value;
                NotifyOfPropertyChange(() => JoyStick1Y);
            }
        }
        public float JoyStick2X
        {
            get
            {
                return Model.JoyStick2X;
            }
            set
            {
                Model.JoyStick2X = value;
                NotifyOfPropertyChange(() => JoyStick2X);
            }
        }
        public float JoyStick2Y
        {
            get
            {
                return Model.JoyStick2Y;
            }
            set
            {
                Model.JoyStick2Y = value;
                NotifyOfPropertyChange(() => JoyStick2Y);
            }
        }
        public float LeftTrigger
        {
            get
            {
                return Model.LeftTrigger;
            }
            set
            {
                Model.LeftTrigger = value;
                NotifyOfPropertyChange(() => LeftTrigger);
            }
        }
        public float RightTrigger
        {
            get
            {
                return Model.RightTrigger;
            }
            set
            {
                Model.RightTrigger = value;
                NotifyOfPropertyChange(() => RightTrigger);
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
        public bool ButtonB
        {
            get
            {
                return Model.ButtonB;
            }
            set
            {
                DebouncedButtonB = !Model.ButtonB && value;
                Model.ButtonB = value;
                NotifyOfPropertyChange(() => ButtonB);
            }
        }
        public bool ButtonX
        {
            get
            {
                return Model.ButtonX;
            }
            set
            {
                DebouncedButtonX = !Model.ButtonX && value;
                Model.ButtonX = value;
                NotifyOfPropertyChange(() => ButtonX);
            }
        }
        public bool ButtonY
        {
            get
            {
                return Model.ButtonY;
            }
            set
            {
                DebouncedButtonY = !Model.ButtonY && value;
                Model.ButtonY = value;
                NotifyOfPropertyChange(() => ButtonY);
            }
        }
        public bool ButtonRb
        {
            get
            {
                return Model.ButtonRb;
            }
            set
            {
                DebouncedButtonRb = !Model.ButtonRb && value;
                Model.ButtonRb = value;
                NotifyOfPropertyChange(() => ButtonRb);
            }
        }
        public bool ButtonLb
        {
            get
            {
                return Model.ButtonLb;
            }
            set
            {
                DebouncedButtonLb = !Model.ButtonLb && value;
                Model.ButtonLb = value;
                NotifyOfPropertyChange(() => ButtonLb);
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
        public bool ButtonStart
        {
            get
            {
                return Model.ButtonStart;
            }
            set
            {
                DebouncedButtonStart = !Model.ButtonStart && value;
                Model.ButtonStart = value;
                NotifyOfPropertyChange(() => ButtonStart);
            }
        }
        public bool ButtonBack
        {
            get
            {
                return Model.ButtonBack;
            }
            set
            {
                DebouncedButtonBack = !Model.ButtonBack && value;
                Model.ButtonBack = value;
                NotifyOfPropertyChange(() => ButtonBack);
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
        public bool DebouncedButtonB
        {
            get
            {
                return Model.ButtonBDebounced;
            }
            set
            {
                Model.ButtonBDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonB);
            }
        }
        public bool DebouncedButtonX
        {
            get
            {
                return Model.ButtonXDebounced;
            }
            set
            {
                Model.ButtonXDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonX);
            }
        }
        public bool DebouncedButtonY
        {
            get
            {
                return Model.ButtonYDebounced;
            }
            set
            {
                Model.ButtonYDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonY);

            }
        }
        public bool DebouncedButtonRb
        {
            get
            {
                return Model.ButtonRbDebounced;
            }
            set
            {
                Model.ButtonRbDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonRb);
            }
        }
        public bool DebouncedButtonLb
        {
            get
            {
                return Model.ButtonLbDebounced;
            }
            set
            {
                Model.ButtonLbDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonLb);
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
        public bool DebouncedButtonStart
        {
            get
            {
                return Model.ButtonStartDebounced;
            }
            set
            {
                if (value) NextControlMode();
                Model.ButtonStartDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonStart);
            }
        }
        public bool DebouncedButtonBack
        {
            get
            {
                return Model.ButtonBackDebounced;
            }
            set
            {
                if (value) PreviousControlMode();
                Model.ButtonBackDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonBack);
            }
        }
        public bool DPadL
        {
            get
            {
                return Model.DPadL;
            }
            set
            {
                Model.DPadL = value;
                NotifyOfPropertyChange(() => DPadL);
            }
        }
        public bool DPadU
        {
            get
            {
                return Model.DPadU;
            }
            set
            {
                Model.DPadU = value;
                NotifyOfPropertyChange(() => DPadU);
            }
        }
        public bool DPadR
        {
            get
            {
                return Model.DPadR;
            }
            set
            {
                Model.DPadR = value;
                NotifyOfPropertyChange(() => DPadR);
            }
        }
        public bool DPadD
        {
            get
            {
                return Model.DPadD;
            }
            set
            {
                Model.DPadD = value;
                NotifyOfPropertyChange(() => DPadD);
            }
        }
        #endregion

        public InputViewModel(ControlCenterViewModel cc)
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
            JoyStick1X = currentGamepad.LeftThumbX < deadzone && currentGamepad.LeftThumbX > -deadzone ? 0 : ((currentGamepad.LeftThumbX + (currentGamepad.LeftThumbX < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));
            JoyStick1Y = currentGamepad.LeftThumbY < deadzone && currentGamepad.LeftThumbY > -deadzone ? 0 : ((currentGamepad.LeftThumbY + (currentGamepad.LeftThumbY < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));
            JoyStick2X = currentGamepad.RightThumbX < deadzone && currentGamepad.RightThumbX > -deadzone ? 0 : ((currentGamepad.RightThumbX + (currentGamepad.RightThumbX < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));
            JoyStick2Y = currentGamepad.RightThumbY < deadzone && currentGamepad.RightThumbY > -deadzone ? 0 : ((currentGamepad.RightThumbY + (currentGamepad.RightThumbY < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone));

            LeftTrigger = (float)currentGamepad.LeftTrigger / 255;
            RightTrigger = (float)currentGamepad.RightTrigger / 255;
            ButtonA = (currentGamepad.Buttons & GamepadButtonFlags.A) != 0;
            ButtonB = (currentGamepad.Buttons & GamepadButtonFlags.B) != 0;
            ButtonX = (currentGamepad.Buttons & GamepadButtonFlags.X) != 0;
            ButtonY = (currentGamepad.Buttons & GamepadButtonFlags.Y) != 0;
            ButtonLb = (currentGamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
            ButtonRb = (currentGamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0;
            ButtonLs = (currentGamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0;
            ButtonRs = (currentGamepad.Buttons & GamepadButtonFlags.RightThumb) != 0;
            ButtonStart = (currentGamepad.Buttons & GamepadButtonFlags.Start) != 0;
            ButtonBack = (currentGamepad.Buttons & GamepadButtonFlags.Back) != 0;
            DPadL = (currentGamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
            DPadU = (currentGamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
            DPadR = (currentGamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;
            DPadD = (currentGamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
        }

        private void EvaluateCurrentMode()
        {
            ControllerModes[CurrentModeIndex].EvaluateMode();
        }
    }
}