using RED.ViewModels.Settings.Input.Controllers;
using RED.ViewModels.Settings.Modules;
using RED.ViewModels.Settings.Network;

namespace RED.Models
{
    internal class SettingsManagerModel
    {
        internal DriveSettingsViewModel Drive;
        internal ScienceSettingsViewModel Science;
        internal XboxControllerInputSettingsViewModel Xbox1;
        internal XboxControllerInputSettingsViewModel Xbox2;
        internal XboxControllerInputSettingsViewModel Xbox3;
        internal XboxControllerInputSettingsViewModel Xbox4;
        internal GPSSettingsViewModel GPS;
        internal PowerSettingsViewModel Power;
        internal NetworkManagerSettingsViewModel Network;
    }
}