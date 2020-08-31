using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.Arm;
using RoverAttachmentManager.Models.ArmModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace RoverAttachmentManager.ViewModels.Arm
{
    public class ControlMultipliersViewModel : PropertyChangedBase
    {
        private readonly ControlMultipliersModel _model;

        public int BaseRangeFactor
        {
            get
            {
                return _model.BaseRangeFactor;
            }
            set
            {
                _model.BaseRangeFactor = value;
                NotifyOfPropertyChange(() => BaseRangeFactor);
            }
        }
        public int ElbowRangeFactor
        {
            get
            {
                return _model.ElbowRangeFactor;
            }
            set
            {
                _model.ElbowRangeFactor = value;
                NotifyOfPropertyChange(() => ElbowRangeFactor);
            }
        }
        public int WristRangeFactor
        {
            get
            {
                return _model.WristRangeFactor;
            }
            set
            {
                _model.WristRangeFactor = value;
                NotifyOfPropertyChange(() => WristRangeFactor);
            }
        }
        public int GripperRangeFactor
        {
            get
            {
                return _model.GripperRangeFactor;
            }
            set
            {
                _model.GripperRangeFactor = value;
                NotifyOfPropertyChange(() => GripperRangeFactor);
            }
        }
        public int Gripper2RangeFactor
        {
            get
            {
                return _model.Gripper2RangeFactor;
            }
            set
            {
                _model.Gripper2RangeFactor = value;
                NotifyOfPropertyChange(() => Gripper2RangeFactor);
            }
        }

        public ControlMultipliersViewModel()
        {
            _model = new ControlMultipliersModel();
        }


    }
}
