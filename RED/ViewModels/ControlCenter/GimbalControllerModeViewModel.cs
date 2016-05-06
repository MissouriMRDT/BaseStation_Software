using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GimbalControllerModeViewModel : PropertyChangedBase, IControllerMode
    {
        private ControlCenterViewModel _cc;
        private GimbalModel _model;

        public string Name { get; set; }
        public IInputDevice InputVM { get; set; }

        public int SpeedLimit
        {
            get
            {
                return _model.speedLimit;
            }
            set
            {
                _model.speedLimit = value;
                NotifyOfPropertyChange(() => SpeedLimit);
            }
        }

        public GimbalControllerModeViewModel(IInputDevice inputVM, ControlCenterViewModel cc)
        {
            _cc = cc;
            _model = new GimbalModel();
            InputVM = inputVM;
            Name = "Gimbal";
            SpeedLimit = 1000;
        }

        public void EnterMode()
        {

        }

        public void EvaluateMode()
        {
            short pan, tilt;

            pan = (short)(InputVM.GimbalPan * SpeedLimit);
            tilt = (short)(InputVM.GimbalTilt * SpeedLimit);
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PTZ1Speed"), ((int)tilt << 16) | (((int)pan) & 0xFFFF));

            if (InputVM.GimbalZoomIn)
                _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.ZoomIn);
            else if (InputVM.GimbalZoomOut)
                _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.ZoomOut);
            else
                _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.Stop);

        }

        public void ExitMode()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PTZ1Speed"), (int)0);
        }

        public void ZoomFocusStop()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.Stop);
        }
        public void ZoomIn()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.ZoomIn);
        }
        public void ZoomOut()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.ZoomOut);
        }
        public void FocusNear()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.FocusNear);
        }
        public void FocusFar()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Command"), (byte)GimbalZoomCommands.FocusFar);
        }

        public void MenuCenter()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Menu"), (byte)GimbalMenuCommands.Menu);
        }
        public void MenuUp()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Menu"), (byte)GimbalMenuCommands.MenuUp);
        }
        public void MenuDown()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Menu"), (byte)GimbalMenuCommands.MenuDown);
        }
        public void MenuLeft()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Menu"), (byte)GimbalMenuCommands.MenuLeft);
        }
        public void MenuRight()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("Camera1Menu"), (byte)GimbalMenuCommands.MenuRight);
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