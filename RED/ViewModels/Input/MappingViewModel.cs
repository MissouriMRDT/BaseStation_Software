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

        public string Name
        {
            get
            {
                return _model.Name;
            }
            set
            {
                _model.Name = value; NotifyOfPropertyChange(() => Name);
            }
        }
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

        private MappingViewModel()
        {
            _model = new MappingModel();

            Channels = new ObservableCollection<MappingChannelViewModel>();
        }

        public MappingViewModel(string name, string deviceType, string modeType, int updatePeriod, IEnumerable<MappingChannelViewModel> channels)
            : this()
        {
            Name = name;
            Channels = new ObservableCollection<MappingChannelViewModel>(channels);
            DeviceType = deviceType;
            ModeType = modeType;
            UpdatePeriod = updatePeriod;
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
