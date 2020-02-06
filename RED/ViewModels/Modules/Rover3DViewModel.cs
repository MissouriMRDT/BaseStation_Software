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
    public class Rover3DViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly Rover3DModel _model;
        private readonly IDataIdResolver _idResolver;
        private readonly IRovecomm _rovecomm;

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
        public float Roll
        {
            get
            {
                return _model.Roll;
            }
            set
            {
                Rotate(Pitch, Yaw, value);
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
                Rotate(value, Yaw, Roll);
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
                Rotate(Pitch, value, Roll);
                _model.Yaw = value;
                NotifyOfPropertyChange(() => Yaw);
            }
        }

        public Rover3DViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver)
        {
            _model = new Rover3DModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;

            ModelImporter importer = new ModelImporter();
            RoverModel = importer.Load(@"../../Addons/Rover.stl");
            Rotate(0, 0, 0);
        }

        void Rotate(double p, double y, double r)
        {
            Transform3DGroup myTransform3DGroup = new Transform3DGroup();

            RotateTransform3D RollTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), r + 172))
            {
                CenterX = 12,
                CenterY = 0,
                CenterZ = 0
            };
            myTransform3DGroup.Children.Add(RollTransform);


            RotateTransform3D PitchTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), p - 39))
            {
                CenterX = 0,
                CenterY = 0,
                CenterZ = 0
            };
            myTransform3DGroup.Children.Add(PitchTransform);

            RotateTransform3D YawTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), y + 5))
            {
                CenterX = 20,
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
                      
                case "PitchHeadingRoll":
                    Pitch = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0));
                    Roll = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 4));
                    break;

                default:           
                    break;
            }
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
    }
}
