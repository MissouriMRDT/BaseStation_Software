using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class ExternalControlsViewModel
    {
        private ControlCenterViewModel _cc;

        private const byte GimbalDisableCommand = 0x00;
        private const byte GimbalEnableCommand = 0x01;

        public ExternalControlsViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
        }

        public void EnableAll()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("GimbalEnableAll"), GimbalEnableCommand);
        }
        public void DisableAll()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("GimbalEnableAll"), GimbalDisableCommand);
        }
    }
}
