namespace RED.ViewModels.Modules
{
    using Annotations;
    using ControlCenter;
    using FirstFloor.ModernUI.Presentation;
    using Interfaces;
    using Models.ControlCenter;
    using Models.Modules;
    using Models.Settings;
    using Properties;
    using RoverComs;
    using RoverComs.Rover;
    using SharpDX.XInput;
    using System;
    using System.Linq;
    using System.Timers;
    using System.Windows.Input;
    using System.Windows.Media;

    public class InputVM : BaseVM, IModule
    {
        private static readonly InputModel Model = new InputModel();
        [CanBeNull] private readonly Controller controllerOne = new Controller(UserIndex.One);

        public string Title
        {
            get
            {
                return Model.Title;
            }
        }
        public bool InUse
        {
            get
            {
                return Model.InUse;
            }
            set
            {
                Model.InUse = value;
            }
        }
        public bool IsManageable
        {
            get
            {
                return Model.IsManageable;
            }
        }
        public int SerialReadSpeed
        {
            get
            {
                return Settings.Default.SerialReadSpeed;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.Int32) return;
                Settings.Default.SerialReadSpeed = value;
                Settings.Default.Save();
                RaisePropertyChanged("SerialReadSpeed");
            }
        }
        public int DriveCommandSpeedMs
        {
            get
            {
                return Settings.Default.DriveCommandSpeed;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.Int32) return;
                Settings.Default.DriveCommandSpeed = value;
                Settings.Default.Save();
                RaisePropertyChanged("DriveCommandSpeed");
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
                if (Connected != value)
                {
                    Hazel.EnqueueMessage(Connected
                        ? "Control Systems, are now Disengaged"
                        : "Control Systems, are now Engaged");
                }
                SetField(ref Model.Connected, value);
                ControlCenterVM.StateVM.ControllerIsConnected = value;
                RaisePropertyChanged("ConnectionStatus");
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
                SetField(ref Model.JoyStick1X, value);
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
                SetField(ref Model.JoyStick1Y, value);
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
                SetField(ref Model.JoyStick2X, value);
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
                SetField(ref Model.JoyStick2Y, value);
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
                SetField(ref Model.LeftTrigger, value);
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
                SetField(ref Model.RightTrigger, value);
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
                SetField(ref Model.ButtonA, value);
                RaisePropertyChanged("ButtonAColor");
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
                SetField(ref Model.ButtonB, value);
                RaisePropertyChanged("ButtonBColor");
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
                SetField(ref Model.ButtonX, value);
                RaisePropertyChanged("ButtonXColor");
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
                SetField(ref Model.ButtonY, value);
                RaisePropertyChanged("ButtonYColor");
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
                if (Model.ButtonRb != value && value)
                    NextRoboticArmFunction();
                SetField(ref Model.ButtonRb, value);
                RaisePropertyChanged("ButtonRbColor");
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
                if (Model.ButtonLb != value && value)
                    PreviousRoboticArmFunction();
                SetField(ref Model.ButtonLb, value);
                RaisePropertyChanged("ButtonLbColor");
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
                SetField(ref Model.ButtonRs, value);
                RaisePropertyChanged("ButtonRsColor");
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
                SetField(ref Model.ButtonLs, value);
                RaisePropertyChanged("ButtonLsColor");
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
                    ControlCenterVM.StateVM.NextControlMode();
                SetField(ref Model.ButtonStart, value);
                RaisePropertyChanged("ButtonStartColor");
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
                    ControlCenterVM.StateVM.PreviousControlMode();
                SetField(ref Model.ButtonBack, value);
                RaisePropertyChanged("ButtonBackColor");
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
                SetField(ref Model.DPadL, value);
                RaisePropertyChanged("DPadLColor");
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
                SetField(ref Model.DPadU, value);
                RaisePropertyChanged("DPadUColor");
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
                SetField(ref Model.DPadR, value);
                RaisePropertyChanged("DPadRColor");
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
                SetField(ref Model.DPadD, value);
                RaisePropertyChanged("DPadDColor");
            }
        }
        #endregion
        #region Controller Working Values

        public int CurrentRawControllerSpeedLeft
        {
            get
            {
                return Model.CurrentRawControllerSpeedLeft;
            }
            set
            {
                SetField(ref Model.CurrentRawControllerSpeedLeft, value);
            }
        }
        public int CurrentRawControllerSpeedRight
        {
            get
            {
                return Model.CurrentRawControllerSpeedRight;
            }
            set
            {
                SetField(ref Model.CurrentRawControllerSpeedRight, value);
            }
        }
        public int SpeedLeft
        {
            get
            {
                return Model.SpeedLeft;
            }
            set
            {
                SetField(ref Model.SpeedLeft, value);
            }
        }
        public int SpeedRight
        {
            get
            {
                return Model.SpeedRight;
            }
            set
            {
                SetField(ref Model.SpeedRight, value);
            }
        }

        private float joystick1XRaw = 0;
        private float joystick1YRaw = 0;
        private float joystick2XRaw = 0;
        private float joystick2YRaw = 0;

        #endregion
        #region Colors
        public SolidColorBrush ButtonAColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonA ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return ButtonA ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush ButtonBColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonB ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return ButtonB ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush ButtonXColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonX ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return ButtonX ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush ButtonYColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonY ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return ButtonY ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush ButtonLbColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonLb ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x25, 0x26));
                }
                return ButtonLb ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            }
        }
        public SolidColorBrush ButtonRbColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonRb ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x25, 0x26));
                }
                return ButtonRb ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            }
        }
        public SolidColorBrush ButtonLsColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonLs ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x25, 0x26));
                }
                return ButtonLs ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            }
        }
        public SolidColorBrush ButtonRsColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonRs ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x25, 0x26));
                }
                return ButtonRs ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            }
        }
        public SolidColorBrush ButtonStartColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonStart ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return ButtonStart ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
            }
        public SolidColorBrush ButtonBackColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return ButtonBack ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return ButtonBack ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush DPadLColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return DPadL ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return DPadL ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush DPadUColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return DPadU ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return DPadU ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush DPadRColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return DPadR ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return DPadR ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        public SolidColorBrush DPadDColor
        {
            get
            {
                if (SettingsAppearanceModel.SelectedTheme.DisplayName == "dark")
                {
                    return DPadD ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
                return DPadD ? new SolidColorBrush(AppearanceManager.Current.AccentColor) : new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            }
        }
        #endregion

        public InputVM() 
            : base(Model.Title)
        {
            SpeedLeft = 128;
            SpeedRight = 128;

            // Initializes thread for reading controller input
            var updater = new Timer(SerialReadSpeed);
            updater.Elapsed += Update;
            updater.Start();

            // Initializes thread for sending drive commands
            var driver = new Timer(DriveCommandSpeedMs);
            driver.Elapsed += Drive;
            driver.Start();

            // Initializes thread for sending robotic arm commands
            var armOperator = new Timer(150);
            armOperator.Elapsed += OperateArm;
            armOperator.Start();

            // Initializes thread for sending robotic arm commands
            var gripperOperator = new Timer(50);
            gripperOperator.Elapsed += OperateGripper;
            gripperOperator.Start();
        }

        private void Drive(object sender, ElapsedEventArgs e)
        {
            if (controllerOne != null && !controllerOne.IsConnected) return;
            if (ControlCenterVM.StateVM.CurrentControlMode != ControlMode.Drive) return;

            var newSpeedLeft = CurrentRawControllerSpeedLeft / 255 + 128;
            var newSpeedRight = CurrentRawControllerSpeedRight / 255 + 128;

            if(newSpeedLeft == 128)
            {
                SpeedLeft = newSpeedLeft;
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Motor.CommandId.LeftSpeed, SpeedLeft));
            }
            else if (newSpeedLeft != SpeedLeft)
            {
                SpeedLeft = newSpeedLeft;
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Motor.CommandId.LeftSpeed, SpeedLeft));
            }
            if (newSpeedRight == 128)
            {
                SpeedRight = newSpeedRight;
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Motor.CommandId.CommandedSpeedRight, SpeedRight));
            }
            else if (newSpeedRight != SpeedRight)
            {
                SpeedRight = newSpeedRight;
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Motor.CommandId.CommandedSpeedRight, SpeedRight));
            }
        }

        public enum ArmFunction
        {
            Wrist,
            Elbow
        }
        public enum ArmAction
        {
            Idle,
            WristCounterClockwise,
            WristClockwise,
            WristDown,
            WristUp,
            ElbowCounterClockwise,
            ElbowClockwise,
            ElbowDown,
            ElbowUp,
            ActuatorBack,
            ActuatorForward,
            BaseClockwise,
            BaseCounterclockwise
        }
        private const int BACK = 0;
        private const int FORWARD = 1;
        private const int COUNTERCLOCKWISE = 0;
        private const int CLOCKWISE = 1;
        private const int DOWN = 2;
        private const int UP = 3;
        private const int OPEN = 0;
        private const int CLOSE = 1;

        private ArmFunction currentFunction;
        public ArmFunction CurrentFunction
        {
            get { return currentFunction; }
            set
            {
                SetField(ref currentFunction, value);
                RaisePropertyChanged("CurrentFunctionDisplay");
            }
        }
        public string CurrentFunctionDisplay
        {
            get
            {
                var mode = CurrentFunction;
                return Enum.GetName(typeof(ArmFunction), mode);
            }
        }

        private ArmAction currentAction;
        public ArmAction CurrentAction
        {
            get { return currentAction; }
            set
            {
                SetField(ref currentAction, value);
                RaisePropertyChanged("CurrentActionDisplay");
            }
        }
        public string CurrentActionDisplay
        { 
            get
            {
                var mode = CurrentAction;
                return Enum.GetName(typeof(ArmAction), mode);
            }
        }

        public void NextRoboticArmFunction()
        {
            if (ControlCenterVM.StateVM.CurrentControlMode != ControlMode.RoboticArm) return;
            var functions = Enum.GetNames(typeof(ArmFunction)).ToList();
            var currentIndex = functions.IndexOf(CurrentFunctionDisplay);
            CurrentFunction = currentIndex == functions.Count - 1
                ? ParseEnum<ArmFunction>(functions[0])
                : ParseEnum<ArmFunction>(functions[currentIndex + 1]);
        }
        public void PreviousRoboticArmFunction()
        {
            if (ControlCenterVM.StateVM.CurrentControlMode != ControlMode.RoboticArm) return;
            var functions = Enum.GetNames(typeof(ArmFunction)).ToList();
            var currentIndex = functions.IndexOf(CurrentFunctionDisplay);
            CurrentFunction = currentIndex == 0
                ? ParseEnum<ArmFunction>(functions[functions.Count - 1])
                : ParseEnum<ArmFunction>(functions[currentIndex - 1]);
        }
        
        private void OperateGripper(object sender, ElapsedEventArgs e)
        {
            if (controllerOne != null && !controllerOne.IsConnected) return;
            if (ControlCenterVM.StateVM.CurrentControlMode != ControlMode.RoboticArm) return;

            if (ButtonB)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.Gripper, CLOSE));
                return;
            }
            if (ButtonX)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.Gripper, OPEN));
                return;
            }
        }

        private void OperateArm(object sender, ElapsedEventArgs e)
        {
            if (controllerOne != null && !controllerOne.IsConnected) return;
            if (ControlCenterVM.StateVM.CurrentControlMode != ControlMode.RoboticArm) return;
            
            if(ButtonY)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Reset, 0));
                ControlCenterVM.ConsoleVM.TelemetryReceiver(new Protocol<string>("Robotic Arm Resetting..."));
                return;
            }

            if (joystick2XRaw < 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Wrist, COUNTERCLOCKWISE));
                CurrentAction = ArmAction.WristCounterClockwise;
                return;
            }
            else if (joystick2XRaw > 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Wrist, CLOCKWISE));
                CurrentAction = ArmAction.WristClockwise;
                return;
            }
            else if (joystick2YRaw < 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Wrist, DOWN));
                CurrentAction = ArmAction.WristDown;
                return;
            }
            else if (joystick2YRaw > 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Wrist, UP));
                CurrentAction = ArmAction.WristUp;
                return;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }

            if (joystick1XRaw < 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Elbow, COUNTERCLOCKWISE));
                CurrentAction = ArmAction.ElbowCounterClockwise;
                return;
            }
            else if (joystick1XRaw > 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Elbow, CLOCKWISE));
                CurrentAction = ArmAction.ElbowClockwise;
                return;
            }
            else if (joystick1YRaw < 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Elbow, DOWN));
                CurrentAction = ArmAction.ElbowDown;
                return;
            }
            else if (joystick1YRaw > 0)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Elbow, UP));
                CurrentAction = ArmAction.ElbowUp;
                return;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }

            if(DPadL)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Base, COUNTERCLOCKWISE));
                CurrentAction = ArmAction.BaseCounterclockwise;
            }
            else if (DPadR)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Base, CLOCKWISE));
                CurrentAction = ArmAction.BaseClockwise;
            }
            else if(DPadU)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Actuator, FORWARD));
                CurrentAction = ArmAction.ActuatorForward;
            }
            else if(DPadD)
            {
                GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)RoboticArm.CommandId.Actuator, BACK));
                CurrentAction = ArmAction.ActuatorBack;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }
        }
        
        private void Update(object sender, ElapsedEventArgs e)
        {
            if (controllerOne != null && controllerOne.IsConnected)
            {
                var currentState = controllerOne.GetState();
                Connected = true;

                #region Normalization of joystick input
                var LX = (float)currentState.Gamepad.LeftThumbX;
                var LY = (float)currentState.Gamepad.LeftThumbY;
                var leftMagnitude = (float)Math.Sqrt(LX * LX + LY * LY);
                if (leftMagnitude > Gamepad.LeftThumbDeadZone)
                {
                    //clip the magnitude at its expected maximum value
                    if (leftMagnitude > 32767) leftMagnitude = 32767;

                    //adjust magnitude relative to the end of the dead zone
                    leftMagnitude -= Gamepad.LeftThumbDeadZone;
                }
                else //if the controller is in the deadzone zero out the magnitude
                {
                    leftMagnitude = 0;
                }
                
                var RX = (float)currentState.Gamepad.RightThumbX;
                var RY = (float)currentState.Gamepad.RightThumbY;
                var rightMagnitude = (float)Math.Sqrt(RX * RX + RY * RY);
                if (rightMagnitude > Gamepad.RightThumbDeadZone)
                {
                    //clip the magnitude at its expected maximum value
                    if (rightMagnitude > 32767) rightMagnitude = 32767;

                    //adjust magnitude relative aoeuaoeuaoeuto the end of the dead zone
                    rightMagnitude -= Gamepad.RightThumbDeadZone;
                }
                else //if the controller is in the deadzone zero out the magnitude
                {
                    rightMagnitude = 0;
                }
                
                // Update Working Values
                if (LY < 0)
                {
                    CurrentRawControllerSpeedLeft = (int)-leftMagnitude;
                }
                else
                {
                    CurrentRawControllerSpeedLeft = (int)leftMagnitude;
                }
                if (RY < 0)
                {
                    CurrentRawControllerSpeedRight = (int)-rightMagnitude;
                }
                else
                {
                    CurrentRawControllerSpeedRight = (int)rightMagnitude;
                }
                JoyStick2X = RX / 32767 * 47.5f + 47.5f;
                JoyStick2Y = RY / 32767 * 47.5f + 47.5f;
                JoyStick1X = LX / 32767 * 47.5f + 47.5f;
                JoyStick1Y = LY / 32767 * 47.5f + 47.5f;
                joystick2XRaw = Math.Abs(RX) < Gamepad.RightThumbDeadZone ? 0 : RX;
                joystick2YRaw = Math.Abs(RY) < Gamepad.RightThumbDeadZone ? 0 : RY;
                joystick1XRaw = Math.Abs(LX) < Gamepad.LeftThumbDeadZone ? 0 : LX;
                joystick1YRaw = Math.Abs(LY) < Gamepad.LeftThumbDeadZone ? 0 : LY;
                #endregion

                LeftTrigger = (float)currentState.Gamepad.LeftTrigger / 255;
                RightTrigger = (float)currentState.Gamepad.RightTrigger / 255;
                ButtonA = (currentState.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                ButtonB = (currentState.Gamepad.Buttons & GamepadButtonFlags.B) != 0;
                ButtonX = (currentState.Gamepad.Buttons & GamepadButtonFlags.X) != 0;
                ButtonY = (currentState.Gamepad.Buttons & GamepadButtonFlags.Y) != 0;
                ButtonLb = (currentState.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
                ButtonRb = (currentState.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0;
                ButtonLs = (currentState.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0;
                ButtonRs = (currentState.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0;
                ButtonStart = (currentState.Gamepad.Buttons & GamepadButtonFlags.Start) != 0;
                ButtonBack = (currentState.Gamepad.Buttons & GamepadButtonFlags.Back) != 0;
                DPadL = (currentState.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
                DPadU = (currentState.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
                DPadR = (currentState.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;
                DPadD = (currentState.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
            }
            else
            {
                Connected = false;
            }
        }

        public void TelemetryReceiver<T>(IProtocol<T> message)
        {
            throw new NotImplementedException("Controller Input Module does not currently receive telemetry data.");
        }

    }
}
