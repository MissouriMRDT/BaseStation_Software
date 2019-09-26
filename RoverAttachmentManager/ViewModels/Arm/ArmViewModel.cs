using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.ArmModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RoverAttachmentManager.ViewModels.Arm
{
    /// <summary>
    /// The main class for handling all arm-based logic, and corresponds to the Arm dropbox in the gui 
    /// 
    /// Inhereting from PropertyBasedChange (caliburn micro for interacting with the view), IInputMode, and 
    /// ISubscribe, this class essentially has three responsibilities.
    /// First, it contains all the logic that tells the arm view what to fill itself with. Most of the fields below are for this,
    /// such as AngleJ1 and so on. These are bound together with certain elements in the view, and changing them here
    /// changes them in the view.
    /// 
    /// Second, as an IInputMode, the class is responsible for handling what happens when the user starts up arm control. 
    /// An instance of this class is called when that happens, and methods such as StartMode, SetValues, and such are for 
    /// this purpose. More information can be found in IInputmode.cs over in Interfaces folder.
    /// 
    /// Finally, as an ISubscribe, this class is responsible for receiving messages from the router whenever RED 
    /// gets telemetry messages from Rover. When that happens rovecomm will call this class and ask it to see
    /// if any of the messages (rovecomm dataid's) belong to it.
    /// 
    /// This class is designed to be constructed in the main view model class (usually ControlCenterViewModel).
    /// </summary>
    public class ArmViewModel : PropertyChangedBase, IInputMode, IRovecommReceiver
    {
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
        private readonly ArmModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private readonly IConfigurationManager _configManager;
        private readonly Dictionary<int, string> _armFaultIds;

        //flag that gets set when the arm detects and error and forces a change state, we want the user to have to wait a second 
        //before commands can be sent again so they can register the fact taht said error occurred
        private bool freezeArm = false; 

        public string Name { get; }
        public string ModeType { get; }

        //the arm should be coded to call its watchdog and reset itself frequently when we're not sending commands to it.
        //It should be coded to hold off on this reset when it's in closed loop mode; when in gui control mode, it's in
        //an odd state where RED is in closed loop mode, but until it sends a command manually to the arm the arm will 
        //remain in open loop mode and thus keep resetting itself, making sending it commands harrowing. So when in gui ctl, 
        //RED will keep sending empty commands to the arm to keep it alive until the user sends it a gui command and 
        //puts it into closed loop mode as well
        private bool guiControlInitialized;

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
        public int BaseRangeFactor
        {
            get
            {
                return _model.BaseRangeFactor;
            }
            set
            {
                _model.BaseRangeFactor = value;
                NotifyOfPropertyChange(() => BaseRangeFactor);
            }
        }
        public int ElbowRangeFactor
        {
            get
            {
                return _model.ElbowRangeFactor;
            }
            set
            {
                _model.ElbowRangeFactor = value;
                NotifyOfPropertyChange(() => ElbowRangeFactor);
            }
        }
        public int WristRangeFactor
        {
            get
            {
                return _model.WristRangeFactor;
            }
            set
            {
                _model.WristRangeFactor = value;
                NotifyOfPropertyChange(() => WristRangeFactor);
            }
        }
        public int GripperRangeFactor
        {
            get
            {
                return _model.GripperRangeFactor;
            }
            set
            {
                _model.GripperRangeFactor = value;
                NotifyOfPropertyChange(() => GripperRangeFactor);
            }
        }
        public int Gripper2RangeFactor
        {
            get
            {
                return _model.Gripper2RangeFactor;
            }
            set
            {
                _model.Gripper2RangeFactor = value;
                NotifyOfPropertyChange(() => Gripper2RangeFactor);
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


        byte previousTool;
        bool laser = false;



        public ArmViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, IConfigurationManager configs)
        {
            _model = new ArmModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            _configManager = configs;
            Name = "Arm";
            ModeType = "Arm";
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

        public void ReceivedRovecommMessageCallback(ref Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ArmAngles":
                    //Array.Reverse(packet.Data);
                    AngleJ1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 0)) / 1000.0);
                    AngleJ2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 4)) / 1000.0);
                    AngleJ3 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 8)) / 1000.0);
                    AngleJ4 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 12)) / 1000.0);
                    AngleJ5 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 16)) / 1000.0);
                    AngleJ6 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 20)) / 1000.0);
                    break;
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

        public void StartMode()
        {
            myState = ArmControlState.OpenLoop;
            ControlState = "Open loop";
        }

        private void SetOpenLoopValues(Dictionary<string, float> values)
        {
            Int16 ArmWristBend = 0;
            Int16 ArmWristTwist = 0;
            Int16 ArmElbowTwist = 0;
            Int16 ArmElbowBend = 0;
            Int16 ArmBaseTwist = 0;
            Int16 ArmBaseBend = 0;
            Int16 Gripper = 0;
            Int16 Gripper2 = 0;
            Int16 Nipper = 0;

            switch (ControllerBase.JoystickDirection(values["WristBend"], values["WristTwist"]))
            {
                case ControllerBase.JoystickDirections.Right:
                case ControllerBase.JoystickDirections.Left:
                case ControllerBase.JoystickDirections.Up:
                case ControllerBase.JoystickDirections.Down:
                    ArmWristBend = (Int16)(values["WristBend"] * WristRangeFactor);
                    ArmWristTwist = (Int16)(values["WristTwist"] * WristRangeFactor);
                    break;
                case ControllerBase.JoystickDirections.None:
                    ArmWristTwist = 0;
                    ArmWristBend = 0;
                    break;
            }

            switch (ControllerBase.JoystickDirection(values["ElbowBend"], values["ElbowTwist"]))
            {
                case ControllerBase.JoystickDirections.Up:
                case ControllerBase.JoystickDirections.Down:
                    ArmElbowTwist = 0;
                    ArmElbowBend = (Int16)(-values["ElbowBend"] * ElbowRangeFactor);
                    break;
                case ControllerBase.JoystickDirections.Right:
                case ControllerBase.JoystickDirections.Left:
                    ArmElbowTwist = (Int16)(-values["ElbowTwist"] * ElbowRangeFactor);
                    ArmElbowBend = 0;
                    break;
                case ControllerBase.JoystickDirections.None:
                    ArmElbowTwist = 0;
                    ArmElbowBend = 0;
                    break;
            }


            ArmBaseTwist = (Int16)(-ControllerBase.TwoButtonToggleDirection(values["BaseTwistDirection"] != 0, (values["BaseTwistMagnitude"])) * BaseRangeFactor);
            ArmBaseBend = (Int16)(-ControllerBase.TwoButtonToggleDirection(values["BaseBendDirection"] != 0, (values["BaseBendMagnitude"])) * BaseRangeFactor);

            float gripperAmmount = ControllerBase.TwoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, values["GripperClose"], -values["GripperOpen"], 0);
            if (SelectedTool == 0)
            {
                Gripper = (Int16)(gripperAmmount * GripperRangeFactor);
            }else if (SelectedTool == 1)
            {
                Gripper2 = (Int16)(gripperAmmount * Gripper2RangeFactor);
            }

            Nipper = (Int16)values["Nipper"];

            Int16[] sendValues = { Gripper2, Nipper, Gripper, ArmWristTwist, ArmWristBend, ArmElbowTwist, ArmElbowBend, ArmBaseBend, ArmBaseTwist }; //order before we reverse
            byte[] data = new byte[sendValues.Length * sizeof(Int16)];
            Buffer.BlockCopy(sendValues, 0, data, 0, data.Length);
            Array.Reverse(data);
            _rovecomm.SendCommand(new Packet("ArmValues", data, 9, (byte)DataTypes.INT16_T));

            if (values["GripperSwap"] == 1)
            {
                _rovecomm.SendCommand(new Packet("GripperSwap", data, 8, (byte)DataTypes.INT16_T));
            }

            if (values["SwitchTool"] == 1 && previousTool == SelectedTool)
            {
                if(++SelectedTool > 1)
                {
                    SelectedTool = 0;
                }
                //_rovecomm.SendCommand(new Packet("ToolSelection", SelectedTool));
            }
            else if (values["SwitchTool"] == 0){
                previousTool = SelectedTool;
            }
            if (values["LaserToggle"] == 1)
            {
                laser = !laser;
                _rovecomm.SendCommand(new Packet("Laser", Convert.ToByte(laser)));
            }
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
            Gripper = (Int16)(ControllerBase.TwoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, values["GripperClose"], -values["GripperOpen"], 0) * GripperRangeFactor);
            Nipper = (Int16)values["Nipper"];

            Int16[] sendValues = { Nipper, Gripper, Roll, Pitch, Yaw, Z, Y, X};
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
            
            if(values["GripperSwap"] == 1)
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

        private void UpdateControlState(Dictionary<string, float> values)
        {
            ArmControlState oldState = myState;
            string state = "";

            if (values["UseAngular"] != 0)
            {
                myState = ArmControlState.GuiControl;
                state = "GUI control";
                guiControlInitialized = false;
            }
            else if (values["UseOpenLoop"] != 0)
            {
                myState = ArmControlState.OpenLoop;
                state = "Open loop";
            }
            else if (values["UseRoverPerspectiveIK"] != 0)
            {
                myState = ArmControlState.IKRoverPOV;
                state = "Rover perspective IK";
            }
            else if (values["UseWristPerspectiveIK"] != 0)
            {
                myState = ArmControlState.IKWristPOV;
                state = "Wrist perspective IK";
            }

            if (oldState != myState)
            {
                _rovecomm.SendCommand(new Packet("ArmStop"));
                ControlState = state;
            }
        }

        public void SetValues(Dictionary<string, float> values)
        {
            UpdateControlState(values);

            if(freezeArm)
            {
                //let user realzie an error has occurred before allowing commands to be sent again.
                Task.Delay(500);
                freezeArm = false;
            }

            if (myState == ArmControlState.OpenLoop)
            {
                SetOpenLoopValues(values);
            }
            else if (myState == ArmControlState.IKRoverPOV || myState == ArmControlState.IKWristPOV)
            {
                SetIKValues(values, myState);
            }
            else if(myState == ArmControlState.GuiControl)
            {
                if(guiControlInitialized == false)
                {
                    _rovecomm.SendCommand(new Packet("ArmStop"));
                }
            }
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(new Packet("ArmStop"));

            myState = ArmControlState.GuiControl;
            ControlState = "GUI control";
        }

        public void EnableCommand(string bus, bool enableState)
        {
            string name;
            switch (bus)
            {
                case "All": name = "ArmEnableAll"; break;
                case "Main": name = "ArmEnableMain"; break;
                case "J1": name = "ArmEnableJ1"; break;
                case "J2": name = "ArmEnableJ2"; break;
                case "J3": name = "ArmEnableJ3"; break;
                case "J4": name = "ArmEnableJ4"; break;
                case "J5": name = "ArmEnableJ5"; break;
                case "J6": name = "ArmEnableJ6"; break;
                case "Endeff1": name = "ArmEnableEndeff1"; break;
                case "Endeff2": name = "ArmEnableEndeff2"; break;
                default: return;
            }

            _rovecomm.SendCommand(new Packet(name, (enableState) ? ArmEnableCommand : ArmDisableCommand), true);
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
            UInt32[] angles = { (UInt32)(AngleJ1*1000), (UInt32)(AngleJ2*1000), (UInt32)(AngleJ3*1000), (UInt32)(AngleJ4*1000), (UInt32)(AngleJ5*1000), (UInt32)(AngleJ6*1000) };
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

        public void LimitSwitchOverride()
        {
            _rovecomm.SendCommand(new Packet("LimitSwitchOverride", (byte)1), true);
        }
        public void LimitSwitchUnOverride()
        {
            _rovecomm.SendCommand(new Packet("LimitSwitchOverride", (byte)0), true);
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