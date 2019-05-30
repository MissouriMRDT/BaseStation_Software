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
            Name = "Gimbal";
            ModeType = "Gimbal";
        }

        public void StartMode()
        {
        }

        public void SetValues(Dictionary<string, float> values)
        {
            
            // Pan, Tilt
            short[] openVals = { (Int16)(values["DriveTilt"] * 50), (Int16)(values["DrivePan"] * 50)};
            byte[] data = new byte[4];
            Buffer.BlockCopy(openVals, 0, data, 0, data.Length);
            Array.Reverse(data);
            
            _rovecomm.SendCommand(new Packet("DriveGimbalIncrement", data, 2, (byte)DataTypes.INT16_T));

            short[] openVals2 = { (Int16)(values["MainTilt"] * 50), (Int16)(values["MainPan"] * 50) };
            data = new byte[4];
            Buffer.BlockCopy(openVals2, 0, data, 0, data.Length);
            Array.Reverse(data);

            _rovecomm.SendCommand(new Packet("MainGimbalIncrement", data, 2,(byte)DataTypes.INT16_T));
            
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(new Packet("MainGimbalIncrement", new byte[] { 0, 0, 0, 0 }, 2, (byte)DataTypes.INT16_T));
            _rovecomm.SendCommand(new Packet("DriveGimbalIncrement", new byte[] { 0, 0, 0, 0 }, 2, (byte)DataTypes.INT16_T));
        }
    }
}