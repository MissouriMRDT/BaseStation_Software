using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class ArmViewModel : PropertyChangedBase, IInputMode, ISubscribe
    {
        private const byte ArmDisableCommand = 0x00;
        private const byte ArmEnableCommand = 0x01;

        private const short motorRangeFactor = 1000;
        private readonly EndEffectorModes[] AvailibleEndEffectorModes = { EndEffectorModes.Gripper, EndEffectorModes.Drill, EndEffectorModes.RegulatorDetach };
        private readonly string[] EndEffectorModeNames = { "Gripper", "Drill", "Regulator Detachment" };

        private readonly ArmModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

        public string Name { get; private set; }
        public string ModeType { get; private set; }
        public IInputDevice InputVM { get; set; }

        public const float BaseServoSpeed = .5f;
        public const int BaseActuatorSpeed = 127;

        public int CurrentEndEffectorMode
        {
            get
            {
                return _model.CurrentEndEffectorMode;
            }
            set
            {
                _model.CurrentEndEffectorMode = value;
                NotifyOfPropertyChange(() => CurrentEndEffectorMode);
                NotifyOfPropertyChange(() => CurrentEndEffectorModeName);
            }
        }
        public string CurrentEndEffectorModeName
        {
            get
            {
                return EndEffectorModeNames[CurrentEndEffectorMode];
            }
        }

        public float AngleJ1
        {
            get
            {
                return _model.AngleJ1;
            }
            set
            {
                _model.AngleJ1 = value;
                NotifyOfPropertyChange(() => AngleJ1);
            }
        }
        public float AngleJ2
        {
            get
            {
                return _model.AngleJ2;
            }
            set
            {
                _model.AngleJ2 = value;
                NotifyOfPropertyChange(() => AngleJ2);
            }
        }
        public float AngleJ3
        {
            get
            {
                return _model.AngleJ3;
            }
            set
            {
                _model.AngleJ3 = value;
                NotifyOfPropertyChange(() => AngleJ3);
            }
        }
        public float AngleJ4
        {
            get
            {
                return _model.AngleJ4;
            }
            set
            {
                _model.AngleJ4 = value;
                NotifyOfPropertyChange(() => AngleJ4);
            }
        }
        public float AngleJ5
        {
            get
            {
                return _model.AngleJ5;
            }
            set
            {
                _model.AngleJ5 = value;
                NotifyOfPropertyChange(() => AngleJ5);
            }
        }
        public float AngleJ6
        {
            get
            {
                return _model.AngleJ6;
            }
            set
            {
                _model.AngleJ6 = value;
                NotifyOfPropertyChange(() => AngleJ6);
            }
        }

        public int EndeffectorSpeedLimit
        {
            get
            {
                return _model.EndeffectorSpeedLimit;
            }
            set
            {
                _model.EndeffectorSpeedLimit = value;
                NotifyOfPropertyChange(() => EndeffectorSpeedLimit);
            }
        }

        public ArmViewModel(IInputDevice inputVM, IDataRouter router, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ArmModel();
            InputVM = inputVM;
            _router = router;
            _idResolver = idResolver;
            _log = log;
            Name = "Arm";
            ModeType = "Arm";
            CurrentEndEffectorMode = 0;

            _router.Subscribe(this, _idResolver.GetId("ArmCurrentPosition"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "ArmCurrentPosition":
                    AngleJ1 = BitConverter.ToSingle(data, 0 * sizeof(float));
                    AngleJ2 = BitConverter.ToSingle(data, 1 * sizeof(float));
                    AngleJ3 = BitConverter.ToSingle(data, 2 * sizeof(float));
                    AngleJ4 = BitConverter.ToSingle(data, 3 * sizeof(float));
                    AngleJ5 = BitConverter.ToSingle(data, 4 * sizeof(float));
                    AngleJ6 = BitConverter.ToSingle(data, 5 * sizeof(float));
                    break;
            }
        }

        public void StartMode()
        {

        }

        public void SetValues(Dictionary<string, float> values)
        {
            if (values["DebouncedArmReset"] != 0)
            {
                _router.Send(_idResolver.GetId("ArmStop"), (Int16)(0));
                _log.Log("Robotic Arm Resetting...");
            }

            if (values["DebouncedToolPrev"] != 0)
                PreviousEndeffectorMode();
            else if (values["DebouncedToolNext"] != 0)
                NextEndeffectorMode();

            switch (JoystickDirection(values["WristBend"], values["WristTwist"]))
            {
                case JoystickDirections.Right:
                case JoystickDirections.Left:
                    _router.Send(_idResolver.GetId("ArmWristClockwise"), (Int16)(values["WristTwist"] * motorRangeFactor));
                    break;
                case JoystickDirections.Up:
                case JoystickDirections.Down:
                    _router.Send(_idResolver.GetId("ArmWristUp"), (Int16)(-values["WristBend"] * motorRangeFactor));
                    break;
                case JoystickDirections.None:
                    _router.Send(_idResolver.GetId("ArmWristUp"), (Int16)(0));
                    break;
            }

            switch (JoystickDirection(values["ElbowBend"], values["ElbowTwist"]))
            {
                case JoystickDirections.Right:
                case JoystickDirections.Left:
                    _router.Send(_idResolver.GetId("ArmElbowClockwise"), (Int16)(values["ElbowTwist"] * motorRangeFactor));
                    break;
                case JoystickDirections.Up:
                case JoystickDirections.Down:
                    _router.Send(_idResolver.GetId("ArmElbowUp"), (Int16)(-values["ElbowBend"] * motorRangeFactor));
                    break;
                case JoystickDirections.None:
                    _router.Send(_idResolver.GetId("ArmWristUp"), (Int16)(0));
                    break;
            }

            Func<bool, bool, Int16, Int16, Int16, Int16> twoButtonTransform = (bool1, bool2, val1, val2, val0) => bool1 ? val1 : (bool2 ? val2 : val0);

            Int16 actuatorSpeed = twoButtonTransform(values["ActuatorForward"] != 0, values["ActuatorBackward"] != 0, BaseActuatorSpeed, -BaseActuatorSpeed, 0);
            _router.Send(_idResolver.GetId("ArmBaseActuatorForward"), actuatorSpeed);

            float baseSpeed = 0;
            if (values["ActuatorForward"] != 0) baseSpeed = BaseServoSpeed;
            else if (values["ActuatorBackward"] != 0) baseSpeed = -BaseServoSpeed;
            _router.Send(_idResolver.GetId("ArmBaseServoClockwise"), (Int16)(baseSpeed / 10f * motorRangeFactor));

            switch (AvailibleEndEffectorModes[CurrentEndEffectorMode])
            {
                case EndEffectorModes.Gripper:
                    {
                        Int16 gripperSpeed = twoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, (Int16)(values["GripperClose"] * EndeffectorSpeedLimit), (Int16)(-values["GripperOpen"] * EndeffectorSpeedLimit), 0);
                        _router.Send(_idResolver.GetId("Gripper"), gripperSpeed);
                        break;
                    }
                case EndEffectorModes.Drill:
                    {
                        Int16 drillSpeed = twoButtonTransform(values["DrillClockwise"] != 0, values["DrillCounterClockwise"] != 0, (Int16)DrillCommands.Forward, (Int16)DrillCommands.Reverse, (Int16)DrillCommands.Stop);
                        _router.Send(_idResolver.GetId("Drill"), drillSpeed);
                        break;
                    }
                case EndEffectorModes.RegulatorDetach:
                    {
                        Int16 gripperSpeed = twoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, (Int16)(values["GripperClose"] * EndeffectorSpeedLimit), (Int16)(-values["GripperOpen"] * EndeffectorSpeedLimit), 0);
                        _router.Send(_idResolver.GetId("Gripper"), gripperSpeed);

                        Int16 drillSpeed = twoButtonTransform(values["DrillClockwise"] != 0, values["DrillCounterClockwise"] != 0, (Int16)DrillCommands.Forward, (Int16)DrillCommands.Reverse, (Int16)DrillCommands.Stop);
                        _router.Send(_idResolver.GetId("RegulatorDetach"), drillSpeed);
                        break;
                    }
            }
        }

        public void StopMode()
        {
            _router.Send(_idResolver.GetId("ArmStop"), (Int16)(0));
        }

        public void NextEndeffectorMode()
        {
            CurrentEndEffectorMode = (CurrentEndEffectorMode + 1) % AvailibleEndEffectorModes.Length;
            _log.Log("Switched to Next Endeffector Mode");
        }
        public void PreviousEndeffectorMode()
        {
            CurrentEndEffectorMode = (CurrentEndEffectorMode + 1 + AvailibleEndEffectorModes.Length) % AvailibleEndEffectorModes.Length;
            _log.Log("Switched to Previous Endeffector Mode");
        }

        public void EnableCommand(string bus, bool enableState)
        {
            ushort id;
            switch (bus)
            {
                case "All": id = _idResolver.GetId("ArmEnableAll"); break;
                case "Main": id = _idResolver.GetId("ArmEnableMain"); break;
                case "J1": id = _idResolver.GetId("ArmEnableJ1"); break;
                case "J2": id = _idResolver.GetId("ArmEnableJ2"); break;
                case "J34": id = _idResolver.GetId("ArmEnableJ34"); break;
                case "J56": id = _idResolver.GetId("ArmEnableJ56"); break;
                case "Endeff": id = _idResolver.GetId("ArmEnableEndeff"); break;
                default: return;
            }

            _router.Send(id, (enableState) ? ArmEnableCommand : ArmDisableCommand);
        }

        public void GetPosition()
        {
            _router.Send(_idResolver.GetId("ArmGetPosition"), new byte[0]);
        }
        public void SetPosition()
        {
            float[] angles = { AngleJ1, AngleJ2, AngleJ3, AngleJ4, AngleJ5, AngleJ6 };
            byte[] data = new byte[angles.Length * sizeof(float)];
            Buffer.BlockCopy(angles, 0, data, 0, data.Length);
            _router.Send(_idResolver.GetId("ArmAbsoluteAngle"), data);
        }

        private JoystickDirections JoystickDirection(float y, float x)
        {
            var angle = Math.Atan2(y, x);
            if (angle > -Math.PI / 6 && angle < Math.PI / 6)
                return JoystickDirections.Right;
            else if (angle > Math.PI / 3 && angle < 2 * Math.PI / 3)
                return JoystickDirections.Up;
            else if (angle > 5 * Math.PI / 6 || angle < -5 * Math.PI / 6)
                return JoystickDirections.Left;
            else if (angle > -2 * Math.PI / 3 && angle < Math.PI / 3)
                return JoystickDirections.Down;
            else
                return JoystickDirections.None;
        }

        private enum JoystickDirections
        {
            None = 0,
            Left,
            Right,
            Up,
            Down
        }
    }

    public enum EndEffectorModes
    {
        None = 0,
        Gripper = 1,
        Drill = 2,
        RegulatorDetach = 3
    }

    public enum DrillCommands : short
    {
        Stop = 0,
        Forward = 1,
        Reverse = 2
    }
}