using Caliburn.Micro;
using RED.Addons;
using RED.Interfaces;
using RED.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GPSViewModel : PropertyChangedBase, ISubscribe
    {
        GPSModel _model;
        ControlCenterViewModel _cc;

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
        public byte FixQuality
        {
            get
            {
                return _model.fixQuality;
            }
            set
            {
                _model.fixQuality = value;
                NotifyOfPropertyChange(() => FixQuality);
            }
        }
        public byte NumberOfSatellites
        {
            get
            {
                return _model.numberOfSatellites;
            }
            set
            {
                _model.numberOfSatellites = value;
                NotifyOfPropertyChange(() => NumberOfSatellites);
            }
        }
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
        public float Speed
        {
            get
            {
                return _model.speed;
            }
            set
            {
                _model.speed = value;
                NotifyOfPropertyChange(() => Speed);
            }
        }
        public float SpeedAngle
        {
            get
            {
                return _model.speedAngle;
            }
            set
            {
                _model.speedAngle = value;
                NotifyOfPropertyChange(() => SpeedAngle);
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

            Waypoints.Add(new GPSCoordinate(37.951631, -91.777713));//Rolla
            Waypoints.Add(new GPSCoordinate(37.850025, -91.701845));//Fugitive Beach
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            var ms = new MemoryStream(data);
            using (var br = new BinaryReader(ms))
            {
                FixObtained = br.ReadBoolean();
                CurrentLocation = new GPSCoordinate()
                {
                    Longitude = br.ReadInt32(),
                    Latitude = br.ReadInt32()
                };
                CurrentAltitude = br.ReadSingle();
                Speed = br.ReadSingle();
                SpeedAngle = br.ReadSingle();
                FixQuality = br.ReadByte();
                NumberOfSatellites = br.ReadByte();
            }
        }
    }
}