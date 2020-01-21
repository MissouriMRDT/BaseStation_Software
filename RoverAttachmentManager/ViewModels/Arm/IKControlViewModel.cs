using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.Arm;
using RoverAttachmentManager.Models.ArmModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static RoverAttachmentManager.ViewModels.Arm.ArmViewModel;

namespace RoverAttachmentManager.ViewModels.Arm
{
    class IKControlViewModel : PropertyChangedBase, IRovecommReceiver
    {

        private bool guiControlInitialized;

        private enum ArmControlState
        {
            OpenLoop,
            IKRoverPOV, //point of view
            IKWristPOV,
            GuiControl
        };

        ArmControlState myState;

        private const string PositionsConfigName = "ArmPositions";

        private const byte ArmDisableCommand = 0x00;
        private const byte ArmEnableCommand = 0x01;

        private readonly byte[] ArmEncoderFaultIds = { 8, 9, 10, 11, 12, 13 };
        private readonly IKControlModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private readonly IConfigurationManager _configManager;
        private readonly Dictionary<int, string> _armFaultIds;

        public string ControlState
        {
            get
            {
                return _model.ControlState;
            }
            set
            {
                _model.ControlState = value;
                NotifyOfPropertyChange(() => ControlState);
            }
        }
        public int IKRangeFactor
        {
            get
            {
                return _model.IKRangeFactor;
            }
            set
            {
                _model.IKRangeFactor = value;
                NotifyOfPropertyChange(() => IKRangeFactor);
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
        public float CoordinateX
        {
            get
            {
                return _model.CoordinateX;
            }
            set
            {
                _model.CoordinateX = value;
                NotifyOfPropertyChange(() => CoordinateX);
            }
        }
        public float CoordinateY
        {
            get
            {
                return _model.CoordinateY;
            }
            set
            {
                _model.CoordinateY = value;
                NotifyOfPropertyChange(() => CoordinateY);
            }
        }
        public float CoordinateZ
        {
            get
            {
                return _model.CoordinateZ;
            }
            set
            {
                _model.CoordinateZ = value;
                NotifyOfPropertyChange(() => CoordinateZ);
            }
        }
        public float Yaw
        {
            get
            {
                return _model.Yaw;
            }
            set
            {
                _model.Yaw = value;
                NotifyOfPropertyChange(() => Yaw);
            }
        }
        public float Pitch
        {
            get
            {
                return _model.Pitch;
            }
            set
            {
                _model.Pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }
        public float Roll
        {
            get
            {
                return _model.Roll;
            }
            set
            {
                _model.Roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }
        public float OpX
        {
            get
            {
                return _model.OpX;
            }
            set
            {
                _model.OpX = value;
                NotifyOfPropertyChange(() => OpX);
            }
        }
        public float OpY
        {
            get
            {
                return _model.OpY;
            }
            set
            {
                _model.OpY = value;
                NotifyOfPropertyChange(() => OpY);
            }
        }
        public float OpZ
        {
            get
            {
                return _model.OpZ;
            }
            set
            {
                _model.OpZ = value;
                NotifyOfPropertyChange(() => OpZ);
            }
        }
        public byte SelectedTool
        {
            get
            {
                return _model.SelectedTool;
            }
            set
            {
                _model.SelectedTool = value;
                NotifyOfPropertyChange(() => SelectedTool);
            }
        }
        public ControlMultipliersViewModel ControlMultipliers
        {
            get
            {
                return _model._controlMultipliers;
            }
            set
            {
                _model._controlMultipliers = value;
                NotifyOfPropertyChange(() => ControlMultipliers);
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

        byte previousTool;
        bool laser = false;

        //flag that gets set when the arm detects and error and forces a change state, we want the user to have to wait a second 
        //before commands can be sent again so they can register the fact taht said error occurred
        private bool freezeArm = false;

        public IKControlViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, IConfigurationManager configs, ArmViewModel parent)
        {
            _model = new IKControlModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            _configManager = configs;

            ControlMultipliers = parent.ControlMultipliers;

            myState = ArmControlState.GuiControl;
            ControlState = "GUI control";
            previousTool = 0;

            _configManager.AddRecord(PositionsConfigName, ArmConfig.DefaultArmPositions);
            InitializePositions(_configManager.GetConfig<ArmPositionsContext>(PositionsConfigName));

            _rovecomm.NotifyWhenMessageReceived(this, "ArmCurrentPosition");
            _rovecomm.NotifyWhenMessageReceived(this, "ArmFault");
            _rovecomm.NotifyWhenMessageReceived(this, "ArmCurrentXYZ");
            _rovecomm.NotifyWhenMessageReceived(this, "ArmAngles");

            _armFaultIds = new Dictionary<int, string>
            {
                { 1, "Motor 1 fault" },
                { 2, "Motor 2 fault" },
                { 3, "Motor 3 fault" },
                { 4, "Motor 4 fault" },
                { 5, "Motor 5 fault" },
                { 6, "Motor 6 fault" },
                { 7, "Arm Master Overcurrent" },
                { 8, "Base Rotate encoder disconnected" },
                { 9, "Base Tilt encoder disconnected" },
                { 10, "Elbow Tilt encoder disconnected" },
                { 11, "Elbow Rotate encoder disconnected" },
                { 12, "Wrist Tilt encoder disconnected" },
                { 13, "Wrist Rotate encoder disconnected" }
            };
        }
        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {

                case "ArmCurrentXYZ":
                    CoordinateX = BitConverter.ToSingle(packet.Data, 0 * sizeof(float));
                    CoordinateY = BitConverter.ToSingle(packet.Data, 1 * sizeof(float));
                    CoordinateZ = BitConverter.ToSingle(packet.Data, 2 * sizeof(float));
                    Yaw = BitConverter.ToSingle(packet.Data, 3 * sizeof(float));
                    Pitch = BitConverter.ToSingle(packet.Data, 4 * sizeof(float));
                    Roll = BitConverter.ToSingle(packet.Data, 5 * sizeof(float));
                    break;
                case "ArmFault":
                    _log.Log($"Arm fault: {_armFaultIds[packet.Data[0]]}");

                    //Arm will automatically exit closed loop mode when it detects an encoder fault
                    //so we make sure to stop spamming closed loop messages at it, as we do in IK control states.
                    if (ArmEncoderFaultIds.Contains(packet.Data[0]) && (myState == ArmControlState.IKRoverPOV || myState == ArmControlState.IKWristPOV))
                    {
                        myState = ArmControlState.OpenLoop;
                        ControlState = "Open loop";
                        freezeArm = true;
                    }
                    break;
            }
        }
        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
        private void SetIKValues(Dictionary<string, float> values, ArmControlState stateToUse)
        {
            Int16 X = 0;
            Int16 Y = 0;
            Int16 Z = 0;
            Int16 Yaw = 0;
            Int16 Pitch = 0;
            Int16 Roll = 0;
            Int16 Gripper = 0;
            Int16 Nipper = 0;

            switch (ControllerBase.JoystickDirection(values["IKYawIncrement"], values["IKPitchIncrement"]))
            {
                case ControllerBase.JoystickDirections.Right:
                case ControllerBase.JoystickDirections.Left:
                case ControllerBase.JoystickDirections.Up:
                case ControllerBase.JoystickDirections.Down:
                    Yaw = (Int16)(values["IKYawIncrement"] * IKRangeFactor);
                    Pitch = (Int16)(values["IKPitchIncrement"] * IKRangeFactor);
                    break;
                case ControllerBase.JoystickDirections.None:
                    Yaw = 0;
                    Pitch = 0;
                    break;
            }

            switch (ControllerBase.JoystickDirection(values["IKXIncrement"], values["IKYIncrement"]))
            {
                case ControllerBase.JoystickDirections.Up:
                case ControllerBase.JoystickDirections.Down:
                    Y = 0;
                    X = (Int16)(values["IKXIncrement"] * IKRangeFactor);
                    break;
                case ControllerBase.JoystickDirections.Right:
                case ControllerBase.JoystickDirections.Left:
                    Y = (Int16)(values["IKYIncrement"] * IKRangeFactor);
                    X = 0;
                    break;
                case ControllerBase.JoystickDirections.None:
                    Y = 0;
                    X = 0;
                    break;
            }


            Z = (Int16)(ControllerBase.TwoButtonToggleDirection(values["IKZDirection"] != 0, (values["IKZMagnitude"])) * IKRangeFactor);
            Roll = (Int16)(ControllerBase.TwoButtonToggleDirection(values["IKRollDirection"] != 0, (values["IKRollMagnitude"])) * IKRangeFactor);
            Gripper = (Int16)(ControllerBase.TwoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, values["GripperClose"], -values["GripperOpen"], 0) * ControlMultipliers.GripperRangeFactor);
            Nipper = (Int16)values["Nipper"];

            Int16[] sendValues = { Nipper, Gripper, Roll, Pitch, Yaw, Z, Y, X };
            byte[] data = new byte[sendValues.Length * sizeof(Int16)];
            Buffer.BlockCopy(sendValues, 0, data, 0, data.Length);
            Array.Reverse(data);

            if (stateToUse == ArmControlState.IKWristPOV)
            {
                _rovecomm.SendCommand(new Packet("IKWristIncrement", data, 8, (byte)DataTypes.INT16_T));
            }
            else if (stateToUse == ArmControlState.IKRoverPOV)
            {
                _rovecomm.SendCommand(new Packet("IKRoverIncrement", data, 8, (byte)DataTypes.INT16_T));
            }

            if (values["GripperSwap"] == 1)
            {
                _rovecomm.SendCommand(new Packet("GripperSwap", data, 8, (byte)DataTypes.INT16_T));
            }

            if (values["SwitchTool"] == 1 && previousTool == SelectedTool)
            {
                if (++SelectedTool > 2)
                {
                    SelectedTool = 0;
                }
                _rovecomm.SendCommand(new Packet("ToolSelection", SelectedTool));
            }
            else if (values["SwitchTool"] == 0)
            {
                previousTool = SelectedTool;
            }

            if (values["LaserToggle"] == 1)
            {
                laser = !laser;
                _rovecomm.SendCommand(new Packet("Laser", Convert.ToByte(laser)));
            }
        }
        public void SetOpPoint()
        {
            float[] opPoints = { OpX, OpY, OpZ };
            byte[] data = new byte[opPoints.Length * sizeof(float)];
            Buffer.BlockCopy(opPoints, 0, data, 0, data.Length);

            // TODO: Determine floats for this
            //_rovecomm.SendCommand(_idResolver.GetId("OpPoint"), data, true);
        }

        public void GetPosition()
        {
            byte[] data = new byte[2];
            data[0] = 0;
            data[1] = 1;
            _rovecomm.SendCommand(new Packet("ArmCommands", data, 2, (byte)DataTypes.UINT8_T));
        }
        public void SetPosition()
        {
            UInt32[] angles = { (UInt32)(AngleJ1 * 1000), (UInt32)(AngleJ2 * 1000), (UInt32)(AngleJ3 * 1000), (UInt32)(AngleJ4 * 1000), (UInt32)(AngleJ5 * 1000), (UInt32)(AngleJ6 * 1000) };
            Array.Reverse(angles);
            byte[] data = new byte[angles.Length * sizeof(UInt32)];
            Buffer.BlockCopy(angles, 0, data, 0, data.Length);
            Array.Reverse(data);
            //TODO: Determine floats for this
            _rovecomm.SendCommand(new Packet("ArmToAngle", data, 6, (byte)DataTypes.UINT32_T));

            myState = ArmControlState.GuiControl;
            guiControlInitialized = true;
        }
        public void ToggleAuto()
        {
            _rovecomm.SendCommand(new Packet("ToggleAutoPositionTelem"));
        }
        public void InitializePositions(ArmPositionsContext config)
        {
            foreach (var position in config.Positions)
                Positions.Add(new ArmPositionViewModel(position));
        }
        public void GetXYZPosition()
        {
            _rovecomm.SendCommand(new Packet("ArmGetXYZ"));
        }

        public void SetXYZPosition()
        {
            float[] coordinates = { CoordinateX, CoordinateY, CoordinateZ, Yaw, Pitch, Roll };
            byte[] data = new byte[coordinates.Length * sizeof(float)];
            Buffer.BlockCopy(coordinates, 0, data, 0, data.Length);

            // TODO: floats
            //_rovecomm.SendCommand(_idResolver.GetId("ArmAbsoluteXYZ"), data, true);

            myState = ArmControlState.GuiControl;
            guiControlInitialized = true;
        }

    }
}