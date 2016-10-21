using Caliburn.Micro;
using RED.Models;
using RED.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Modules
{
    public class DropBaysViewModel : PropertyChangedBase
    {
        private ControlCenterViewModel _cc;

        public DropBaysViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
        }

        public void OpenBay(byte index)
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("DropBayOpen"), index);
        }
    }
}