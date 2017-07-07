using Caliburn.Micro;
using RED.Addons;
using RED.Interfaces;
using RED.Models.Modules;
using System;
using System.IO;

namespace RED.ViewModels.Modules
{
    public class GPSViewModel : PropertyChangedBase, ISubscribe
    {
        GPSModel _model;
        private IDataIdResolver _idResolver;
        private IDataRouter _router;

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
        public GPSCoordinate BaseStationLocation
        {
            get
            {
                return _model.baseStationLocation;
            }
            set
            {
                _model.baseStationLocation = value;
                NotifyOfPropertyChange(() => BaseStationLocation);
                RecalculateAntennaDirection();
            }
        }
        public double AntennaDirectionDeg
        {
            get
            {
                return _model.antennaDirectionDeg;
            }
            set
            {
                _model.antennaDirectionDeg = value;
                NotifyOfPropertyChange(() => AntennaDirectionDeg);
            }
        }

        public float Heading
        {
            get
            {
                return _model.heading;
            }
            set
            {
                _model.heading = value;
                NotifyOfPropertyChange(() => Heading);
                NotifyOfPropertyChange(() => HeadingDeg);
            }
        }
        public float HeadingDeg
        {
            get
            {
                return (float)(Heading * 180d / Math.PI);
            }
        }

        public GPSViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new GPSModel();
            _router = router;
            _idResolver = idResolver;

            _router.Subscribe(this, _idResolver.GetId("GPSQuality"));
            _router.Subscribe(this, _idResolver.GetId("GPSPosition"));
            _router.Subscribe(this, _idResolver.GetId("GPSSpeed"));
            _router.Subscribe(this, _idResolver.GetId("GPSSpeedAngle"));
            _router.Subscribe(this, _idResolver.GetId("GPSAltitude"));
            _router.Subscribe(this, _idResolver.GetId("GPSSatellites"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "GPSData":
                    var ms = new MemoryStream(data);
                    using (var br = new BinaryReader(ms))
                    {
                        FixObtained = br.ReadByte() != 0;
                        FixQuality = br.ReadByte();
                        NumberOfSatellites = br.ReadByte();
                        CurrentLocation = new GPSCoordinate()
                        {
                            Latitude = br.ReadInt32() / 10000000f,
                            Longitude = br.ReadInt32() / 10000000f
                        };
                        CurrentAltitude = br.ReadSingle();
                        Speed = br.ReadSingle();
                        SpeedAngle = br.ReadSingle();
                    }
                    break;
                case "Heading":
                    Heading = BitConverter.ToSingle(data, 0);
                    break;
                case "GPSQuality":
                    FixObtained = data[0] != 0;
                    FixQuality = data[0];
                    break;
                case "GPSPosition":
                    CurrentLocation = new GPSCoordinate()
                    {
                        Latitude = BitConverter.ToInt32(data, 1 * sizeof(Int32)) / 10000000f,
                        Longitude = -BitConverter.ToInt32(data, 0 * sizeof(Int32)) / 10000000f
                    };
                    break;
                case "GPSSpeed":
                    Speed = BitConverter.ToSingle(data, 0);
                    break;
                case "GPSSpeedAngle":
                    SpeedAngle = BitConverter.ToSingle(data, 0);
                    break;
                case "GPSAltitude":
                    CurrentAltitude = BitConverter.ToSingle(data, 0);
                    break;
                case "GPSSatellites":
                    NumberOfSatellites = data[0];
                    break;
            }
        }

        private void RecalculateAntennaDirection()
        {
            var thetaRad = Math.Atan2(CurrentLocation.Latitude - BaseStationLocation.Latitude, CurrentLocation.Longitude - BaseStationLocation.Longitude);
            AntennaDirectionDeg = thetaRad / Math.PI * 180;
        }
    }
}