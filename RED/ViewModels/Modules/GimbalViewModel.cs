using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RED.Models.Modules;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{

    public class GimbalViewModel : PropertyChangedBase, IInputMode
    {
        private const float MaxZoomSpeed = 1000;
        private const int SpeedLimit = 1000;
        private readonly GimbalModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private GimbalStates controlState = GimbalStates.MainGimbal;

        public string Name { get; }
        public string ModeType { get; }
        
        public int PanIncrement
        {
            get
            {
                return _model.PanIncrement;
            }
            set
            {
                _model.PanIncrement = value;
                NotifyOfPropertyChange(() => PanIncrement);
            }
        }

        public int TiltIncrement
        {
            get
            {
                return _model.TiltIncrement;
            }
            set
            {
                _model.TiltIncrement = value;
                NotifyOfPropertyChange(() => TiltIncrement);
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

        public GimbalViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new GimbalModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            Name = "Main Gimbal";
            ModeType = "Gimbal";
            ControlState = System.IO.Path.GetFullPath(@"Images/NotConnected.png");
        }

        public void StartMode()
        {
            controlState = GimbalStates.MainGimbal;
            ControlState = System.IO.Path.GetFullPath(@"Images/NotConnected.png");
        }

        public void SetValues(Dictionary<string, float> values)
        {
            UpdateControlState(values);

            //increments
            // Pan, Tilt
            short[] openValsLeft = { (Int16)(values["PanLeft"] * 10), (Int16)(values["TiltLeft"] * 10) };
            short[] openValsRight = { (Int16)(values["PanRight"] * 10), (Int16)(values["TiltRight"] * 10) };

            if (controlState == GimbalStates.DriveGimbal)
            {
                _rovecomm.SendCommand(Packet.Create("LeftDriveGimbal", openValsLeft));
                _rovecomm.SendCommand(Packet.Create("RightDriveGimbal", openValsRight));
            }
            else
            {
                _rovecomm.SendCommand(Packet.Create("LeftMainGimbal", openValsLeft));
                _rovecomm.SendCommand(Packet.Create("RightMainGimbal", openValsRight));
            }
        }

        private void UpdateControlState(Dictionary<string, float> values)
        {
            if (values["MainGimbalSwitch"] == 1)
            {
                controlState = GimbalStates.MainGimbal;
                ControlState = System.IO.Path.GetFullPath(@"Images/UpArrow.png");
            }
            else if (values["DriveGimbalSwitch"] == 1)
            {
                controlState = GimbalStates.DriveGimbal;
                ControlState = System.IO.Path.GetFullPath(@"Images/DownArrow.png");
            }
        }

        public void StopMode()
        {
            short[] zero = { 0, 0 };
            _rovecomm.SendCommand(Packet.Create("LeftDriveGimbal", zero));
            _rovecomm.SendCommand(Packet.Create("RightDriveGimbal", zero));
            _rovecomm.SendCommand(Packet.Create("LeftMainGimbal", zero));
            _rovecomm.SendCommand(Packet.Create("RightMainGimbal", zero));
        }

        private enum GimbalStates
        {
            MainGimbal,
            DriveGimbal
        }
    }
}