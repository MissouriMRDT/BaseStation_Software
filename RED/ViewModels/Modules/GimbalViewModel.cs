using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class GimbalViewModel : PropertyChangedBase, IInputMode
    {
        private readonly GimbalModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private readonly string[] CommandDataId = { "Camera1Command", "Camera2Command" };
        private readonly string[] PTZDataId = { "PTZ1Speed", "PTZ2Speed" };
        private readonly string[] MenuDataId = { "Camera1Menu", "Camera2Menu" };

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
            short pan, tilt;

            pan = (short)(values["Pan"] * SpeedLimit);
            tilt = (short)(values["Tilt"] * SpeedLimit);
            _rovecomm.SendCommand(_idResolver.GetId(PTZDataId[0]), ((int)tilt << 16) | (((int)pan) & 0xFFFF));

            if (values["ZoomIn"] != 0)
                _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.ZoomIn);
            else if (values["ZoomOut"] != 0)
                _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.ZoomOut);
            else
                _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.Stop);
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("PTZ1Speed"), (int)0, true);
        }

        public void ZoomFocusStop()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.Stop, true);
        }
        public void ZoomIn()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.ZoomIn, true);
        }
        public void ZoomOut()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.ZoomOut, true);
        }
        public void FocusNear()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.FocusNear, true);
        }
        public void FocusFar()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[0]), (byte)GimbalZoomCommands.FocusFar, true);
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