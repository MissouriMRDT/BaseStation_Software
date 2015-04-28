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
                NotifyOfPropertyChangeThreadSafe(() => Connected);
                _controlCenter.StateManager.ControllerIsConnected = value;
                NotifyOfPropertyChangeThreadSafe(() => ConnectionStatus);
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
                NotifyOfPropertyChangeThreadSafe(() => JoyStick1X);
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
                NotifyOfPropertyChangeThreadSafe(() => JoyStick1Y);
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
                NotifyOfPropertyChangeThreadSafe(() => JoyStick2X);
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
                NotifyOfPropertyChangeThreadSafe(() => JoyStick2Y);
            }
        }
        public float LeftTrigger
        {
            get
            {
                return -(Model.LeftTrigger * 180) - 90;
            }
            set
            {
                Model.LeftTrigger = value;
                NotifyOfPropertyChangeThreadSafe(() => LeftTrigger);
            }
        }
        public float RightTrigger
        {
            get
            {
                return (Model.RightTrigger * 180) - 90;
            }
            set
            {
                Model.RightTrigger = value;
                NotifyOfPropertyChangeThreadSafe(() => RightTrigger);
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
                Model.ButtonA = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonA);
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
                Model.ButtonB = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonB);
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
                Model.ButtonX = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonX);
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
                Model.ButtonY = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonY);
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
                Model.ButtonRb = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonRb);
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
                Model.ButtonLb = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonLb);
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
                Model.ButtonRs = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonRs);
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
                Model.ButtonLs = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonLs);
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
                if (Model.ButtonStart != value && value)
                    NextControlMode();
                Model.ButtonStart = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonStart);
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
                if (Model.ButtonBack != value && value)
                    PreviousControlMode();
                Model.ButtonBack = value;
                NotifyOfPropertyChangeThreadSafe(() => ButtonBack);
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
                NotifyOfPropertyChangeThreadSafe(() => DPadL);
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
                NotifyOfPropertyChangeThreadSafe(() => DPadU);
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
                NotifyOfPropertyChangeThreadSafe(() => DPadR);
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
                NotifyOfPropertyChangeThreadSafe(() => DPadD);
            }
        }
        #endregion

        public InputViewModel(ControlCenterViewModel cc)
        {
            _controlCenter = cc;

            ControllerModes.Add(new DriveControllerModeViewModel(this, _controlCenter));
            ControllerModes.Add(new ArmControllerMode(this, _controlCenter));
            ControllerModes.Add(new GripperControllerMode(this, _controlCenter));
            if (ControllerModes.Count == 0) throw new ArgumentException("IEnumerable 'modes' must have at least one item");
            CurrentModeIndex = 0;

            // Initializes thread for reading controller input
            var updater = new Timer(SerialReadSpeed);
            updater.Elapsed += Update;
            updater.Elapsed += EvaluateCurrentMode;
            updater.Start();
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

        private void Update(object sender, ElapsedEventArgs e)
        {
            if (ControllerOne == null || !ControllerOne.IsConnected)
            {
                Connected = false;
                return;
            }
            var currentGamepad = ControllerOne.GetState().Gamepad;
            Connected = true;

            JoyStick2X = currentGamepad.RightThumbX < Gamepad.RightThumbDeadZone && currentGamepad.RightThumbX > -Gamepad.RightThumbDeadZone ? 0 : (float)currentGamepad.RightThumbX / 32768;
            JoyStick2Y = currentGamepad.RightThumbY < Gamepad.RightThumbDeadZone && currentGamepad.RightThumbY > -Gamepad.RightThumbDeadZone ? 0 : (float)currentGamepad.RightThumbY / 32768;
            JoyStick1X = currentGamepad.LeftThumbX < Gamepad.LeftThumbDeadZone && currentGamepad.LeftThumbX > -Gamepad.LeftThumbDeadZone ? 0 : (float)currentGamepad.LeftThumbX / 32768;
            JoyStick1Y = currentGamepad.LeftThumbY < Gamepad.LeftThumbDeadZone && currentGamepad.LeftThumbY > -Gamepad.LeftThumbDeadZone ? 0 : (float)currentGamepad.LeftThumbY / 32768;

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

        private void EvaluateCurrentMode(object sender, ElapsedEventArgs e)
        {
            ControllerModes[CurrentModeIndex].EvaluateMode();
        }

        /// <summary>
        /// Wraps the Caliburn.Micro NotifyOfPropertyChange method to always perform on the UI thread.
        /// </summary>
        /// <param name="property">Name of the property</param>
        private void NotifyOfPropertyChangeThreadSafe<T>(System.Linq.Expressions.Expression<Func<T>> property)
        {
            new System.Action(() => NotifyOfPropertyChange(property)).BeginOnUIThread();
        }
    }
}