using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RED.Models.Modules;
using System;
using System.Collections;
using System.IO;
using System.Net;

namespace RED.ViewModels.Modules
{
    public class PowerViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly PowerModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private TextWriter LogFile;

        public bool AutoStartLog
        {
            get
            {
                return _model.AutoStartLog;
            }
            set
            {
                _model.AutoStartLog = value;
                if (AutoStartLog && LogFile == null) SaveFile(true);
                NotifyOfPropertyChange(() => AutoStartLog);
            }
        }

        

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
        public float General12V40ACurrent
        {
            get
            {
                return _model.General12V40ACurrent;
            }
            set
            {
                _model.General12V40ACurrent = value;
                NotifyOfPropertyChange(() => General12V40ACurrent);
            }
        }
        public float ActuationCurrent
        {
            get
            {
                return _model.ActuationCurrent;
            }
            set
            {
                _model.ActuationCurrent = value;
                NotifyOfPropertyChange(() => ActuationCurrent);
            }
        }
        public float LogicCurrent
        {
            get
            {
                return _model.LogicCurrent;
            }
            set
            {
                _model.LogicCurrent = value;
                NotifyOfPropertyChange(() => LogicCurrent);
            }
        }
        public float CommunicationsCurrent
        {
            get
            {
                return _model.CommunicationsCurrent;
            }
            set
            {
                _model.CommunicationsCurrent = value;
                NotifyOfPropertyChange(() => CommunicationsCurrent);
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
        public float BMSTemperature1
        {
            get
            {
                return _model.BMSTemperature1;
            }
            set
            {
                _model.BMSTemperature1 = value;
                NotifyOfPropertyChange(() => BMSTemperature1);
            }
        }
        public float BMSTemperature2
        {
            get
            {
                return _model.BMSTemperature2;
            }
            set
            {
                _model.BMSTemperature2 = value;
                NotifyOfPropertyChange(() => BMSTemperature2);
            }
        }

        public BitArray Status
        {
            get
            {
                return _model.Status;
            }
            set
            {
                _model.Status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public PowerViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new PowerModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            _rovecomm.NotifyWhenMessageReceived(this, "Motor1Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor2Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor3Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor4Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor5Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor6Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor7Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Motor8Current");
            _rovecomm.NotifyWhenMessageReceived(this, "Bus5VCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "Bus12VCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "General12V40A");
            _rovecomm.NotifyWhenMessageReceived(this, "ActuationCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "LogicCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "CommunicationsCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "InputVoltage");

            _rovecomm.NotifyWhenMessageReceived(this, "Cell1Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell2Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell3Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell4Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell5Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell6Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell7Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "Cell8Voltage");
            _rovecomm.NotifyWhenMessageReceived(this, "TotalPackCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "TotalPackVoltage");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSTemperature1");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSTemperature2");

            _rovecomm.NotifyWhenMessageReceived(this, "PowerBusOverCurrentNotification");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSPackOvercurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSPackUndervoltage");

            _rovecomm.NotifyWhenMessageReceived(this, "TotalPackCurrentInt");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSTemperatureInt");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSVoltages");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSError");
            _rovecomm.NotifyWhenMessageReceived(this, "PowerCurrents");
            _rovecomm.NotifyWhenMessageReceived(this, "PowerBusStatus");
        }

        public void ReceivedRovecommMessageCallback(ref Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                // New Id's
                case "TotalPackCurrentInt": TotalPackCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 0)) / 1000.0); break;
                case "BMSTemperatureInt": BMSTemperature1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet.Data, 0)) / 1000.0); break;
                case "BMSVoltages":
                    TotalPackVoltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)) / 1000.0);
                    Cell1Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 2)) / 1000.0);
                    Cell2Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 4)) / 1000.0);
                    Cell3Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 6)) / 1000.0);
                    Cell4Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 8)) / 1000.0);
                    Cell5Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 10)) / 1000.0);
                    Cell6Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 12)) / 1000.0);
                    Cell7Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 14)) / 1000.0);
                    Cell8Voltage = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 16)) / 1000.0);
                    break;
                case "BMSError":
                    _log.Log($"Recieved BMSError Report:\n  {BitConverter.ToString(packet.Data)}"); break;

                case "PowerCurrents":
                    ActuationCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)) / 1000.0);
                    LogicCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 2)) / 1000.0);
                    CommunicationsCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 4)) / 1000.0);
                    Motor1Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 6)) / 1000.0);
                    Motor2Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 8)) / 1000.0);
                    Motor3Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 10)) / 1000.0);
                    Motor4Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 12)) / 1000.0);
                    Motor5Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 14)) / 1000.0);
                    Motor6Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 16)) / 1000.0);
                    Motor7Current = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 18)) / 1000.0);
                    General12V40ACurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 20)) / 1000.0);

                    break;

                case "PowerBusStatus":
                    Status = new BitArray(packet.Data);
                    break;

                // Old Id's
                case "Motor1Current": Motor1Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor2Current": Motor2Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor3Current": Motor3Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor4Current": Motor4Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor5Current": Motor5Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor6Current": Motor6Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor7Current": Motor7Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Motor8Current": Motor8Current = BitConverter.ToSingle(packet.Data, 0); break;
                case "Bus5VCurrent": Bus5VCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "Bus12VCurrent": Bus12VCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "General12V40ACurrent": General12V40ACurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "ActuationCurrent": ActuationCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "LogicCurrent": LogicCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "CommunicationsCurrent": CommunicationsCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "InputVoltage": InputVoltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell1Voltage": Cell1Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell2Voltage": Cell2Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell3Voltage": Cell3Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell4Voltage": Cell4Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell5Voltage": Cell5Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell6Voltage": Cell6Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell7Voltage": Cell7Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "Cell8Voltage": Cell8Voltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "TotalPackCurrent": TotalPackCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "TotalPackVoltage": TotalPackVoltage = BitConverter.ToSingle(packet.Data, 0); break;
                case "BMSTemperature1": BMSTemperature1 = BitConverter.ToSingle(packet.Data, 0); break;
                case "BMSTemperature2": BMSTemperature2 = BitConverter.ToSingle(packet.Data, 0); break;

                case "PowerBusOverCurrentNotification":
                    _log.Log($"Overcurrent notification from Powerboard from Bus Index {packet.Data[0]}");
                    break;
                case "BMSPackOvercurrent":
                    _log.Log("Overcurrent notification from BMS");
                    break;
                case "BMSPackUndervoltage":
                    _log.Log("Undervoltage notification from BMS");
                    break;
                
            }

            if (LogFile != null)
            {
                switch (packet.Name)
                {
                    case "PowerBusOverCurrentNotification":
                        LogFile.WriteLine("{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}, {1}, Power Overcurrent: Bus {2}", DateTime.Now, packet.Name, packet.Data[0]);
                        break;
                    case "BMSPackOvercurrent":
                        LogFile.WriteLine("{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}, {1}, BMS Overcurrent", DateTime.Now, packet.Name);
                        break;
                    case "BMSPackUndervoltage":
                        LogFile.WriteLine("{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}, {1}, BMS Undervoltage", DateTime.Now, packet.Name);
                        break;
                    default:
                        if (packet.Data.Length != 4) break;
                        LogFile.WriteLine("{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}, {1}, {2}", DateTime.Now, packet.Name, BitConverter.ToSingle(packet.Data, 0));
                        break;
                }
                LogFile.Flush();
            }
		}

		public void RebootRover()
        {
            _rovecomm.SendCommand(new Packet("BMSStop", (byte)10), true);
        }
        public void EStopRover()
        {
            _rovecomm.SendCommand(new Packet("BMSStop", (byte)0), true);
        }

        public void FanControl(bool state)
        {
            _rovecomm.SendCommand(new Packet("BMSFanControl", state ? 0 : 1), true);
        }
        public void BuzzerControl(bool state)
        {
            _rovecomm.SendCommand(new Packet("BMSBuzzerControl", state ? 0 : 1), true);
        }

        public void EnableBus(byte index)
        {
            
            BitArray bits = new BitArray(16);
            bits.Set(index + 1, true);
            byte[] thebits = new byte[2];

            bits.CopyTo(thebits, 0);

            byte[] bytes = new byte[3];
            thebits.CopyTo(bytes, 0);

            bytes[2] = 1;

            _rovecomm.SendCommand(new Packet("PowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));

        }
        public void DisableBus(byte index)
        {

            BitArray bits = new BitArray(16);
            bits.Set(index + 1, true);
            byte[] thebits = new byte[2];

            bits.CopyTo(thebits, 0);

            byte[] bytes = new byte[3];
            thebits.CopyTo(bytes, 0);

            bytes[2] = 0;

            _rovecomm.SendCommand(new Packet("PowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
        }

        public void MotorBusses(bool state)
        {
            BitArray bits = new BitArray(16);
            bits.Set(9, true);
            bits.Set(10, true);
            bits.Set(11, true);

            byte[] bytes = new byte[3];
            bits.CopyTo(bytes, 0);

            bytes[2] = (state) ? (byte)1 : (byte)0;
            _rovecomm.SendCommand(new Packet("PowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
        }

        public void SaveFile(bool state)
        {
            if (state)
            {
                LogFile = File.AppendText("REDPowerData" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".log");
            }
            else
            {
                if (LogFile != null)
                {
                    LogFile.Close();
                    LogFile = null;
                }
            }
        }
    }
}