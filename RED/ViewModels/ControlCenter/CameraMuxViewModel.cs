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
    public class CameraMuxViewModel : PropertyChangedBase
    {
        private ControlCenterViewModel _cc;
        private CameraMuxModel _model;

        public byte MuxIndex
        {
            get
            {
                return _model.MuxIndex;
            }
            set
            {
                _model.MuxIndex = value;
                NotifyOfPropertyChange(() => MuxIndex);
            }
        }

        public CameraMuxViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
            _model = new CameraMuxModel();
        }

        public void SetMux()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CameraMuxSet"), MuxIndex);
        }
    }
}