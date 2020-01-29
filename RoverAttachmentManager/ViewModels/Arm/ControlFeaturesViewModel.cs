using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using RoverAttachmentManager.Models.Arm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.ViewModels.Arm
{
    public class ControlFeaturesViewModel : PropertyChangedBase
    {
        private readonly ControlFeaturesModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public byte SelectedTool
        {
            get
            {
                return _model.SelectedTool;
            }
            set
            {
                _model.SelectedTool = value;
                NotifyOfPropertyChange(() => SelectedTool);
            }
        }

        public ControlFeaturesViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ControlFeaturesModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

        }

        public void LimitSwitchOverride()
        {
            _rovecomm.SendCommand(new Packet("LimitSwitchOverride", (byte)1), true);
        }
        public void LimitSwitchUnOverride()
        {
            _rovecomm.SendCommand(new Packet("LimitSwitchOverride", (byte)0), true);
        }

    }
}
