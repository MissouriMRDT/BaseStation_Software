using Core.RoveProtocol;
using Core.Configurations;
using RED.ViewModels;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using RED.ViewModels.Tools;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
using RED.Models.Modules;
using Core.Cameras;

namespace RED.Models
{
    internal class ControlCenterModel
    {
        internal SettingsManagerViewModel _settingsManager;

        internal ConsoleViewModel _console;
        internal XMLConfigManager _configManager;
        internal MetadataManager _metadataManager;
        internal Rovecomm _rovecomm;
        internal InputManagerViewModel _input;
        internal WaypointManagerViewModel _waypoint;
        internal StopwatchToolViewModel _stopwatchTool;
        internal Rover3DViewModel _RoverModel; 
        internal GPSViewModel _GPS;
        internal PowerViewModel _power;
        internal MapViewModel _map;
        internal DriveViewModel _drive;
        internal GimbalViewModel _gimbal;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;
        internal FlightStickViewModel _flightStickController;
        internal KeyboardInputViewModel _keyboardController;

        internal CameraViewModel _camera1;
        internal CameraViewModel _camera2;
        internal CameraViewModel _camera3;

        internal bool _networkManagerEnabled;
        internal bool _attachmentManagerEnabled;
    }
}
