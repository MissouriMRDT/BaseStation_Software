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

        public float Ph
        {
            get
            {
                return _model.Ph;
            }
            set
            {
                _model.Ph = value;
                NotifyOfPropertyChange(() => Ph);
            }
        }
        public short Moisture
        {
            get
            {
                return _model.Moisture;
            }
            set
            {
                _model.Moisture = value;
                NotifyOfPropertyChange(() => Moisture);
            }
        }

        public short CCDPixelCount
        {
            get
            {
                return _model.CCDPixelCount;
            }
            set
            {
                _model.CCDPixelCount = value;
                NotifyOfPropertyChange(() => CCDPixelCount);
            }
        }
        public float CCDProgress
        {
            get
            {
                return _model.CCDProgress;
            }
            set
            {
                _model.CCDProgress = value;
                NotifyOfPropertyChange(() => CCDProgress);
            }
        }
        public string CCDFilePath
        {
            get
            {
                return _model.CCDFilePath;
            }
            set
            {
                _model.CCDFilePath = value;
                NotifyOfPropertyChange(() => CCDFilePath);
            }
        }
        public bool CCDIsReceiving
        {
            get
            {
                return _model.CCDIsReceiving;
            }
            set
            {
                _model.CCDIsReceiving = value;
                NotifyOfPropertyChange(() => CCDIsReceiving);
            }
        }

        public ScienceViewModel(ControlCenterViewModel cc)
        {
            _model = new ScienceModel();
            _cc = cc;
        }
    }
}