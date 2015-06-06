using Caliburn.Micro;
using RED.Addons;
using RED.Models;
using RED.ViewModels.ControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Settings
{
    public class GPSSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private GPSViewModel _vm;

        public double BaseStationLocationLatitude
        {
            get
            {
                return _vm.BaseStationLocation.Latitude;
            }
            set
            {
                _vm.BaseStationLocation = new GPSCoordinate(value, _vm.BaseStationLocation.Longitude);
                _settings.CurrentSettings.GPSBaseStationLocationLatitude = value;
                NotifyOfPropertyChange(() => BaseStationLocationLatitude);
            }
        }

        public double BaseStationLocationLongitude
        {
            get
            {
                return _vm.BaseStationLocation.Longitude;
            }
            set
            {
                _vm.BaseStationLocation = new GPSCoordinate(_vm.BaseStationLocation.Latitude, value);
                _settings.CurrentSettings.GPSBaseStationLocationLongitude = value;
                NotifyOfPropertyChange(() => BaseStationLocationLongitude);
            }
        }

        public GPSSettingsViewModel(SettingsManagerViewModel settings, GPSViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.BaseStationLocation = new GPSCoordinate(_settings.CurrentSettings.GPSBaseStationLocationLatitude, _settings.CurrentSettings.GPSBaseStationLocationLongitude);
        }
    }
}