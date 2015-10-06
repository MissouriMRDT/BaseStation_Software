namespace RED.Models
{
    using ViewModels;
    using ViewModels.ControlCenter;

    public class ControlCenterModel
    {
        internal SettingsManagerViewModel _settingsManager;

        internal SaveModuleStateViewModel _saveModuleState;
        internal RemoveModuleStateViewModel _removeModuleState;

        internal StateViewModel _stateManager;
        internal ConsoleViewModel _console;
        internal ModuleManagerViewModel _gridManager;
        internal DataRouter _dataRouter;
        internal MetadataManager _metadataManager;
        internal AsyncTcpServerViewModel _tcpAsyncServer;
        internal InputViewModel _input;
        internal ScienceViewModel _science;
        internal GPSViewModel _GPS;
        internal SensorViewModel _sensor;
        internal SensorCombinedViewModel _sensorCombined;
    }
}