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

        public int GimbalIndex
        {
            get
            {
                return _model.GimbalIndex;
            }
            set
            {
                _model.GimbalIndex = value;
                NotifyOfPropertyChange(() => GimbalIndex);
            }
        }
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

        public GimbalViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, int gimbalIndex)
        {
            _model = new GimbalModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            Name = "Gimbal " + (gimbalIndex + 1).ToString();
            ModeType = "Gimbal";

            if (gimbalIndex == 0)
                SpeedLimit = 1000;
            else if (gimbalIndex == 1)
                SpeedLimit = 300;

            GimbalIndex = gimbalIndex;
        }

        public void StartMode()
        { }

        public void SetValues(Dictionary<string, float> values)
        {
            short pan, tilt;

            pan = (short)(values["Pan"] * SpeedLimit);
            tilt = (short)(values["Tilt"] * SpeedLimit);
            _rovecomm.SendCommand(_idResolver.GetId(PTZDataId[GimbalIndex]), ((int)tilt << 16) | (((int)pan) & 0xFFFF));

            if (values["ZoomIn"] != 0)
                _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.ZoomIn);
            else if (values["ZoomOut"] != 0)
                _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.ZoomOut);
            else
                _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.Stop);
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("PTZ1Speed"), (int)0, true);
        }

        public void ZoomFocusStop()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.Stop, true);
        }
        public void ZoomIn()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.ZoomIn, true);
        }
        public void ZoomOut()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.ZoomOut, true);
        }
        public void FocusNear()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.FocusNear, true);
        }
        public void FocusFar()
        {
            _rovecomm.SendCommand(_idResolver.GetId(CommandDataId[GimbalIndex]), (byte)GimbalZoomCommands.FocusFar, true);
        }

        public void MenuCenter()
        {
            _rovecomm.SendCommand(_idResolver.GetId(MenuDataId[GimbalIndex]), (byte)GimbalMenuCommands.Menu, true);
        }
        public void MenuUp()
        {
            _rovecomm.SendCommand(_idResolver.GetId(MenuDataId[GimbalIndex]), (byte)GimbalMenuCommands.MenuUp, true);
        }
        public void MenuDown()
        {
            _rovecomm.SendCommand(_idResolver.GetId(MenuDataId[GimbalIndex]), (byte)GimbalMenuCommands.MenuDown, true);
        }
        public void MenuLeft()
        {
            _rovecomm.SendCommand(_idResolver.GetId(MenuDataId[GimbalIndex]), (byte)GimbalMenuCommands.MenuLeft, true);
        }
        public void MenuRight()
        {
            _rovecomm.SendCommand(_idResolver.GetId(MenuDataId[GimbalIndex]), (byte)GimbalMenuCommands.MenuRight, true);
        }

        private enum GimbalZoomCommands : byte
        {
            Stop = 0,
            ZoomIn = 1,
            ZoomOut = 2,
            FocusNear = 3,
            FocusFar = 4
        }

        private enum GimbalMenuCommands : byte
        {
            Menu = 0,
            MenuLeft = 1,
            MenuRight = 2,
            MenuUp = 3,
            MenuDown = 4
        }
    }
}