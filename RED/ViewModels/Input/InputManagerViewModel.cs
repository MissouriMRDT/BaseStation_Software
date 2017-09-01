using Caliburn.Micro;
using RED.Configurations.Input;
using RED.Contexts.Input;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace RED.ViewModels.Input
{
    public class InputManagerViewModel : PropertyChangedBase
    {
        private readonly InputManagerModel _model;
        private readonly ILogger _log;
        private readonly IConfigurationManager _configManager;

        private const string MappingsConfigName = "InputMappings";
        private const string SelectionsConfigName = "InputSelections";

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

        public InputManagerViewModel(ILogger log, IConfigurationManager configs, IInputDevice[] devices, MappingViewModel[] mappings, IInputMode[] modes)
        {
            _model = new InputManagerModel();
            _log = log;
            _configManager = configs;

            Devices = new ObservableCollection<IInputDevice>(devices);
            Mappings = new ObservableCollection<MappingViewModel>(mappings);
            Modes = new ObservableCollection<IInputMode>(modes);
            Selectors = new ObservableCollection<InputSelectorViewModel>();

            _configManager.AddRecord(MappingsConfigName, InputManagerConfig.DefaultInputMappings);
            _configManager.AddRecord(SelectionsConfigName, InputManagerConfig.DefaultInputSelections);

            InitializeMappings(_configManager.GetConfig<InputMappingsContext>(MappingsConfigName));
            InitializeSelections(_configManager.GetConfig<InputSelectionsContext>(SelectionsConfigName));
        }

        private void SelectorSwitchDevice(object sender, IInputDevice device)
        {
            foreach (var selector in Selectors.Where(x => x.SelectedDevice == device && x != sender))
                selector.Stop();
        }
        private void SelectorCycleMode(object sender, IInputDevice device)
        {
            Selectors.Concat(Selectors).SkipWhile(x => x != sender).Skip(1).First(x => x.SelectedDevice == device).Start();
        }

        public void StopAll()
        {
            foreach (var selector in Selectors)
                selector.Disable();
        }

        public void AddDevice(IInputDevice device)
        {
            Devices.Add(device);
        }

        public void SaveConfigurations()
        {
            _configManager.SetConfig(MappingsConfigName, new InputMappingsContext(Mappings.Select(x => x.ToContext()).ToArray()));
            _configManager.SetConfig(SelectionsConfigName, new InputSelectionsContext(Selectors.Select(x => x.GetContext()).ToArray()));
        }

        private void InitializeMappings(InputMappingsContext config)
        {
            foreach (InputMappingContext mapping in config.Mappings)
                Mappings.Add(new MappingViewModel(mapping));

            _log.Log("Input Mappings loaded");
        }

        private void InitializeSelections(InputSelectionsContext config)
        {
            foreach (IInputMode mode in Modes)
                Selectors.Add(new InputSelectorViewModel(_log, mode, Devices, Mappings));

            foreach (var selector in Selectors)
            {
                selector.SwitchDevice += SelectorSwitchDevice;
                selector.CycleMode += SelectorCycleMode;
            }

            foreach (InputSelectionContext selection in config.Selections)
                foreach (var selector in Selectors.Where(x => x.Mode.Name == selection.ModeName))
                    selector.SetContext(selection);
        }
    }
}
