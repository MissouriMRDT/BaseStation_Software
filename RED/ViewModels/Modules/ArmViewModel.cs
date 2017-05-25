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

        private readonly ArmModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

        public string Name { get; private set; }
        public string ModeType { get; private set; }
        public IInputDevice InputVM { get; set; }

        public const float Joint1FixedSpeed = .5f;
        public const int Joint2FixedSpeed = 127;

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

        public float CurrentMain
        {
            get
            {
                return _model.CurrentMain;
            }
            set
            {
                _model.CurrentMain = value;
                NotifyOfPropertyChange(() => CurrentMain);
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

            _router.Subscribe(this, _idResolver.GetId("ArmCurrentPosition"));
            _router.Subscribe(this, _idResolver.GetId("ArmFault"));
            _router.Subscribe(this, _idResolver.GetId("ArmCurrentMain"));
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
                case "ArmFault":
                    _log.Log("Arm reported a fault code of " + data[0]);
                    break;
                case "ArmCurrentMain":
                    CurrentMain = BitConverter.ToSingle(data, 0);
                    break;
            }
        }

        public void StartMode()
        { }

        public void SetValues(Dictionary<string, float> values)
        {
            if (values["DebouncedArmReset"] != 0)
            {
                _router.Send(_idResolver.GetId("ArmStop"), (Int16)(0));
                _log.Log("Robotic Arm Resetting...");
            }

            switch (JoystickDirection(values["WristBend"], values["WristTwist"]))
            {
                case JoystickDirections.Right:
                case JoystickDirections.Left:
                    _router.Send(_idResolver.GetId("ArmJ4"), (Int16)(0));
                    _router.Send(_idResolver.GetId("ArmJ5"), (Int16)(values["WristTwist"] * motorRangeFactor));
                    break;
                case JoystickDirections.Up:
                case JoystickDirections.Down:
                    _router.Send(_idResolver.GetId("ArmJ4"), (Int16)(values["WristBend"] * motorRangeFactor));
                    _router.Send(_idResolver.GetId("ArmJ5"), (Int16)(0));
                    break;
                case JoystickDirections.None:
                    _router.Send(_idResolver.GetId("ArmJ4"), (Int16)(0));
                    _router.Send(_idResolver.GetId("ArmJ5"), (Int16)(0));
                    break;
            }

            switch (JoystickDirection(values["ElbowBend"], values["ElbowTwist"]))
            {
                case JoystickDirections.Up:
                case JoystickDirections.Down:
                    _router.Send(_idResolver.GetId("ArmJ3"), (Int16)(values["ElbowBend"] * motorRangeFactor));
                    break;
                case JoystickDirections.Right:
                case JoystickDirections.Left:
                case JoystickDirections.None:
                    _router.Send(_idResolver.GetId("ArmJ3"), (Int16)(0));
                    break;
            }

            Int16 actuatorSpeed = (Int16)twoButtonTransform(values["ShoulderBendForward"] != 0, values["ShoulderBendBackward"] != 0, Joint2FixedSpeed, -Joint2FixedSpeed, 0);
            _router.Send(_idResolver.GetId("ArmJ2"), actuatorSpeed);

            float baseSpeed = (float)twoButtonTransform(values["ShoulderTwistForward"] != 0, values["ShoulderTwistBackward"] != 0, Joint1FixedSpeed, -Joint1FixedSpeed, 0f);
            _router.Send(_idResolver.GetId("ArmJ1"), (Int16)(baseSpeed / 10f * motorRangeFactor));

            float gripperSpeed = (float)twoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, values["GripperClose"], -values["GripperOpen"], 0F);
            _router.Send(_idResolver.GetId("Gripper"), (Int16)(gripperSpeed * EndeffectorSpeedLimit));

            float servoSpeed = (Int16)twoButtonTransform(values["ServoClockwise"] > 0, values["ServoCounterClockwise"] > 0, values["ServoClockwise"], -values["ServoCounterClockwise"], 0F);
            _router.Send(_idResolver.GetId("EndeffectorServo"), (Int16)(servoSpeed * EndeffectorSpeedLimit));

            float towRopeSpeed = (Int16)twoButtonTransform(values["TowRopeOut"] > 0, values["TowRopeIn"] > 0, values["TowRopeOut"], -values["TowRopeIn"], 0F);
            _router.Send(_idResolver.GetId("CarabinerSpeed"), (Int16)(towRopeSpeed * EndeffectorSpeedLimit));
        }

        public void StopMode()
        {
            _router.Send(_idResolver.GetId("ArmStop"), (Int16)(0));
            _router.Send(_idResolver.GetId("Gripper"), (Int16)(0));
            _router.Send(_idResolver.GetId("EndeffectorServo"), (Int16)(0));
            _router.Send(_idResolver.GetId("CarabinerSpeed"), (Int16)(0));
        }

        public void EnableCommand(string bus, bool enableState)
        {
            ushort id;
            switch (bus)
            {
                case "All": id = _idResolver.GetId("ArmEnableAll"); break;
                case "Main": id = _idResolver.GetId("ArmEnableMain"); break;
                case "J12": id = _idResolver.GetId("ArmEnableJ1"); break;
                case "J3": id = _idResolver.GetId("ArmEnableJ3"); break;
                case "J45": id = _idResolver.GetId("ArmEnableJ4"); break;
                case "Endeff": id = _idResolver.GetId("ArmEnableEndeff"); break;
                case "Servo": id = _idResolver.GetId("ArmEnableServo"); break;
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
            if (x == 0.0f && y == 0.0f) return JoystickDirections.None;

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

        private T twoButtonTransform<T>(bool bool1, bool bool2, T val1, T val2, T val0)
        {
            return bool1 ? val1 : (bool2 ? val2 : val0);
        }
    }
}