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

        public int RollIncrement
        {
            get
            {
                return _model.RollIncrement;
            }
            set
            {
                _model.RollIncrement = value;
                NotifyOfPropertyChange(() => RollIncrement);
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
            short[] openVals = { (Int16)(values["Tilt"] * TiltIncrement), (Int16)(values["Pan"] * PanIncrement)};
            byte[] data = new byte[4];
            Buffer.BlockCopy(openVals, 0, data, 0, data.Length);
            Array.Reverse(data);

            if (controlState == GimbalStates.DriveGimbal)
            {
                _rovecomm.SendCommand(new Packet("DriveGimbalIncrement", data, 2, (byte)DataTypes.INT16_T));
            }
            else
            {
                _rovecomm.SendCommand(new Packet("MainGimbalIncrement", data, 2,(byte)DataTypes.INT16_T));
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
            //_rovecomm.SendCommand(new Packet("GimbalOpenValues", new byte[]{ 0, 0, 0, 0, 0 }, 5, (byte)DataTypes.UINT8_T), true);
            _rovecomm.SendCommand(new Packet("MainGimbalIncrement", new byte[] { 0, 0, 0, 0 }, 2, (byte)DataTypes.INT16_T));
        }

        public void Snapshot()
        {
           // _rovecomm.SendCommand(new Packet("GimbalRecord", (byte)GimbalRecordValues.Snapshot), true);
        }

        public void RecordStart()
        {
           // _rovecomm.SendCommand(new Packet("GimbalRecord", (byte)GimbalRecordValues.Start), true);
        }

        public void RecordStop()
        {
           // _rovecomm.SendCommand(new Packet("GimbalRecord", (byte)GimbalRecordValues.Stop), true);
        }

        private enum GimbalRecordValues
        {
            Stop = 0,
            Start = 1,
            Snapshot = 2
        }

        private enum GimbalStates
        {
            MainGimbal,
            DriveGimbal
        }
    }
}