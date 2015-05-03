using Caliburn.Micro;
using RED.Addons;
using RED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GPSViewModel : PropertyChangedBase
    {
        GPSModel _model;
        ControlCenterViewModel _cc;

        public GPSViewModel(ControlCenterViewModel cc)
        {
            _model = new GPSModel();
            _cc = cc;
        }
    }
}