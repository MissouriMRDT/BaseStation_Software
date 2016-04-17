using Caliburn.Micro;
using RED.Models;
using RED.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class DropBaysViewModel : PropertyChangedBase
    {
        private ControlCenterViewModel _cc;

        public DropBaysViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
        }
    }
}