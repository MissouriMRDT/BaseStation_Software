using Caliburn.Micro;
using RED.Addons.Navigation;
using Core.Interfaces;
using RED.Models.Modules;
using System;
using System.IO;
using Core.Models;
using System.Net;

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
        public float Pitch
        {
            get
            {
                return _model.Pitch;
            }
            set
            {
                _model.Pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }
        public float Roll
        {
            get
            {
                return _model.Roll;
            }
            set
            {
                _model.Roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }
        public float TrueHeading
        {
            get
            {
                return _model.TrueHeading;
            }
            set
            {
                _model.TrueHeading = value;
                NotifyOfPropertyChange(() => TrueHeading);
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

        public float Heading
        {
            get
            {
                return _model.Heading;
            }
            set
            {
                _model.Heading = value;
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
            _rovecomm.NotifyWhenMessageReceived(this, "NavPitch");
            _rovecomm.NotifyWhenMessageReceived(this, "NavRoll");
            _rovecomm.NotifyWhenMessageReceived(this, "NavTrueHeading");
            _rovecomm.NotifyWhenMessageReceived(this, "PitchHeadingRoll");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSQuality");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSPosition");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSSpeed");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSSpeedAngle");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSAltitude");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSSatellites");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSTelem");
            _rovecomm.NotifyWhenMessageReceived(this, "PitchHeadingRoll");
            _rovecomm.NotifyWhenMessageReceived(this, "RoverDistanceSession");
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "Lidar":
                    if (packet.Data[1] == 5) //we want to output the data only when the lidar is functioning and has cycled
                    {                          //Data[1] is 5 when it has cycled, and decrements when it has not
                        Lidar = (float)(packet.Data[0] / 10.0);
                    }
                    break;
                case "NavPitch": Pitch = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavRoll": Roll = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavTrueHeading":
                    TrueHeading = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)); break;
                case "PitchHeadingRoll":
                    Pitch = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0));
                    TrueHeading = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 2));
                    Heading = TrueHeading;
                    Roll = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 4));
                    break;

                case "GPSData":
                    var ms = new MemoryStream(packet.Data);
                    using (var br = new BinaryReader(ms))
                    {
                        FixObtained = br.ReadByte() != 0;
                        FixQuality = br.ReadByte();
                        NumberOfSatellites = br.ReadByte();
                        RawLocation = new GPSCoordinate()
                        {
                            Latitude = br.ReadInt32() / 10000000d,
                            Longitude = br.ReadInt32() / 10000000d
                        };
                        //CurrentAltitude = br.ReadSingle();
                        //Speed = br.ReadSingle();
                        //SpeedAngle = br.ReadSingle();
                    }
                    break;
                case "Heading":
                    Heading = BitConverter.ToSingle(packet.Data, 0);
                    break;
                
                case "GPSQuality":
                    FixObtained = packet.Data[0] != 0;
                    FixQuality = packet.Data[0];
                    break;
                case "GPSPosition":
                    RawLocation = new GPSCoordinate()
                    {
                        Latitude = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 1 * sizeof(Int32))) / 10000000d,
                        Longitude = -IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 0 * sizeof(Int32))) / 10000000d
                    };
                    
                    break;

                case "GPSTelem":
                    FixObtained = packet.Data[0] != 0;
                    FixQuality = packet.Data[0];
                    NumberOfSatellites = packet.Data[1];
                    break;
                case "GPSSatellites":
                    NumberOfSatellites = packet.Data[0];
                    break;
                case "RoverDistanceSession":
                    //RoverMetrics.txt should be found in RED/Bin/Debug
                    RoverDistanceTraveled = RoverDistanceStart + IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0))/1000.0f;
                    System.IO.File.WriteAllText(System.IO.Path.GetFullPath("RoverMetrics.txt"), RoverDistanceTraveled.ToString());
                    break;
            }
        }

		public void ReceivedRovecommMessageCallback(int index, bool reliable) {
			ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
		}

		private void RecalculateAntennaDirection()
        {
            var thetaRad = Math.Atan2(CurrentLocation.Latitude - BaseStationLocation.Latitude, CurrentLocation.Longitude - BaseStationLocation.Longitude);
            AntennaDirectionDeg = thetaRad / Math.PI * 180;
        }
    }
}