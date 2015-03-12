using RED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels
{
    public class SettingsManagerViewModel
    {
        private SettingsManagerModel _model;
        private ControlCenterViewModel _controlCenter;

        public SettingsManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new SettingsManagerModel();
            _controlCenter = cc;
        }
    }
}
