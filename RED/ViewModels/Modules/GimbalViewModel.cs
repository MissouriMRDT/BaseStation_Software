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
        private readonly GimbalModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private bool _inClosedLoop = false;

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

            if(values["MastPositionUp"] == 1)
            {
                _inClosedLoop = true;
                _rovecomm.SendCommand(_idResolver.GetId("MastPosition"), 1);
            }
            else if(values["MastPositionDown"] == 1)
            {
                _inClosedLoop = true;
                _rovecomm.SendCommand(_idResolver.GetId("MastPosition"), 0);
            }
            else if(values["CancelClosedLoop"] == 1)
            {
                _inClosedLoop = false;
            }

            pan = (short)(values["Pan"] * SpeedLimit);
            tilt = (short)(values["Tilt"] * SpeedLimit);
            if (values["ZoomIn"] != 0)
                zoom = (byte)GimbalZoomCommands.ZoomIn;
            else if (values["ZoomOut"] != 0)
                zoom = (byte)GimbalZoomCommands.ZoomOut;
            else if (values["FocusIn"] != 0)
                zoom = (byte)GimbalZoomCommands.FocusNear;
            else if (values["FocusOut"] != 0)
                zoom = (byte)GimbalZoomCommands.FocusFar;
            else
                zoom = (byte)GimbalZoomCommands.Stop;

            roll = (Int16)(values["Roll"] * RollIncrement);
            short[] ptzrValues = { pan, tilt, zoom, roll };
            byte[] data = new byte[ptzrValues.Length * sizeof(Int16)];
            Buffer.BlockCopy(ptzrValues, 0, data, 0, data.Length);
            _rovecomm.SendCommand(_idResolver.GetId("GimbalPTZR"), data);

            if (_inClosedLoop == false)
            {
                mast = (Int16)(ControllerBase.TwoButtonToggleDirection(values["GimbalMastTiltDirection"] != 0, (values["GimbalMastTiltMagnitude"])) * SpeedLimit);
                _rovecomm.SendCommand(_idResolver.GetId("Mast"), mast);
            }
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalPTZR"), (Int32)0, true);
        }

        public void ZoomFocusStop()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ZoomFocus"), (byte)GimbalZoomCommands.Stop, true);
        }
        public void ZoomIn()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ZoomFocus"), (byte)GimbalZoomCommands.ZoomIn, true);
        }
        public void ZoomOut()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ZoomFocus"), (byte)GimbalZoomCommands.ZoomOut, true);
        }
        public void FocusNear()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ZoomFocus"), (byte)GimbalZoomCommands.FocusNear, true);
        }
        public void FocusFar()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ZoomFocus"), (byte)GimbalZoomCommands.FocusFar, true);
        }

        private enum GimbalZoomCommands : byte
        {
            Stop = 0,
            ZoomIn = 1,
            ZoomOut = 2,
            FocusNear = 3,
            FocusFar = 4
        }
    }
}