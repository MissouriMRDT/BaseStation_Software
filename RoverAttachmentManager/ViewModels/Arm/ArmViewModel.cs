using Caliburn.Micro;
using Core.Configurations;
using Core.Interfaces;
using Core.Interfaces.Input;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.Arm;
using RoverAttachmentManager.Models.ArmModels;
using System;
using System.Collections.Generic;

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
    public class ArmViewModel : PropertyChangedBase, IInputMode
    {
        public enum ArmControlState
        {
            OpenLoop,
            IKRoverPOV, //point of view
            IKWristPOV,
            GuiControl
        };

        public ArmControlState myState;
        
        private readonly ArmModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private const string PositionsConfigName = "ArmPositions";

        public string Name { get; }
        public string ModeType { get; }

        //the arm should be coded to call its watchdog and reset itself frequently when we're not sending commands to it.
        //It should be coded to hold off on this reset when it's in closed loop mode; when in gui control mode, it's in
        //an odd state where RED is in closed loop mode, but until it sends a command manually to the arm the arm will 
        //remain in open loop mode and thus keep resetting itself, making sending it commands harrowing. So when in gui ctl, 
        //RED will keep sending empty commands to the arm to keep it alive until the user sends it a gui command and 
        //puts it into closed loop mode as well
        public bool guiControlInitialized;

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
        public ArmPowerViewModel ArmPower
        {
            get
            {
                return _model.ArmPower;
            }
            set
            {
                _model.ArmPower = value;
                NotifyOfPropertyChange(() => ArmPower);
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
        public ControlFeaturesViewModel ControlFeatures
        {
            get
            {
                return _model._controlFeatures;
            }
            set
            {
                _model._controlFeatures = value;
                NotifyOfPropertyChange(() => ControlFeatures);
            }
        }
        public AngularControlViewModel AngularControl
        {
            get
            {
                return _model._angularControl;
            }
            set
            {
                _model._angularControl = value;
                NotifyOfPropertyChange(() => AngularControl);
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
        public InputManagerViewModel InputManager
        {
            get
            {
                return _model.InputManager;
            }
            set
            {
                _model.InputManager = value;
                NotifyOfPropertyChange(() => InputManager);
            }
        }

        public XMLConfigManager ConfigManager
        {
            get
            {
                return _model._configManager;
            }
            set
            {
                _model._configManager = value;
                NotifyOfPropertyChange();
            }
        }
        public XboxControllerInputViewModel XboxController1
        {
            get
            {
                return _model._xboxController1;
            }
            set
            {
                _model._xboxController1 = value;
                NotifyOfPropertyChange(() => XboxController1);
            }
        }
        public XboxControllerInputViewModel XboxController2
        {
            get
            {
                return _model._xboxController2;
            }
            set
            {
                _model._xboxController2 = value;
                NotifyOfPropertyChange(() => XboxController2);
            }
        }
        public XboxControllerInputViewModel XboxController3
        {
            get
            {
                return _model._xboxController3;
            }
            set
            {
                _model._xboxController3 = value;
                NotifyOfPropertyChange(() => XboxController3);
            }
        }

        public byte previousTool;
        public bool laser = false;



        public ArmViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, IConfigurationManager configs)
        {
            _model = new ArmModel();
            ControlMultipliers = new ControlMultipliersViewModel();
            ControlFeatures = new ControlFeaturesViewModel(networkMessenger, idResolver, log);
            AngularControl = new AngularControlViewModel(networkMessenger, idResolver, log, configs, this);
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            ConfigManager = new XMLConfigManager(log);

            XboxController1 = new XboxControllerInputViewModel(1);
            XboxController2 = new XboxControllerInputViewModel(2);
            XboxController3 = new XboxControllerInputViewModel(3);

            // Programatic instanciation of InputManager view, vs static like everything else in a xaml 
            InputManager = new InputManagerViewModel(log, ConfigManager,
                new IInputDevice[] { XboxController1, XboxController2, XboxController3 },
                new MappingViewModel[0],
                new IInputMode[] { this });

            ArmPower = new ArmPowerViewModel(_rovecomm, _idResolver, _log);

            Name = "Arm";
            ModeType = "Arm";
            myState = ArmControlState.GuiControl;
            ControlState = "GUI control";
            previousTool = 0;

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
                    ArmWristBend = (Int16)(values["WristBend"] * ControlMultipliers.WristRangeFactor);
                    ArmWristTwist = (Int16)(values["WristTwist"] * ControlMultipliers.WristRangeFactor);
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
                    ArmElbowBend = (Int16)(-values["ElbowBend"] * ControlMultipliers.ElbowRangeFactor);
                    break;
                case ControllerBase.JoystickDirections.Right:
                case ControllerBase.JoystickDirections.Left:
                    ArmElbowTwist = (Int16)(-values["ElbowTwist"] * ControlMultipliers.ElbowRangeFactor);
                    ArmElbowBend = 0;
                    break;
                case ControllerBase.JoystickDirections.None:
                    ArmElbowTwist = 0;
                    ArmElbowBend = 0;
                    break;
            }


            ArmBaseTwist = (Int16)(-ControllerBase.TwoButtonToggleDirection(values["BaseTwistDirection"] != 0, (values["BaseTwistMagnitude"])) * ControlMultipliers.BaseRangeFactor);
            ArmBaseBend = (Int16)(-ControllerBase.TwoButtonToggleDirection(values["BaseBendDirection"] != 0, (values["BaseBendMagnitude"])) * ControlMultipliers.BaseRangeFactor);

            float gripperAmmount = ControllerBase.TwoButtonTransform(values["GripperClose"] > 0, values["GripperOpen"] > 0, values["GripperClose"], -values["GripperOpen"], 0);
            if (ControlFeatures.SelectedTool == 0)
            {
                Gripper = (Int16)(gripperAmmount * ControlMultipliers.GripperRangeFactor);
            }
            else if (ControlFeatures.SelectedTool == 1)
            {
                Gripper2 = (Int16)(gripperAmmount * ControlMultipliers.Gripper2RangeFactor);
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

            if (values["SwitchTool"] == 1 && previousTool == ControlFeatures.SelectedTool)
            {
                if (++ControlFeatures.SelectedTool > 1)
                {
                    ControlFeatures.SelectedTool = 0;
                }
                //_rovecomm.SendCommand(new Packet("ToolSelection", SelectedTool));
            }
            else if (values["SwitchTool"] == 0)
            {
                previousTool = ControlFeatures.SelectedTool;
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

            if (values["SwitchTool"] == 1 && previousTool == ControlFeatures.SelectedTool)
            {
                if (++ControlFeatures.SelectedTool > 2)
                {
                    ControlFeatures.SelectedTool = 0;
                }
                _rovecomm.SendCommand(new Packet("ToolSelection", ControlFeatures.SelectedTool));
            }
            else if (values["SwitchTool"] == 0)
            {
                previousTool = ControlFeatures.SelectedTool;
            }

            if (values["LaserToggle"] == 1)
            {
                laser = !laser;
                _rovecomm.SendCommand(new Packet("Laser", Convert.ToByte(laser)));
            }
        }
        public void StopMode()
        {
            _rovecomm.SendCommand(new Packet("ArmStop"));

            myState = ArmControlState.GuiControl;
            ControlState = "GUI control";
        }
    }
}