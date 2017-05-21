using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input;
using RED.ViewModels.Input.Controllers;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RED.ViewModels.Input
{
    public class InputManagerViewModel : Screen
    {
        InputManagerModel _model;
        ILogger _log;

        private XmlSerializer mappingsSerializer = new XmlSerializer(typeof(MappingViewModel[]));

        public int DefaultSerialReadSpeed
        {
            get
            {
                return _model.DefaultSerialReadSpeed;
            }
            set
            {
                _model.DefaultSerialReadSpeed = value;
                NotifyOfPropertyChange(() => DefaultSerialReadSpeed);
            }
        }

        public ObservableCollection<IInputDevice> Devices
        {
            get
            {
                return _model.Devices;
            }
            private set
            {
                _model.Devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }
        public ObservableCollection<MappingViewModel> Mappings
        {
            get
            {
                return _model.Mappings;
            }
            private set
            {
                _model.Mappings = value;
                NotifyOfPropertyChange(() => Mappings);
            }
        }
        public ObservableCollection<IInputMode> Modes
        {
            get
            {
                return _model.Modes;
            }
            private set
            {
                _model.Modes = value;
                NotifyOfPropertyChange(() => Modes);
            }
        }

        public InputManagerViewModel(ILogger log, IInputDevice[] devices, MappingViewModel[] mappings, IInputMode[] modes)
        {
            _model = new InputManagerModel();
            _log = log;

            Devices = new ObservableCollection<IInputDevice>(devices);
            Mappings = new ObservableCollection<MappingViewModel>(mappings);
            Modes = new ObservableCollection<IInputMode>(modes);
        }

        public void Start()
        {
        }

        public void Stop()
        {
            foreach (var mapping in Mappings)
                mapping.IsActive = false;
        }

        public void AddDevice(IInputDevice device)
        {
            Devices.Add(device);
        }

        public void SaveMappingsToFile(string url)
        {
            using (var stream = new FileStream(url, FileMode.Create))
            {
                mappingsSerializer.Serialize(stream, Mappings.ToArray());
            }
        }

        public void LoadMappingsFromFile(string url)
        {
            using (var stream = File.OpenRead(url))
            {
                MappingViewModel[] savedMappings = (MappingViewModel[])mappingsSerializer.Deserialize(stream);

                foreach (MappingViewModel mapping in savedMappings)
                {
                    mapping.Device = Devices.FirstOrDefault(x => x.DeviceType == mapping.DeviceType);
                    mapping.Mode = Modes.FirstOrDefault(x => x.ModeType == mapping.ModeType);
                    Mappings.Add(mapping);
                }

                _log.Log("Input Mappings loaded from file \"" + url + "\"");
            }
        }
    }
}
