using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using RED.ViewModels.Input;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class GimbalViewModel : PropertyChangedBase, IInputMode
    {
        private const float MaxZoomSpeed = 1000;
        private readonly GimbalModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public string Name { get; }
        public string ModeType { get; }
        
        public int SpeedLimit
        {
            get
            {
                return _model.SpeedLimit;
            }
            set
            {
                _model.SpeedLimit = value;
                NotifyOfPropertyChange(() => SpeedLimit);
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

        public GimbalViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new GimbalModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            Name = "Main Gimbal";
            ModeType = "Gimbal";
            
            SpeedLimit = 1000;
        }

        public void StartMode()
        { }

        public void SetValues(Dictionary<string, float> values)
        {
            short pan, tilt, mast, zoom, roll;

            switch (ControllerBase.JoystickDirection(values["Pan"], values["Tilt"]))
            {
                case ControllerBase.JoystickDirections.Right:
                case ControllerBase.JoystickDirections.Left:
                    pan = (short)(values["Pan"] * SpeedLimit);
                    tilt = 0;
                    break;
                case ControllerBase.JoystickDirections.Up:
                case ControllerBase.JoystickDirections.Down:
                    tilt = (short)(values["Tilt"] * SpeedLimit);
                    pan = 0;
                    break;

                default:
                    tilt = 0;
                    pan = 0;
                    break;
            }

            zoom = (Int16)(values["Zoom"] * MaxZoomSpeed);
            roll = (Int16)(values["Roll"] * RollIncrement);
            mast = (Int16)(ControllerBase.TwoButtonToggleDirection(values["GimbalMastTiltDirection"] != 0, (values["GimbalMastTiltMagnitude"])) * SpeedLimit);

            short[] openVals = { pan, tilt, roll, mast, zoom };
            byte[] data = new byte[openVals.Length * sizeof(Int16)];
            Buffer.BlockCopy(openVals, 0, data, 0, data.Length);
            _rovecomm.SendCommand(_idResolver.GetId("GimbalOpenValues"), data);
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalOpenValues"), new byte[]{ 0, 0, 0, 0, 0 }, true);
        }
    }
}