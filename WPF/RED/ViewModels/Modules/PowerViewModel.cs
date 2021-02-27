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
        
        public byte RebootTime
        {
            get
            {
                return _model.RebootTime;
            }
            set
            {
                _model.RebootTime = value;
                NotifyOfPropertyChange(() => RebootTime);
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
        public float TwelveVoltCurrent 
        {
            get
            {
                return _model.TwelveVoltCurrent;
            }
            set
            {
                _model.TwelveVoltCurrent = value;
                NotifyOfPropertyChange(() => TwelveVoltCurrent);
            }
        }
        public float AuxiliaryCurrent
        {
            get
            {
                return _model.AuxiliaryCurrent;
            }
            set
            {
                _model.AuxiliaryCurrent = value;
                NotifyOfPropertyChange(() => AuxiliaryCurrent);
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
        public BitArray MotorBusStatus
        {
            get
            {
                return _model.MotorBusStatus;
            }
            set
            {
                _model.MotorBusStatus = value;
                NotifyOfPropertyChange(() => MotorBusStatus);
            }
        }

        public PowerViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new PowerModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            //2019 BMS
            _rovecomm.NotifyWhenMessageReceived(this, "TotalPackCurrentInt");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSTemperatureInt");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSVoltages");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSError");

            /* 2020 BMS support
            //From BMS board
            _rovecomm.NotifyWhenMessageReceived(this, "TotalPackCurrentInt");
            _rovecomm.NotifyWhenMessageReceived(this, "TotalPackVoltageInt");
            _rovecomm.NotifyWhenMessageReceived(this, "CellCurrentInts");
            _rovecomm.NotifyWhenMessageReceived(this, "BMSTemperatureInt");
            */
            //From Power board
            _rovecomm.NotifyWhenMessageReceived(this, "MotorBusEnabled");
            _rovecomm.NotifyWhenMessageReceived(this, "12VEnabled");
            _rovecomm.NotifyWhenMessageReceived(this, "30VEnabled");
            _rovecomm.NotifyWhenMessageReceived(this, "VacuumEnabled");
            _rovecomm.NotifyWhenMessageReceived(this, "PatchPanelEnabled");
            _rovecomm.NotifyWhenMessageReceived(this, "MotorBusCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "12VBusCurrent");
            _rovecomm.NotifyWhenMessageReceived(this, "30VBusCurrent");
            
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                //2019 BMS

                case "BMSVoltages":
                    Int16[] cellVoltages = packet.GetDataArray<Int16>();
                    TotalPackVoltage = (float)(cellVoltages[0] / 1000.0);
                    Cell1Voltage = (float)(cellVoltages[1] / 1000.0);
                    Cell2Voltage = (float)(cellVoltages[2] / 1000.0);
                    Cell3Voltage = (float)(cellVoltages[3] / 1000.0);
                    Cell4Voltage = (float)(cellVoltages[4] / 1000.0);
                    Cell5Voltage = (float)(cellVoltages[5] / 1000.0);
                    Cell6Voltage = (float)(cellVoltages[6] / 1000.0);
                    Cell7Voltage = (float)(cellVoltages[7] / 1000.0);
                    Cell8Voltage = (float)(cellVoltages[8] / 1000.0);
                    break;

                case "TotalPackCurrentInt":
                    TotalPackCurrent = (float)(packet.GetData<Int32>() / 1000.0);
                    break;

                case "BMSTemperatureInt":
                    BMSTemperature1 = (float)(packet.GetData<Int32>() / 1000.0);
                    break;

                case "BMSError":
                    _log.Log($"Recieved BMSError Report:\n  {BitConverter.ToString(packet.Data)}");
                    break;

                /*
                // 2020 Id's
                case "TotalPackCurrentInt":
                    TotalPackCurrent = (float)(packet.GetData<Int32>() / 1000.0);
                    break;

                case "BMSTemperatureInt":
                    BMSTemperature1 = (float)(packet.GetData<Int32>() / 1000.0);
                    break;

                case "CellCurrentInts":
                    float[] cellVoltages = packet.GetDataArray<float>();
                    Cell1Voltage = (float)(cellVoltages[0]);
                    Cell2Voltage = (float)(cellVoltages[1]);
                    Cell3Voltage = (float)(cellVoltages[2]);
                    Cell4Voltage = (float)(cellVoltages[3]);
                    Cell5Voltage = (float)(cellVoltages[4]);
                    Cell6Voltage = (float)(cellVoltages[5]);
                    Cell7Voltage = (float)(cellVoltages[6]);
                    Cell8Voltage = (float)(cellVoltages[7]);
                    break;

                case "TotalPackVoltageInt":
                    TotalPackVoltage = (float)(packet.GetData<Int32>() / 1000.0);
                    break;
                */

                case "PowerCurrents":
                    float[] currents = packet.GetDataArray<float>();
                    ActuationCurrent = (float)(currents[0]);
                    LogicCurrent = (float)(currents[1]);
                    CommunicationsCurrent = (float)(currents[2] / 1000.0);
                    Motor1Current = (float)(currents[3] / 1000.0);
                    Motor2Current = (float)(currents[4] / 1000.0);
                    Motor3Current = (float)(currents[5] / 1000.0);
                    Motor4Current = (float)(currents[6] / 1000.0);
                    Motor5Current = (float)(currents[7] / 1000.0);
                    Motor6Current = (float)(currents[8] / 1000.0);
                    AuxiliaryCurrent = (float)(currents[9] / 1000.0);
                    TwelveVoltCurrent = (float)(currents[10] / 1000.0);

                    break;

                case "PowerBusStatus":
                    MotorBusStatus = new BitArray(packet.Data);
                    break;

                case "MotorBusEnabled":
                    MotorBusStatus = new BitArray(packet.GetData<Byte>());
                    break;

                default:
                    break;
            }

            
            /* // TODO: Rewrite this section to match current packet style
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
            */
        }

        public void RebootRover()
        {
            //2020 BMS estop support
            //_rovecomm.SendCommand(Packet.Create("SoftwareEStop", (byte)RebootTime), true);

            //2019 BMS estop
            _rovecomm.SendCommand(Packet.Create("BMSStop", (byte)RebootTime), true);
        }
        public void EStopRover()
        {
            _rovecomm.SendCommand(Packet.Create("BMSStop", (byte)0), true);
        }

        public void EnableBus(byte index) //not updated
        {
            BitArray bits = new BitArray(32);
            bits.Set(index + 1, true);
            byte[] thebits = new byte[4];

            bits.CopyTo(thebits, 0);

            byte[] bytes = new byte[5];
            thebits.CopyTo(bytes, 1);

            bytes[0] = 1;

            _rovecomm.SendCommand(Packet.Create("PowerBusEnableDisable", bytes));
        }

        public void DisableBus(byte index) //not updated
        {
            BitArray bits = new BitArray(16);
            bits.Set(index + 1, true);
            byte[] thebits = new byte[2];

            bits.CopyTo(thebits, 0);

            byte[] bytes = new byte[3];
            thebits.CopyTo(bytes, 0);

            bytes[2] = 0;

            _rovecomm.SendCommand(Packet.Create("PowerBusEnableDisable", bytes));
        }

        public void MotorBusses(bool state) //not updated
        {
            BitArray bits = new BitArray(16);
            bits.Set(9, true);
            bits.Set(10, true);
            bits.Set(11, true);

            byte[] bytes = new byte[3];
            bits.CopyTo(bytes, 0);

            bytes[2] = (state) ? (byte)1 : (byte)0;
            _rovecomm.SendCommand(Packet.Create("PowerBusEnableDisable", bytes));
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