using Caliburn.Micro;
using RED.Configurations.Modules;
using RED.Contexts.Modules;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using RED.ViewModels.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace RED.ViewModels.Modules
{
    public class ArmViewModel : PropertyChangedBase, IInputMode, ISubscribe
    {
        private const string PositionsConfigName = "ArmPositions";

        private const byte ArmDisableCommand = 0x00;
        private const byte ArmEnableCommand = 0x01;

        private const short MotorRangeFactor = 1000;

        private readonly ArmModel _model;
        private readonly IDataRouter _router;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private readonly IConfigurationManager _configManager;

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

        public ObservableCollection<ArmPositionViewModel> Positions
        {
            get
            {
                return _model.Positions;
            }
            private set
            {
                _model.Positions = value;
                NotifyOfPropertyChange(() => Positions);
            }
        }
        public ArmPositionViewModel SelectedPosition
        {
            get
            {
                return _model.SelectedPosition;
            }
            set
            {
                _model.SelectedPosition = value;
                NotifyOfPropertyChange(() => SelectedPosition);
            }
        }

        public ArmViewModel(IInputDevice inputVM, IDataRouter router, IDataIdResolver idResolver, ILogger log, IConfigurationManager configs)
        {
            _model = new ArmModel();
            InputVM = inputVM;
            _router = router;
            _idResolver = idResolver;
            _log = log;
            _configManager = configs;
            Name = "Arm";
            ModeType = "Arm";

            _configManager.AddRecord(PositionsConfigName, ArmConfig.DefaultArmPositions);
            InitializePositions(_configManager.GetConfig<ArmPositionsContext>(PositionsConfigName));

            _router.Subscribe(this, _idResolver.GetId("ArmCurrentPosition"));
            _router.Subscribe(this, _idResolver.GetId("ArmFault"));
            _router.Subscribe(this, _idResolver.GetId("ArmCurrentMain"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data, bool reliable)
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
                    _log.Log("Arm reported a fault code of {0}", data[0]);
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
                _router.Send(_idResolver.GetId("ArmStop"), (Int16)(0), true);
                _log.Log("Robotic Arm Resetting...");
            }

            switch (JoystickDirection(values["WristBend"], values["WristTwist"]))
            {
                case JoystickDirections.Right:
                case JoystickDirections.Left:
                    _router.Send(_idResolver.GetId("ArmJ4"), (Int16)(0));
                    _router.Send(_idResolver.GetId("ArmJ5"), (Int16)(values["WristTwist"] * MotorRangeFactor));
                    break;
                case JoystickDirections.Up:
                case JoystickDirections.Down:
                    _router.Send(_idResolver.GetId("ArmJ4"), (Int16)(values["WristBend"] * MotorRangeFactor));
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
                    _router.Send(_idResolver.GetId("ArmJ3"), (Int16)(values["ElbowBend"] * MotorRangeFactor));
                    break;
                case JoystickDirections.Right:
                case JoystickDirections.Left:
                case JoystickDirections.None:
                    _router.Send(_idResolver.GetId("ArmJ3"), (Int16)(0));
                    break;
            }

            Int16 actuatorSpeed = (Int16)ControllerBase.TwoButtonTransform(values["ShoulderBendForward"] != 0, values["ShoulderBendBackward"] != 0, Joint2FixedSpeed, -Joint2FixedSpeed, 0);
            _router.Send(_idResolver.GetId("ArmJ2"), actuatorSpeed);

            float baseSpeed = (float)ControllerBase.TwoButtonTransform(values["ShoulderTwistForward"] != 0, values["ShoulderTwistBackward"] != 0, Joint1FixedSpeed, -Joint1FixedSpeed, 0f);
            _router.Send(_idResolver.GetId("ArmJ1"), (Int16)(baseSpeed / 10f * MotorRangeFactor));

            float gripperSpeed = (float)ControllerBase.TwoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, values["GripperClose"], -values["GripperOpen"], 0F);
            _router.Send(_idResolver.GetId("Gripper"), (Int16)(gripperSpeed * EndeffectorSpeedLimit));

            float servoSpeed = (Int16)ControllerBase.TwoButtonTransform(values["ServoClockwise"] > 0, values["ServoCounterClockwise"] > 0, values["ServoClockwise"], -values["ServoCounterClockwise"], 0F);
            _router.Send(_idResolver.GetId("EndeffectorServo"), (Int16)(servoSpeed * EndeffectorSpeedLimit));

            float towRopeSpeed = (Int16)ControllerBase.TwoButtonTransform(values["TowRopeOut"] > 0, values["TowRopeIn"] > 0, values["TowRopeOut"], -values["TowRopeIn"], 0F);
            _router.Send(_idResolver.GetId("CarabinerSpeed"), (Int16)(towRopeSpeed * EndeffectorSpeedLimit));
        }

        public void StopMode()
        {
            _router.Send(_idResolver.GetId("ArmStop"), (Int16)(0), true);
            _router.Send(_idResolver.GetId("Gripper"), (Int16)(0), true);
            _router.Send(_idResolver.GetId("EndeffectorServo"), (Int16)(0), true);
            _router.Send(_idResolver.GetId("CarabinerSpeed"), (Int16)(0), true);
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

            _router.Send(id, (enableState) ? ArmEnableCommand : ArmDisableCommand, true);
        }

        public void GetPosition()
        {
            _router.Send(_idResolver.GetId("ArmGetPosition"), new byte[0], true);
        }
        public void SetPosition()
        {
            float[] angles = { AngleJ1, AngleJ2, AngleJ3, AngleJ4, AngleJ5, AngleJ6 };
            byte[] data = new byte[angles.Length * sizeof(float)];
            Buffer.BlockCopy(angles, 0, data, 0, data.Length);
            _router.Send(_idResolver.GetId("ArmAbsoluteAngle"), data, true);
        }

        public void LimitSwitchOverride(byte index)
        {
            _router.Send(_idResolver.GetId("LimitSwitchOverride"), index, true);
        }
        public void LimitSwitchUnOverride(byte index)
        {
            _router.Send(_idResolver.GetId("LimitSwitchUnOverride"), index, true);
        }

        public void RecallPosition()
        {
            AngleJ1 = SelectedPosition.J1;
            AngleJ2 = SelectedPosition.J2;
            AngleJ3 = SelectedPosition.J3;
            AngleJ4 = SelectedPosition.J4;
            AngleJ5 = SelectedPosition.J5;
            AngleJ6 = SelectedPosition.J6;
        }
        public void StorePosition()
        {
            Positions.Add(new ArmPositionViewModel()
                {
                    Name = "Unnamed Position",
                    J1 = AngleJ1,
                    J2 = AngleJ2,
                    J3 = AngleJ3,
                    J4 = AngleJ4,
                    J5 = AngleJ5,
                    J6 = AngleJ6
                });
        }
        public void DeletePosition()
        {
            Positions.Remove(SelectedPosition);
        }
        public void SaveConfigurations()
        {
            _configManager.SetConfig(PositionsConfigName, new ArmPositionsContext(Positions.Select(x => x.GetContext()).ToArray()));
        }
        public void InitializePositions(ArmPositionsContext config)
        {
            foreach (var position in config.Positions)
                Positions.Add(new ArmPositionViewModel(position));
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

        public class ArmPositionViewModel : PropertyChangedBase
        {
            private readonly ArmModel.ArmPositionModel _model;

            public string Name
            {
                get
                {
                    return _model.Name;
                }
                set
                {
                    _model.Name = value; NotifyOfPropertyChange(() => Name);
                }

            }
            public float J1
            {
                get
                {
                    return _model.J1;
                }
                set
                {
                    _model.J1 = value; NotifyOfPropertyChange(() => J1);
                }

            }
            public float J2
            {
                get
                {
                    return _model.J2;
                }
                set
                {
                    _model.J2 = value; NotifyOfPropertyChange(() => J2);
                }

            }
            public float J3
            {
                get
                {
                    return _model.J3;
                }
                set
                {
                    _model.J3 = value; NotifyOfPropertyChange(() => J3);
                }

            }
            public float J4
            {
                get
                {
                    return _model.J4;
                }
                set
                {
                    _model.J4 = value; NotifyOfPropertyChange(() => J4);
                }

            }
            public float J5
            {
                get
                {
                    return _model.J5;
                }
                set
                {
                    _model.J5 = value; NotifyOfPropertyChange(() => J5);
                }

            }
            public float J6
            {
                get
                {
                    return _model.J6;
                }
                set
                {
                    _model.J6 = value; NotifyOfPropertyChange(() => J6);
                }

            }

            public ArmPositionViewModel()
            {
                _model = new ArmModel.ArmPositionModel();
            }

            public ArmPositionViewModel(ArmPositionContext ctx)
                : this()
            {
                Name = ctx.Name;
                J1 = ctx.J1;
                J2 = ctx.J2;
                J3 = ctx.J3;
                J4 = ctx.J4;
                J5 = ctx.J5;
                J6 = ctx.J6;
            }

            public ArmPositionContext GetContext()
            {
                return new ArmPositionContext(Name, J1, J2, J3, J4, J5, J6);
            }
        }
    }
}