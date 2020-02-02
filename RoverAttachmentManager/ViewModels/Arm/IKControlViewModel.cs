using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.Arm;
using RoverAttachmentManager.Models.ArmModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static RoverAttachmentManager.ViewModels.Arm.ArmViewModel;

namespace RoverAttachmentManager.ViewModels.Arm
{
    public class IKControlViewModel : PropertyChangedBase, IRovecommReceiver
    {
        
        private readonly IKControlModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        
        public float CoordinateX
        {
            get
            {
                return _model.CoordinateX;
            }
            set
            {
                _model.CoordinateX = value;
                NotifyOfPropertyChange(() => CoordinateX);
            }
        }
        public float CoordinateY
        {
            get
            {
                return _model.CoordinateY;
            }
            set
            {
                _model.CoordinateY = value;
                NotifyOfPropertyChange(() => CoordinateY);
            }
        }
        public float CoordinateZ
        {
            get
            {
                return _model.CoordinateZ;
            }
            set
            {
                _model.CoordinateZ = value;
                NotifyOfPropertyChange(() => CoordinateZ);
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
                _model.Yaw = value;
                NotifyOfPropertyChange(() => Yaw);
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
        public float OpX
        {
            get
            {
                return _model.OpX;
            }
            set
            {
                _model.OpX = value;
                NotifyOfPropertyChange(() => OpX);
            }
        }
        public float OpY
        {
            get
            {
                return _model.OpY;
            }
            set
            {
                _model.OpY = value;
                NotifyOfPropertyChange(() => OpY);
            }
        }
        public float OpZ
        {
            get
            {
                return _model.OpZ;
            }
            set
            {
                _model.OpZ = value;
                NotifyOfPropertyChange(() => OpZ);
            }
        }
        public ArmViewModel Arm
        {
            get
            {
                return _model._arm;
            }
            set
            {
                _model._arm = value;
                NotifyOfPropertyChange(() => Arm);
            }
        }


        public IKControlViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, ArmViewModel parent)
        {
            _model = new IKControlModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            Arm = parent;

            _rovecomm.NotifyWhenMessageReceived(this, "ArmCurrentXYZ");
        }
        public void GetPosition()
        {
            byte[] data = new byte[2];
            data[0] = 0;
            data[1] = 1;
            _rovecomm.SendCommand(new Packet("ArmCommands", data, 2, (byte)DataTypes.UINT8_T));
        }
        public void ToggleAuto()
        {
            _rovecomm.SendCommand(new Packet("ToggleAutoPositionTelem"));
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {

                case "ArmCurrentXYZ":
                    CoordinateX = BitConverter.ToSingle(packet.Data, 0 * sizeof(float));
                    CoordinateY = BitConverter.ToSingle(packet.Data, 1 * sizeof(float));
                    CoordinateZ = BitConverter.ToSingle(packet.Data, 2 * sizeof(float));
                    Yaw = BitConverter.ToSingle(packet.Data, 3 * sizeof(float));
                    Pitch = BitConverter.ToSingle(packet.Data, 4 * sizeof(float));
                    Roll = BitConverter.ToSingle(packet.Data, 5 * sizeof(float));
                    break;
            }
        }
        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
        public void SetOpPoint()
        {
            float[] opPoints = { OpX, OpY, OpZ };
            byte[] data = new byte[opPoints.Length * sizeof(float)];
            Buffer.BlockCopy(opPoints, 0, data, 0, data.Length);

            // TODO: Determine floats for this
            //_rovecomm.SendCommand(_idResolver.GetId("OpPoint"), data, true);
        }
        public void GetXYZPosition()
        {
            _rovecomm.SendCommand(new Packet("ArmGetXYZ"));
        }
        public void SetXYZPosition()
        {
            float[] coordinates = { CoordinateX, CoordinateY, CoordinateZ, Yaw, Pitch, Roll };
            byte[] data = new byte[coordinates.Length * sizeof(float)];
            Buffer.BlockCopy(coordinates, 0, data, 0, data.Length);

            // TODO: floats
            //_rovecomm.SendCommand(_idResolver.GetId("ArmAbsoluteXYZ"), data, true);

            Arm.myState = ArmControlState.GuiControl;
        }
    }
}