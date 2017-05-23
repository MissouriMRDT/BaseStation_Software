using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RED.ViewModels.Input
{
    public class InputSelectorViewModel : PropertyChangedBase
    {
        InputSelectorModel _model;
        ILogger _log;

        public IInputMode Mode
        {
            get
            {
                return _model.Mode;
            }
            private set
            {
                _model.Mode = value;
                NotifyOfPropertyChange(() => Mode);
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
            set
            {
                _model.Mappings = value;
                NotifyOfPropertyChange(() => Mappings);
                NotifyOfPropertyChange(() => SelectedMapping);
            }
        }

        public IInputDevice SelectedDevice
        {
            get
            {
                return _model.SelectedDevice;
            }
            set
            {
                _model.SelectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
                NotifyOfPropertyChange(() => EligibleMappings);
            }
        }
        public MappingViewModel SelectedMapping
        {
            get
            {
                return _model.SelectedMapping;
            }
            set
            {
                _model.SelectedMapping = value;
                NotifyOfPropertyChange(() => SelectedMapping);
                Stop();
            }
        }

        public bool Enabled
        {
            get
            {
                return _model.Enabled;
            }
            private set
            {
                _model.Enabled = value;
                NotifyOfPropertyChange(() => Enabled);
            }
        }
        public bool IsRunning
        {
            get
            {
                return _model.IsRunning;
            }
            private set
            {
                _model.IsRunning = value;
                NotifyOfPropertyChange(() => IsRunning);
            }
        }

        public IEnumerable<MappingViewModel> EligibleMappings
        {
            get
            {
                if (SelectedDevice == null || Mode == null) return Enumerable.Empty<MappingViewModel>();
                return Mappings.Where(x => x.DeviceType == SelectedDevice.DeviceType && x.ModeType == Mode.ModeType);
            }
        }

        public event EventHandler<IInputDevice> SwitchDevice;

        public InputSelectorViewModel(ILogger log, IInputMode mode, ObservableCollection<IInputDevice> devices, ObservableCollection<MappingViewModel> mappings)
        {
            _model = new InputSelectorModel();
            _log = log;
            Enabled = false;

            Mode = mode;
            Devices = devices;
            Mappings = mappings;
        }

        public async void Start()
        {
            if (Mode == null || SelectedDevice == null || SelectedMapping == null)
            {
                Stop();
                return;
            }

            SwitchDevice(this, SelectedDevice);

            try
            {
                IsRunning = true;
                while (IsRunning)
                {
                    if (SelectedDevice.IsReady())
                    {
                        if (!Enabled) Enable();
                        var rawValues = SelectedDevice.GetValues();
                        var mappedValues = SelectedMapping.Map(rawValues);
                        Mode.SetValues(mappedValues);
                    }
                    else
                    {
                        Disable();
                    }
                    await Task.Delay(SelectedMapping.UpdatePeriod);
                }
            }
            catch (KeyNotFoundException)
            {
                _log.Log(string.Format("Missing key when mapping {0} to {1} with {2}.", SelectedDevice.Name, Mode.Name, SelectedMapping.Name));
            }
            finally
            {
                Stop();
            }
        }

        public void Stop()
        {
            IsRunning = false;
            Disable();
        }

        public void Enable()
        {
            Mode.StartMode();
            Enabled = true;
        }
        public void Disable()
        {
            Enabled = false;
            Mode.StopMode();
        }

        public InputSelectionContext GetContext()
        {
            return new InputSelectionContext()
            {
                ModeName = Mode == null ? "" : Mode.Name,
                DeviceName = SelectedDevice == null ? "" : SelectedDevice.Name,
                MappingName = SelectedMapping == null ? "" : SelectedMapping.Name,
                Active = IsRunning
            };
        }

        public void SetContext(InputSelectionContext context)
        {
            SelectedDevice = Devices.FirstOrDefault(x => x.Name == context.DeviceName);
            SelectedMapping = Mappings.FirstOrDefault(x => x.Name == context.MappingName);
            if (context.Active) Start();
        }
    }
}
