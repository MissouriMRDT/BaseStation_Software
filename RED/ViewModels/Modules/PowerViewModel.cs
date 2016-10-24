using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using System;

namespace RED.ViewModels.Modules
{
    public class PowerViewModel : PropertyChangedBase, ISubscribe
    {
        private ControlCenterViewModel _cc;
        private PowerModel _model;

        public float Motor1Current
        {
            get
            {
                return _model.Motor1Current;
            }
            set
            {
                _model.Motor1Current = value;
                NotifyOfPropertyChange(() => Motor1Current);
            }
        }
        public float Motor2Current
        {
            get
            {
                return _model.Motor2Current;
            }
            set
            {
                _model.Motor2Current = value;
                NotifyOfPropertyChange(() => Motor2Current);
            }
        }
        public float Motor3Current
        {
            get
            {
                return _model.Motor3Current;
            }
            set
            {
                _model.Motor3Current = value;
                NotifyOfPropertyChange(() => Motor3Current);
            }
        }
        public float Motor4Current
        {
            get
            {
                return _model.Motor4Current;
            }
            set
            {
                _model.Motor4Current = value;
                NotifyOfPropertyChange(() => Motor4Current);
            }
        }
        public float Motor5Current
        {
            get
            {
                return _model.Motor5Current;
            }
            set
            {
                _model.Motor5Current = value;
                NotifyOfPropertyChange(() => Motor5Current);
            }
        }
        public float Motor6Current
        {
            get
            {
                return _model.Motor6Current;
            }
            set
            {
                _model.Motor6Current = value;
                NotifyOfPropertyChange(() => Motor6Current);
            }
        }
        public float Motor7Current
        {
            get
            {
                return _model.Motor7Current;
            }
            set
            {
                _model.Motor7Current = value;
                NotifyOfPropertyChange(() => Motor7Current);
            }
        }
        public float Motor8Current
        {
            get
            {
                return _model.Motor8Current;
            }
            set
            {
                _model.Motor8Current = value;
                NotifyOfPropertyChange(() => Motor8Current);
            }
        }
        public float Bus5VCurrent
        {
            get
            {
                return _model.Bus5VCurrent;
            }
            set
            {
                _model.Bus5VCurrent = value;
                NotifyOfPropertyChange(() => Bus5VCurrent);
            }
        }
        public float Bus12VCurrent
        {
            get
            {
                return _model.Bus12VCurrent;
            }
            set
            {
                _model.Bus12VCurrent = value;
                NotifyOfPropertyChange(() => Bus12VCurrent);
            }
        }
        public float InputVoltage
        {
            get
            {
                return _model.InputVoltage;
            }
            set
            {
                _model.InputVoltage = value;
                NotifyOfPropertyChange(() => InputVoltage);
            }
        }

        public float Cell1Voltage
        {
            get
            {
                return _model.Cell1Voltage;
            }
            set
            {
                _model.Cell1Voltage = value;
                NotifyOfPropertyChange(() => Cell1Voltage);
            }
        }
        public float Cell2Voltage
        {
            get
            {
                return _model.Cell2Voltage;
            }
            set
            {
                _model.Cell2Voltage = value;
                NotifyOfPropertyChange(() => Cell2Voltage);
            }
        }
        public float Cell3Voltage
        {
            get
            {
                return _model.Cell3Voltage;
            }
            set
            {
                _model.Cell3Voltage = value;
                NotifyOfPropertyChange(() => Cell3Voltage);
            }
        }
        public float Cell4Voltage
        {
            get
            {
                return _model.Cell4Voltage;
            }
            set
            {
                _model.Cell4Voltage = value;
                NotifyOfPropertyChange(() => Cell4Voltage);
            }
        }
        public float Cell5Voltage
        {
            get
            {
                return _model.Cell5Voltage;
            }
            set
            {
                _model.Cell5Voltage = value;
                NotifyOfPropertyChange(() => Cell5Voltage);
            }
        }
        public float Cell6Voltage
        {
            get
            {
                return _model.Cell6Voltage;
            }
            set
            {
                _model.Cell6Voltage = value;
                NotifyOfPropertyChange(() => Cell6Voltage);
            }
        }
        public float Cell7Voltage
        {
            get
            {
                return _model.Cell7Voltage;
            }
            set
            {
                _model.Cell7Voltage = value;
                NotifyOfPropertyChange(() => Cell7Voltage);
            }
        }
        public float Cell8Voltage
        {
            get
            {
                return _model.Cell8Voltage;
            }
            set
            {
                _model.Cell8Voltage = value;
                NotifyOfPropertyChange(() => Cell8Voltage);
            }
        }
        public float TotalPackCurrent
        {
            get
            {
                return _model.TotalPackCurrent;
            }
            set
            {
                _model.TotalPackCurrent = value;
                NotifyOfPropertyChange(() => TotalPackCurrent);
            }
        }
        public float TotalPackVoltage
        {
            get
            {
                return _model.TotalPackVoltage;
            }
            set
            {
                _model.TotalPackVoltage = value;
                NotifyOfPropertyChange(() => TotalPackVoltage);
            }
        }

        public PowerViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
            _model = new PowerModel();

            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("GPSQuality"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor1Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor2Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor3Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor4Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor5Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor6Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor7Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Motor8Current"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Bus5VCurrent"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Bus12VCurrent"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("InputVoltage"));

            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell1Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell2Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell3Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell4Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell5Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell6Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell7Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Cell8Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("TotalPackCurrent"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("TotalPackVoltage"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_cc.MetadataManager.GetTelemetry(dataId).Name)
            {
                case "Motor1Current": Motor1Current = BitConverter.ToSingle(data, 0); break;
                case "Motor2Current": Motor2Current = BitConverter.ToSingle(data, 0); break;
                case "Motor3Current": Motor3Current = BitConverter.ToSingle(data, 0); break;
                case "Motor4Current": Motor4Current = BitConverter.ToSingle(data, 0); break;
                case "Motor5Current": Motor5Current = BitConverter.ToSingle(data, 0); break;
                case "Motor6Current": Motor6Current = BitConverter.ToSingle(data, 0); break;
                case "Motor7Current": Motor7Current = BitConverter.ToSingle(data, 0); break;
                case "Motor8Current": Motor8Current = BitConverter.ToSingle(data, 0); break;
                case "Bus5VCurrent": Bus5VCurrent = BitConverter.ToSingle(data, 0); break;
                case "Bus12VCurrent": Bus12VCurrent = BitConverter.ToSingle(data, 0); break;
                case "InputVoltage": InputVoltage = BitConverter.ToSingle(data, 0); break;

                case "Cell1Voltage": Cell1Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell2Voltage": Cell2Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell3Voltage": Cell3Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell4Voltage": Cell4Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell5Voltage": Cell5Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell6Voltage": Cell6Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell7Voltage": Cell7Voltage = BitConverter.ToSingle(data, 0); break;
                case "Cell8Voltage": Cell8Voltage = BitConverter.ToSingle(data, 0); break;
                case "TotalPackCurrent": TotalPackCurrent = BitConverter.ToSingle(data, 0); break;
                case "TotalPackVoltage": TotalPackVoltage = BitConverter.ToSingle(data, 0); break;

                case "PowerBusOverCurrentNotification":
                    _cc.Console.WriteToConsole("Overcurrent notification from Powerboard from Bus Index " + data[0].ToString());
                    break;
            }
        }

        public void RebootRover()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("BMSReboot"), new byte[0]);
        }
        public void EStopRover()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("BMSStop"), new byte[0]);
        }

        public void EnableBus(byte index)
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PowerBusEnable"), index);
        }
        public void DisableBus(byte index)
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("PowerBusDisable"), index);
        }
    }
}