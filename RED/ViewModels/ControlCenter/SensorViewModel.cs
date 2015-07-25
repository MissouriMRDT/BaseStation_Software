using Caliburn.Micro;
using RED.Models;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class SensorViewModel : PropertyChangedBase, ISubscribe
    {
        private SensorModel _model;
        private ControlCenterViewModel _cc;

        public SensorViewModel(ControlCenterViewModel cc)
        {
            _model = new SensorModel();
            _cc = cc;
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {

        }
    }
}