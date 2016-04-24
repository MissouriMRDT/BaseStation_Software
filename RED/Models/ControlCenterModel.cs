namespace RED.Models
{
    using ViewModels;
    using ViewModels.ControlCenter;
    using Interfaces;

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
    }
}