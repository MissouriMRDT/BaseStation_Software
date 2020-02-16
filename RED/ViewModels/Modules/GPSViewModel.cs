using Caliburn.Micro;
using RED.Addons.Navigation;
using Core.Interfaces;
using RED.Models.Modules;
using System;
using System.IO;
using Core.RoveProtocol;
using System.Net;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace RED.ViewModels.Modules
{
    public class GPSViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly GPSModel _model;
        private readonly IDataIdResolver _idResolver;
        private readonly IRovecomm _rovecomm;
        private readonly ILogger _log;

        public float Lidar
        {
            get
            {
                return _model.Lidar;
            }
            set
            {
                _model.Lidar = value;
                NotifyOfPropertyChange(() => Lidar);
            }
        }

        public bool FixObtained
        {
            get
            {
                return _model.FixObtained;
            }
            set
            {
                _model.FixObtained = value;
                NotifyOfPropertyChange(() => FixObtained);
            }
        }
        public byte FixQuality
        {
            get
            {
                return _model.FixQuality;
            }
            set
            {
                _model.FixQuality = value;
                NotifyOfPropertyChange(() => FixQuality);
            }
        }
        public byte NumberOfSatellites
        {
            get
            {
                return _model.NumberOfSatellites;
            }
            set
            {
                _model.NumberOfSatellites = value;
                NotifyOfPropertyChange(() => NumberOfSatellites);
            }
        }
        public GPSCoordinate CurrentLocation
        {
            get
            {
                return _model.CurrentLocation;
            }
            set
            {
                _model.CurrentLocation = value;
                NotifyOfPropertyChange(() => CurrentLocation);
            }
        }
        public GPSCoordinate RawLocation
        {
            get
            {
                return _model.RawLocation;
            }
            set
            {
                _model.RawLocation = value;
                CurrentLocation = new GPSCoordinate(RawLocation.Latitude + Offset.Latitude,
                    RawLocation.Longitude + Offset.Longitude);
                NotifyOfPropertyChange(() => RawLocation);
                NotifyOfPropertyChange(() => CurrentLocation);
            }
        }

        public GPSCoordinate Offset
        {
            get
            {
                return _model.Offset;
            }
            set
            {
                _model.Offset = value;
                NotifyOfPropertyChange(() => Offset);
            }
        }
        public GPSCoordinate BaseStationLocation
        {
            get
            {
                return _model.BaseStationLocation;
            }
            set
            {
                _model.BaseStationLocation = value;
                NotifyOfPropertyChange(() => BaseStationLocation);
                RecalculateAntennaDirection();
            }
        }
        public double AntennaDirectionDeg
        {
            get
            {
                return _model.AntennaDirectionDeg;
            }
            set
            {
                _model.AntennaDirectionDeg = value;
                NotifyOfPropertyChange(() => AntennaDirectionDeg);
            }
        }


        public float RoverDistanceStart
        {

            get
            {
                return _model.RoverDistanceStart;
            }
            set
            {
                _model.RoverDistanceStart = value;
                NotifyOfPropertyChange(() => RoverDistanceStart);

            }
        }
        public float RoverDistanceTraveled
        {

            get
            {
                return _model.RoverDistanceTraveled;
            }
            set
            {
                _model.RoverDistanceTraveled = value;
                NotifyOfPropertyChange(() => RoverDistanceTraveled);
            }
        }

        public GPSViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new GPSModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            if (File.Exists(System.IO.Path.GetFullPath("RoverMetrics.txt")))
            {
                //RoverMetrics.txt should be found in RED/Bin/Debug
                RoverDistanceStart = float.Parse(System.IO.File.ReadAllText(System.IO.Path.GetFullPath("RoverMetrics.txt")));
            }
            RoverDistanceTraveled = RoverDistanceStart;


            _rovecomm.NotifyWhenMessageReceived(this, "Lidar");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSPosition");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSTelem");
            _rovecomm.NotifyWhenMessageReceived(this, "RoverDistanceSession");
        }



        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "GPSPosition":
                    RawLocation = new GPSCoordinate()
                    {
                        Latitude = packet.GetDataArray<Int32>()[0] / 10000000d,
                        Longitude = packet.GetDataArray<Int32>()[1] / -10000000d
                    };
                    break;

                case "PitchHeadingRoll":
                    Pitch = packet.GetDataArray<Int16>()[0];
                    Heading = packet.GetDataArray<Int16>()[1];
                    Roll = packet.GetDataArray<Int16>()[2];
                    break;

                /* not actually possible from n3?
                case "RoverDistanceSession":
                    //RoverMetrics.txt should be found in RED/Bin/Debug
                    RoverDistanceTraveled = RoverDistanceStart + IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0))/1000.0f;
                    System.IO.File.WriteAllText(System.IO.Path.GetFullPath("RoverMetrics.txt"), RoverDistanceTraveled.ToString());
                    break;
                */
            }
        }

		private void RecalculateAntennaDirection()
        {
            var thetaRad = Math.Atan2(CurrentLocation.Latitude - BaseStationLocation.Latitude, CurrentLocation.Longitude - BaseStationLocation.Longitude);
            AntennaDirectionDeg = thetaRad / Math.PI * 180;
        }
    }
}
