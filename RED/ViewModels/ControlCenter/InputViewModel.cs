namespace RED.ViewModels.ControlCenter
{
    using Annotations;
    using Caliburn.Micro;
    using Models;
    using SharpDX.XInput;
    using System;
    using System.Linq;
    using System.Timers;

    public class InputViewModel : PropertyChangedBase
    {
        private readonly InputModel Model = new InputModel();
        private readonly ControlCenterViewModel _controlCenter;
        [CanBeNull]
        public readonly Controller ControllerOne = new Controller(UserIndex.One);

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
                return Model.SerialReadSpeed;
            }
            set
            {
                Model.SerialReadSpeed = value;
                NotifyOfPropertyChange(() => SerialReadSpeed);
            }
        }
        public int DriveCommandSpeedMs
        {
            get
            {
                return Model.DriveCommandSpeed;
            }
            set
            {
                Model.DriveCommandSpeed = value;
                NotifyOfPropertyChange(() => DriveCommandSpeedMs);
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
                if (Model.ButtonRb != value && value)
                    NextRoboticArmFunction();
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
                if (Model.ButtonLb != value && value)
                    PreviousRoboticArmFunction();
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
                    _controlCenter.StateManager.NextControlMode();
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
                    _controlCenter.StateManager.PreviousControlMode();
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
        #region Controller Working Values

        public int CurrentRawControllerSpeedLeft
        {
            get
            {
                return Model.CurrentRawControllerSpeedLeft;
            }
            set
            {
                Model.CurrentRawControllerSpeedLeft = value;
                NotifyOfPropertyChangeThreadSafe(() => CurrentRawControllerSpeedLeft);
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
                Model.CurrentRawControllerSpeedRight = value;
                NotifyOfPropertyChangeThreadSafe(() => CurrentRawControllerSpeedRight);
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
                Model.SpeedLeft = value;
                NotifyOfPropertyChangeThreadSafe(() => SpeedLeft);
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
                Model.SpeedRight = value;
                NotifyOfPropertyChangeThreadSafe(() => SpeedRight);
            }
        }

        #endregion

        public InputViewModel(ControlCenterViewModel cc)
        {
            _controlCenter = cc;

            SpeedLeft = 128;
            SpeedRight = 128;
            IsFullSpeed = false;

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
            if (ControllerOne != null && !ControllerOne.IsConnected) return;
            if (_controlCenter.StateManager.CurrentControlMode != ControlMode.Drive) return;

            var newSpeedLeft = CurrentRawControllerSpeedLeft / 255 + 128;
            var newSpeedRight = CurrentRawControllerSpeedRight / 255 + 128;

            if (newSpeedLeft == 128)
            {
                SpeedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, SpeedLeft);
            }
            else if (newSpeedLeft != SpeedLeft)
            {
                if (!IsFullSpeed)
                {
                    if (newSpeedLeft > 150)
                        SpeedLeft = 150;
                    else if (newSpeedLeft < 106)
                        SpeedLeft = 106;
                    else
                        SpeedLeft = newSpeedLeft;
                }
                else
                {
                    SpeedLeft = newSpeedLeft;
                }
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, SpeedLeft);
            }
            if (newSpeedRight == 128)
            {
                SpeedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorCommandedSpeedRight").Id, SpeedRight);
            }
            else if (newSpeedRight != SpeedRight)
            {
                if (!IsFullSpeed)
                {
                    if (newSpeedRight > 150)
                        SpeedRight = 150;
                    else if (newSpeedRight < 106)
                        SpeedRight = 106;
                    else
                        SpeedRight = newSpeedRight;
                }
                else
                {
                    SpeedRight = newSpeedRight;
                }
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorCommandedSpeedRight").Id, SpeedRight);
            }
        }

        public bool IsFullSpeed
        {
            get
            {
                return Model.isFullSpeed;
            }
            set
            {
                Model.isFullSpeed = value;
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
                currentFunction = value;
                NotifyOfPropertyChangeThreadSafe(() => CurrentFunction);
                NotifyOfPropertyChangeThreadSafe(() => CurrentFunctionDisplay);

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
                currentAction = value;
                NotifyOfPropertyChangeThreadSafe(() => CurrentAction);
                NotifyOfPropertyChangeThreadSafe(() => CurrentActionDisplay);
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
            if (_controlCenter.StateManager.CurrentControlMode != ControlMode.RoboticArm) return;
            var functions = Enum.GetNames(typeof(ArmFunction)).ToList();
            var currentIndex = functions.IndexOf(CurrentFunctionDisplay);
            CurrentFunction = currentIndex == functions.Count - 1
                ? ParseEnum<ArmFunction>(functions[0])
                : ParseEnum<ArmFunction>(functions[currentIndex + 1]);
        }
        public void PreviousRoboticArmFunction()
        {
            if (_controlCenter.StateManager.CurrentControlMode != ControlMode.RoboticArm) return;
            var functions = Enum.GetNames(typeof(ArmFunction)).ToList();
            var currentIndex = functions.IndexOf(CurrentFunctionDisplay);
            CurrentFunction = currentIndex == 0
                ? ParseEnum<ArmFunction>(functions[functions.Count - 1])
                : ParseEnum<ArmFunction>(functions[currentIndex - 1]);
        }

        private void OperateGripper(object sender, ElapsedEventArgs e)
        {
            if (ControllerOne != null && !ControllerOne.IsConnected) return;
            if (_controlCenter.StateManager.CurrentControlMode != ControlMode.RoboticArm) return;

            if (ButtonB)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("AuxGripper").Id, CLOSE);
                return;
            }
            if (ButtonX)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("AuxGripper").Id, OPEN);
                return;
            }
        }

        private void OperateArm(object sender, ElapsedEventArgs e)
        {
            if (ControllerOne != null && !ControllerOne.IsConnected) return;
            if (_controlCenter.StateManager.CurrentControlMode != ControlMode.RoboticArm) return;

            if (ButtonY)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmReset").Id, 0);
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
                return;
            }

            if (JoyStick2X < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, COUNTERCLOCKWISE);
                CurrentAction = ArmAction.WristCounterClockwise;
                return;
            }
            else if (JoyStick2X > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, CLOCKWISE);
                CurrentAction = ArmAction.WristClockwise;
                return;
            }
            else if (JoyStick2Y < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, DOWN);
                CurrentAction = ArmAction.WristDown;
                return;
            }
            else if (JoyStick2Y > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, UP);
                CurrentAction = ArmAction.WristUp;
                return;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }

            if (JoyStick1X < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, COUNTERCLOCKWISE);
                CurrentAction = ArmAction.ElbowCounterClockwise;
                return;
            }
            else if (JoyStick1X > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, CLOCKWISE);
                CurrentAction = ArmAction.ElbowClockwise;
                return;
            }
            else if (JoyStick1Y < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, DOWN);
                CurrentAction = ArmAction.ElbowDown;
                return;
            }
            else if (JoyStick1Y > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, UP);
                CurrentAction = ArmAction.ElbowUp;
                return;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }

            if (DPadL)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, COUNTERCLOCKWISE);
                CurrentAction = ArmAction.BaseCounterclockwise;
            }
            else if (DPadR)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, CLOCKWISE);
                CurrentAction = ArmAction.BaseClockwise;
            }
            else if (DPadU)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, FORWARD);
                CurrentAction = ArmAction.ActuatorForward;
            }
            else if (DPadD)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, BACK);
                CurrentAction = ArmAction.ActuatorBack;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            if (ControllerOne != null && ControllerOne.IsConnected)
            {
                var currentState = ControllerOne.GetState();
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
                JoyStick2X = currentState.Gamepad.RightThumbX < Gamepad.RightThumbDeadZone ? 0 : (float)RX / 32767;
                JoyStick2Y = currentState.Gamepad.RightThumbY < Gamepad.RightThumbDeadZone ? 0 : (float)RY / 32767;
                JoyStick1X = currentState.Gamepad.LeftThumbX < Gamepad.LeftThumbDeadZone ? 0 : (float)LX / 32767;
                JoyStick1Y = currentState.Gamepad.LeftThumbY < Gamepad.LeftThumbDeadZone ? 0 : (float)LY / 32767;
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

        private T ParseEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
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