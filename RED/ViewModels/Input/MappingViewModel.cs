using Caliburn.Micro;
using RED.Models.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RED.ViewModels.Input
{
    public class MappingViewModel : PropertyChangedBase
    {
        private MappingModel _model;

        public ObservableCollection<MappingChannelViewModel> Channels
        {
            get
            {
                return _model.Channels;
            }
            set
            {
                _model.Channels = value; NotifyOfPropertyChange(() => Channels);
            }

        }
        public string DeviceType
        {
            get
            {
                return _model.DeviceType;
            }
            set
            {
                _model.DeviceType = value; NotifyOfPropertyChange(() => DeviceType);
            }

        }
        public string ModeType
        {
            get
            {
                return _model.ModeType;
            }
            set
            {
                _model.ModeType = value; NotifyOfPropertyChange(() => ModeType);
            }

        }
        public uint UpdatePeriod
        {
            get
            {
                return _model.UpdatePeriod;
            }
            set
            {
                _model.UpdatePeriod = value; NotifyOfPropertyChange(() => UpdatePeriod);
            }

        }
        public bool IsActive
        {
            get
            {
                return _model.IsActive;
            }
            set
            {
                _model.IsActive = value; NotifyOfPropertyChange(() => IsActive);
            }

        }

        public MappingViewModel()
        {
            _model = new MappingModel();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, float> Map(Dictionary<string, float> values)
        {
            throw new System.NotImplementedException();
        }
    }
}
