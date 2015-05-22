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
    public class ScienceViewModel : PropertyChangedBase
    {
        private ScienceModel _model;
        private ControlCenterViewModel _cc;

        public ScienceViewModel(ControlCenterViewModel cc)
        {
            _model = new ScienceModel();
            _cc = cc;
        }
    }
}