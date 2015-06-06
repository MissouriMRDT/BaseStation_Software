using RED.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models
{
    public class SettingsManagerModel
    {
        internal NetworkSettingsViewModel network;
        internal DriveSettingsViewModel drive;
        internal ScienceSettingsViewModel science;
        internal InputSettingsViewModel input;
        internal GPSSettingsViewModel gps;
    }
}