using Caliburn.Micro;
using RED.Interfaces.Input;
using RED.Models.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RED.ViewModels.Input
{
    [XmlType(TypeName = "Mapping")]
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
        public int UpdatePeriod
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

        [XmlIgnore]
        public IInputDevice Device { get; set; }
        [XmlIgnore]
        public IInputMode Mode { get; set; }

        [XmlIgnore]
        private bool isRunning = false;

        public MappingViewModel()
        {
            _model = new MappingModel();

            Channels = new ObservableCollection<MappingChannelViewModel>();
        }

        public async Task Start()
        {
            if (Device == null || Mode == null) return;

            try
            {
                Mode.StartMode();
                Device.StartDevice();
                isRunning = true;
                while (isRunning)
                {
                    var values = Device.GetValues();
                    Mode.SetValues(Map(values));
                    await Task.Delay(UpdatePeriod);
                }
            }
            catch
            {
                Stop();
            }
        }

        public void Stop()
        {
            isRunning = false;
            if (Device != null) Device.StopDevice();
            if (Mode != null) Mode.StopMode();
        }

        public Dictionary<string, float> Map(Dictionary<string, float> values)
        {
            var result = new Dictionary<string, float>();
            foreach (var channel in Channels)
                result.Add(channel.OutputKey, channel.Map(values[channel.InputKey]));
            return result;
        }
    }
}
