using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GimbalViewModel : PropertyChangedBase
    {
        private const short Speed = 1000;

        private ControlCenterViewModel _cc;

        public GimbalViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
        }

        public void MoveUp()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PTZ1Speed"), (int)Speed);
        }
        public void MoveDown()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PTZ1Speed"), (int)(-Speed & 0xFFFF));
        }
        public void MoveLeft()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PTZ1Speed"), (int)(-Speed) << 16);
        }
        public void MoveRight()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PTZ1Speed"), (int)Speed << 16);
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