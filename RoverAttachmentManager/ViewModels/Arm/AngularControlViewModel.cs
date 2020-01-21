using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.Arm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static RoverAttachmentManager.ViewModels.Arm.ArmViewModel;

namespace RoverAttachmentManager.ViewModels.Arm
{
    public class AngularControlViewModel : PropertyChangedBase, IRovecommReceiver
    {
        AngularControlModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private readonly IConfigurationManager _configManager;
        private const string PositionsConfigName = "ArmPositions";

        public float AngleJ1
        {
            get
            {
                return _model.AngleJ1;
            }
            set
            {
                _model.AngleJ1 = value;
                NotifyOfPropertyChange(() => AngleJ1);
            }
        }
        public float AngleJ2
        {
            get
            {
                return _model.AngleJ2;
            }
            set
            {
                _model.AngleJ2 = value;
                NotifyOfPropertyChange(() => AngleJ2);
            }
        }
        public float AngleJ3
        {
            get
            {
                return _model.AngleJ3;
            }
            set
            {
                _model.AngleJ3 = value;
                NotifyOfPropertyChange(() => AngleJ3);
            }
        }
        public float AngleJ4
        {
            get
            {
                return _model.AngleJ4;
            }
            set
            {
                _model.AngleJ4 = value;
                NotifyOfPropertyChange(() => AngleJ4);
            }
        }
        public float AngleJ5
        {
            get
            {
                return _model.AngleJ5;
            }
            set
            {
                _model.AngleJ5 = value;
                NotifyOfPropertyChange(() => AngleJ5);
            }
        }
        public float AngleJ6
        {
            get
            {
                return _model.AngleJ6;
            }
            set
            {
                _model.AngleJ6 = value;
                NotifyOfPropertyChange(() => AngleJ6);
            }
        }

        public ObservableCollection<ArmPositionViewModel> Positions
        {
            get
            {
                return _model.Positions;
            }
            private set
            {
                _model.Positions = value;
                NotifyOfPropertyChange(() => Positions);
            }
        }
        public ArmPositionViewModel SelectedPosition
        {
            get
            {
                return _model.SelectedPosition;
            }
            set
            {
                _model.SelectedPosition = value;
                NotifyOfPropertyChange(() => SelectedPosition);
            }
        }
        public ArmViewModel Arm
        {
            get
            {
                return _model.Arm;
            }
            set
            {
                _model.Arm = value;
                NotifyOfPropertyChange(() => Arm);
            }
        }

        public AngularControlViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, IConfigurationManager configs, ArmViewModel parent)
        {
            _model = new AngularControlModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            _configManager = configs;

            _configManager.AddRecord(PositionsConfigName, ArmConfig.DefaultArmPositions);
            InitializePositions(_configManager.GetConfig<ArmPositionsContext>(PositionsConfigName));

            Arm = parent;
        }

        public void GetPosition()
        {
            byte[] data = new byte[2];
            data[0] = 0;
            data[1] = 1;
            _rovecomm.SendCommand(new Packet("ArmCommands", data, 2, (byte)DataTypes.UINT8_T));
        }
        public void SetPosition()
        {
            UInt32[] angles = { (UInt32)(AngleJ1 * 1000), (UInt32)(AngleJ2 * 1000), (UInt32)(AngleJ3 * 1000), (UInt32)(AngleJ4 * 1000), (UInt32)(AngleJ5 * 1000), (UInt32)(AngleJ6 * 1000) };
            Array.Reverse(angles);
            byte[] data = new byte[angles.Length * sizeof(UInt32)];
            Buffer.BlockCopy(angles, 0, data, 0, data.Length);
            Array.Reverse(data);
            //TODO: Determine floats for this
            _rovecomm.SendCommand(new Packet("ArmToAngle", data, 6, (byte)DataTypes.UINT32_T));

            Arm.myState = ArmControlState.GuiControl;
            Arm.guiControlInitialized = true;

            _rovecomm.NotifyWhenMessageReceived(this, "ArmAngles");
        }

        public void ToggleAuto()
        {
            _rovecomm.SendCommand(new Packet("ToggleAutoPositionTelem"));
        }

        public void StorePosition()
        {
            Positions.Add(new ArmPositionViewModel()
            {
                Name = "Unnamed Position",
                J1 = AngleJ1,
                J2 = AngleJ2,
                J3 = AngleJ3,
                J4 = AngleJ4,
                J5 = AngleJ5,
                J6 = AngleJ6
            });
        }
        public void RecallPosition()
        {
            AngleJ1 = SelectedPosition.J1;
            AngleJ2 = SelectedPosition.J2;
            AngleJ3 = SelectedPosition.J3;
            AngleJ4 = SelectedPosition.J4;
            AngleJ5 = SelectedPosition.J5;
            AngleJ6 = SelectedPosition.J6;
        }
        public void DeletePosition()
        {
            Positions.Remove(SelectedPosition);
        }
        public void InitializePositions(ArmPositionsContext config)
        {
            foreach (var position in config.Positions)
                Positions.Add(new ArmPositionViewModel(position));
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ArmAngles":
                    //Array.Reverse(packet.Data);
                    AngleJ1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 0)) / 1000.0);
                    AngleJ2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 4)) / 1000.0);
                    AngleJ3 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 8)) / 1000.0);
                    AngleJ4 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 12)) / 1000.0);
                    AngleJ5 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 16)) / 1000.0);
                    AngleJ6 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 20)) / 1000.0);
                    break;

            }
        }
    }
}
