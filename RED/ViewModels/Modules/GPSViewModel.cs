using Caliburn.Micro;
using RED.Addons.Navigation;
using Core.Interfaces;
using RED.Models.Modules;
using System;
using System.IO;
using Core.Models;
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
        public Model3D RoverModel
        {
            get
            {
                return _model.RoverModel;
            }
            set
            {
                _model.RoverModel = value;
                NotifyOfPropertyChange(() => RoverModel);
            }
        }
        public float HeadingDeg
        {
            get
            {
                return (float)(Heading * 180d / Math.PI);
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
                rotate(Pitch, Yaw, value);
                _model.Roll = value;
                NotifyOfPropertyChange(() => Roll);
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
                rotate(value, Yaw, Roll);
                _model.Pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }

        public float Yaw
        {
            get
            {
                return _model.Yaw;
            }
            set
            {
                rotate(Pitch, value, Roll);
                _model.Yaw = value;
                NotifyOfPropertyChange(() => Yaw);
            }
        }

        public GPSViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver)
        {
            _model = new GPSModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;

            ModelImporter importer = new ModelImporter();
            RoverModel = importer.Load(@"../../Addons/Rover.stl");
            rotate(0, 0, 0);

            _rovecomm.NotifyWhenMessageReceived(this, "GPSQuality");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSPosition");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSSpeed");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSSpeedAngle");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSAltitude");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSSatellites");
            _rovecomm.NotifyWhenMessageReceived(this, "GPSTelem");
            _rovecomm.NotifyWhenMessageReceived(this, "PitchHeadingRoll");
        }

        void rotate(double p, double y, double r)
        {
            Transform3DGroup myTransform3DGroup = new Transform3DGroup();

            RotateTransform3D RollTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), r+172))
            {
                CenterX = 12,
                CenterY = 0,
                CenterZ = 0
            };
            myTransform3DGroup.Children.Add(RollTransform);


            RotateTransform3D PitchTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), p-39))
            {
                CenterX = 0,
                CenterY = 0,
                CenterZ = 0
            };
            myTransform3DGroup.Children.Add(PitchTransform);

            RotateTransform3D YawTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), y+5))
            {
                CenterX = 12,
                CenterY = 0,
                CenterZ = 0
            };
            myTransform3DGroup.Children.Add(YawTransform);

            RoverModel.Transform = myTransform3DGroup;
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
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
                case "PitchHeadingRoll":
                    Heading = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 2));
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