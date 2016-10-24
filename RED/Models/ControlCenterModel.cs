using RED.ViewModels;
using RED.ViewModels.Input;
using RED.ViewModels.Modules;
using RED.ViewModels.Network;

namespace RED.Models
{
    public class ControlCenterModel
    {
        internal SettingsManagerViewModel _settingsManager;

        internal StateViewModel _stateManager;
        internal ConsoleViewModel _console;
        internal DataRouter _dataRouter;
        internal MetadataManager _metadataManager;
        internal SubscriptionManagerViewModel _subscriptionManager;
        internal NetworkManagerViewModel _networkManager;
        internal InputManagerViewModel _input;
        internal ScienceViewModel _science;
        internal GPSViewModel _GPS;
        internal SensorViewModel _sensor;
        internal SensorCombinedViewModel _sensorCombined;
        internal GimbalControllerModeViewModel _gimbalControllerMode;
        internal DropBaysViewModel _dropBays;
        internal PowerViewModel _power;
        internal CameraMuxViewModel _cameraMux;
        internal ExternalControlsViewModel _externalControls;
    }
}