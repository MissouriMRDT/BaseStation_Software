using Caliburn.Micro;
using RED.Addons;
using RED.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GPSViewModel : PropertyChangedBase
    {
        GPSModel _model;
        ControlCenterViewModel _cc;
        public GPSCoordinate CurrentLocation
        {
            get
            {
                return _model.currentLocation;
            }
            set
            {
                _model.currentLocation = value;
                NotifyOfPropertyChange(() => CurrentLocation);
            }
        }

        public float CurrentAltitude
        {
            get
            {
                return _model.currentAltitude;
            }
            set
            {
                _model.currentAltitude = value;
                NotifyOfPropertyChange(() => CurrentAltitude);
            }
        }

        public bool FixObtained
        {
            get
            {
                return _model.fixObtained;
            }
            set
            {
                _model.fixObtained = value;
                NotifyOfPropertyChange(() => FixObtained);
            }
        }

        public ObservableCollection<GPSCoordinate> Waypoints
        {
            get
            {
                return _model.waypoints;
            }
            set
            {
                _model.waypoints = value;
                NotifyOfPropertyChange(() => Waypoints);
            }
        }

        public GPSViewModel(ControlCenterViewModel cc)
        {
            _model = new GPSModel();
            _cc = cc;
        }
    }
}