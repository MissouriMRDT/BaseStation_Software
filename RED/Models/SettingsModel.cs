using RED.ViewModels.Settings.Input;
using RED.ViewModels.Settings.Input.Controllers;
using RED.ViewModels.Settings.Modules;

namespace RED.Models
{
    internal class SettingsManagerModel
    {
        internal DriveSettingsViewModel drive;
        internal ScienceSettingsViewModel science;
        internal InputSettingsViewModel input;
        internal XboxControllerInputSettingsViewModel xbox;
        internal GPSSettingsViewModel gps;
    }
}