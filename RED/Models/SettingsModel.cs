using RED.ViewModels.Settings.Input.Controllers;
using RED.ViewModels.Settings.Modules;
using RED.ViewModels.Settings.Network;

namespace RED.Models
{
    internal class SettingsManagerModel
    {
        internal DriveSettingsViewModel drive;
        internal ScienceSettingsViewModel science;
        internal XboxControllerInputSettingsViewModel xbox1;
        internal XboxControllerInputSettingsViewModel xbox2;
        internal XboxControllerInputSettingsViewModel xbox3;
        internal XboxControllerInputSettingsViewModel xbox4;
        internal GPSSettingsViewModel gps;
        internal PowerSettingsViewModel power;
        internal NetworkManagerSettingsViewModel network;
    }
}