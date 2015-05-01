using RED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GPSViewModel
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