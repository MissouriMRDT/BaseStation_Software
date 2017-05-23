using Caliburn.Micro;
using RED.Contexts;
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
    public class InputManagerViewModel : PropertyChangedBase
    {
        InputManagerModel _model;
        ILogger _log;

        private XmlSerializer mappingsSerializer = new XmlSerializer(typeof(MappingViewModel[]));
        private XmlSerializer selectionsSerializer = new XmlSerializer(typeof(InputSelectionContext[]));

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

        public ObservableCollection<InputSelectorViewModel> Selectors
        {
            get
            {
                return _model.Selectors;
            }
            private set
            {
                _model.Selectors = value;
                NotifyOfPropertyChange(() => Selectors);
            }
        }

        public InputManagerViewModel(ILogger log, IInputDevice[] devices, MappingViewModel[] mappings, IInputMode[] modes)
        {
            _model = new InputManagerModel();
            _log = log;

            Devices = new ObservableCollection<IInputDevice>(devices);
            Mappings = new ObservableCollection<MappingViewModel>(mappings);
            Modes = new ObservableCollection<IInputMode>(modes);
            Selectors = new ObservableCollection<InputSelectorViewModel>();

            foreach (IInputMode mode in Modes)
                Selectors.Add(new InputSelectorViewModel(_log, mode, Devices, Mappings));
        }

        public void Start()
        {
        }

        public void Stop()
        {
            foreach (var selector in Selectors)
                selector.Disable();
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
                    Mappings.Add(mapping);

                _log.Log("Input Mappings loaded from file \"" + url + "\"");
            }
        }

        public void SaveSelectionsToFile(string url)
        {
            using (var stream = new FileStream(url, FileMode.Create))
            {
                selectionsSerializer.Serialize(stream, Selectors.Select(x => x.GetContext()).ToArray());
            }
        }

        public void LoadSelectionsFromFile(string url)
        {
            using (var stream = File.OpenRead(url))
            {
                InputSelectionContext[] savedSelections = (InputSelectionContext[])selectionsSerializer.Deserialize(stream);

                foreach (InputSelectionContext selection in savedSelections)
                    foreach (var selector in Selectors.Where(x => x.Mode.Name == selection.ModeName))
                        selector.SetContext(selection);
            }
        }
    }
}
