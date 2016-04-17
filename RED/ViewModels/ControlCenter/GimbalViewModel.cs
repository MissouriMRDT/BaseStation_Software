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
        private ControlCenterViewModel _cc;

        public GimbalViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
        }
    }
}