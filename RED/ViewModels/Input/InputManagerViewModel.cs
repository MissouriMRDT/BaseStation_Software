using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input;
using RED.ViewModels.Input.Controllers;
using System.Collections.ObjectModel;

namespace RED.ViewModels.Input
{
    public class InputManagerViewModel : Screen
    {
        InputManagerModel _model;

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

        public InputManagerViewModel(IInputDevice[] devices, MappingViewModel[] mappings, IInputMode[] modes)
        {
            _model = new InputManagerModel();

            Devices = new ObservableCollection<IInputDevice>(devices);
            Mappings = new ObservableCollection<MappingViewModel>(mappings);
            Modes = new ObservableCollection<IInputMode>(modes);
        }

        public void Start()
        {
            foreach (var mapping in Mappings)
                if (mapping.IsActive)
                    ActivateMapping(mapping);
        }

        public void Stop()
        {
            foreach (var mapping in Mappings)
                if (mapping.IsActive)
                    DeactivateMapping(mapping);
        }

        public async void ActivateMapping(MappingViewModel mapping)
        {
            mapping.IsActive = true;
            await mapping.Start();
        }

        public void DeactivateMapping(MappingViewModel mapping)
        {
            mapping.Stop();
            mapping.IsActive = false;
        }

        public void AddDevice(IInputDevice device)
        {
            Devices.Add(device);
        }

        public void SaveMappingsToFile()
        {
            throw new System.NotImplementedException();
        }

        public void LoadMappingsFromFile()
        {
            throw new System.NotImplementedException();
        }
    }
}
