using RED.ViewModels;
using RED.ViewModels.Input;
using RED.ViewModels.Input.Controllers;
using RED.ViewModels.Modules;
using RED.ViewModels.Network;

namespace RED.Models
{
    internal class ControlCenterModel
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
        internal DropBaysViewModel _dropBays;
        internal PowerViewModel _power;
        internal CameraViewModel _cameraMux;
        internal ExternalControlsViewModel _externalControls;
        internal AutonomyViewModel _autonomy;
        internal ScienceArmViewModel _scienceArm;
        internal DriveControllerModeViewModel _driveControllerMode;
        internal ArmControllerModeViewModel _armControllerMode;
        internal GimbalControllerModeViewModel _gimbal1ControllerMode;
        internal GimbalControllerModeViewModel _gimbal2ControllerMode;
        internal XboxControllerInputViewModel _xboxController;
        internal FlightStickViewModel _flightStickController;
    }
}