using Caliburn.Micro;
using RED.Addons;
using RED.ViewModels.Modules;

namespace RED.ViewModels.Settings.Modules
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

        public double StartLocationLatitude
        {
            get
            {
                return _settings.CurrentSettings.GPSStartLocationLatitude;
            }
            set
            {
                _settings.CurrentSettings.GPSStartLocationLatitude = value;
                NotifyOfPropertyChange(() => StartLocationLatitude);
            }
        }

        public double StartLocationLongitude
        {
            get
            {
                return _settings.CurrentSettings.GPSStartLocationLongitude;
            }
            set
            {
                _settings.CurrentSettings.GPSStartLocationLongitude = value;
                NotifyOfPropertyChange(() => StartLocationLongitude);
            }
        }

        public GPSSettingsViewModel(SettingsManagerViewModel settings, GPSViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.BaseStationLocation = new GPSCoordinate(_settings.CurrentSettings.GPSBaseStationLocationLatitude, _settings.CurrentSettings.GPSBaseStationLocationLongitude);
        }

        public void SetLocation(string presetName)
        {
            switch (presetName)
            {
                case "SDELC":
                    StartLocationLatitude = 37.951631;
                    StartLocationLongitude = -91.777713;
                    break;
                case "FugitiveBeach":
                    StartLocationLatitude = 37.850025;
                    StartLocationLongitude = -91.701845;
                    break;
                case "HanksvilleInn":
                    StartLocationLatitude = 38.373933;
                    StartLocationLongitude = -110.708362;
                    break;
                case "MDRS":
                    StartLocationLatitude = 38.406426;
                    StartLocationLongitude = -110.791919;
                    break;
                case "Kielce":
                    StartLocationLatitude = 50.782542;
                    StartLocationLongitude = 20.462027;
                    break;
            }
        }
    }
}