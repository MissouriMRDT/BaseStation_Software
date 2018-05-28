using RED.Roveprotocol;
using RED.ViewModels;
using RED.ViewModels.Input;
using RED.ViewModels.Input.Controllers;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using RED.ViewModels.Network;
using RED.ViewModels.Tools;

namespace RED.Models
{
    internal class ControlCenterModel
    {
        internal SettingsManagerViewModel _settingsManager;

        internal ConsoleViewModel _console;
        internal XMLConfigManager _configManager;
        internal MetadataManager _metadataManager;
        internal Rovecomm _rovecomm;
        internal NetworkManagerViewModel _networkManager;
        internal InputManagerViewModel _input;
        internal WaypointManagerViewModel _waypoint;
        internal PingToolViewModel _pingTool;
        internal StopwatchToolViewModel _stopwatchTool;
        internal TelemetryLogToolViewModel _telemetryLogTool;

        internal ScienceViewModel _science;
        internal GPSViewModel _GPS;
        internal SensorViewModel _sensor;
        internal DropBaysViewModel _dropBays;
        internal PowerViewModel _power;
        internal CameraViewModel _cameraMux;
        internal ExternalControlsViewModel _externalControls;
        internal AutonomyViewModel _autonomy;
        internal LightingViewModel _lighting;
        internal MapViewModel _map;
        internal DriveViewModel _drive;
        internal ArmViewModel _arm;
        internal GimbalViewModel _gimbal;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;
        internal XboxControllerInputViewModel _xboxController4;
        internal FlightStickViewModel _flightStickController;
        internal KeyboardInputViewModel _keyboardController;
    }
}
