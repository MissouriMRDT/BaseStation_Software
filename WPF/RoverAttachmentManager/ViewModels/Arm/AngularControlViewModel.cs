using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RoverAttachmentManager.Configurations.Modules;
using RoverAttachmentManager.Contexts;
using RoverAttachmentManager.Models.Arm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
            //_rovecomm.SendCommand(Packet.Create("ToggleAutoPositionTelem"));
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

        public void SaveConfigurations()
        {
            _configManager.SetConfig(PositionsConfigName, new ArmPositionsContext(Positions.Select(x => x.GetContext()).ToArray()));
        }

        public void InitializePositions(ArmPositionsContext config)
        {
            foreach (var position in config.Positions)
                Positions.Add(new ArmPositionViewModel(position));
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ArmAngles":
                    float[] angles = packet.GetDataArray<float>();
                    AngleJ1 = (float)(angles[0]);
                    AngleJ2 = (float)(angles[1]);
                    AngleJ3 = (float)(angles[2]);
                    AngleJ4 = (float)(angles[3]);
                    AngleJ5 = (float)(angles[4]);
                    AngleJ6 = (float)(angles[5]);
                    break;

            }
        }

        public class ArmPositionViewModel : PropertyChangedBase
        {
            private readonly AngularControlModel.ArmPositionModel _model;

            public string Name
            {
                get
                {
                    return _model.Name;
                }
                set
                {
                    _model.Name = value; NotifyOfPropertyChange(() => Name);
                }

            }
            public float J1
            {
                get
                {
                    return _model.J1;
                }
                set
                {
                    _model.J1 = value; NotifyOfPropertyChange(() => J1);
                }

            }
            public float J2
            {
                get
                {
                    return _model.J2;
                }
                set
                {
                    _model.J2 = value; NotifyOfPropertyChange(() => J2);
                }

            }
            public float J3
            {
                get
                {
                    return _model.J3;
                }
                set
                {
                    _model.J3 = value; NotifyOfPropertyChange(() => J3);
                }

            }
            public float J4
            {
                get
                {
                    return _model.J4;
                }
                set
                {
                    _model.J4 = value; NotifyOfPropertyChange(() => J4);
                }

            }
            public float J5
            {
                get
                {
                    return _model.J5;
                }
                set
                {
                    _model.J5 = value; NotifyOfPropertyChange(() => J5);
                }

            }
            public float J6
            {
                get
                {
                    return _model.J6;
                }
                set
                {
                    _model.J6 = value; NotifyOfPropertyChange(() => J6);
                }

            }

            public ArmPositionViewModel()
            {
                _model = new AngularControlModel.ArmPositionModel();
            }

            public ArmPositionViewModel(ArmPositionContext ctx)
                : this()
            {
                Name = ctx.Name;
                J1 = ctx.J1;
                J2 = ctx.J2;
                J3 = ctx.J3;
                J4 = ctx.J4;
                J5 = ctx.J5;
                J6 = ctx.J6;
            }

            public ArmPositionContext GetContext()
            {
                return new ArmPositionContext(Name, J1, J2, J3, J4, J5, J6);
            }
        }
    }
}
