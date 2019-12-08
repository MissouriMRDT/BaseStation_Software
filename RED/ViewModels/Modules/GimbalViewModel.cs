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
        }

        public void StartMode()
        {
            controlState = GimbalStates.MainGimbal;
            ControlState = "Main Gimbal";
        }

        public void SetValues(Dictionary<string, float> values)
        {
            UpdateControlState(values);
            
            // Pan, Tilt
            short[] openVals = { (Int16)(values["Pan"] * 50), (Int16)(values["Tilt"] * 50)};

            if (controlState == GimbalStates.DriveGimbal)
            {
                _rovecomm.SendCommand(Packet.Create("DriveGimbalIncrement", openVals));
            }
            else
            {
                _rovecomm.SendCommand(Packet.Create("MainGimbalIncrement", openVals));
            }
        }

        private void UpdateControlState(Dictionary<string, float> values)
        {
            if(values["MainGimbalSwitch"] == 1)
            {
                controlState = GimbalStates.MainGimbal;
                ControlState = "Main Gimbal";
            }
            else if(values["DriveGimbalSwitch"] == 1)
            {
                controlState = GimbalStates.DriveGimbal;
                ControlState = "Drive Gimbal";
            }
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(Packet.Create("MainGimbalIncrement", new Int16[] { 0, 0 }));
        }

        private enum GimbalStates
        {
            MainGimbal,
            DriveGimbal
        }
    }
}