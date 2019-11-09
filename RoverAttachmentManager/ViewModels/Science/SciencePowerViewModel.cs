using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RoverAttachmentManager.Models.Science;
using System;
using System.Collections;
using System.IO;
using System.Net;

namespace RoverAttachmentManager.ViewModels.Science
{
    public class SciencePowerViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly SciencePowerModel _model;
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
        public float WOActuationCurrent1
        {
            get
            {
                return _model.WOActuationCurrent1;
            }
            set
            {
                _model.WOActuationCurrent1 = value;
                NotifyOfPropertyChange(() => WOActuationCurrent1);
            }
        }
        public float WOActuationCurrent2
        {
            get
            {
                return _model.WOActiationCurrent2;
            }
            set
            {
                _model.WOActiationCurrent2 = value;
                NotifyOfPropertyChange(() => WOActuationCurrent2);
            }
        }
        public float GenevaCurrent
        {
            get
            {
                return _model.GenevaCurrent;
            }
            set
            {
                _model.GenevaCurrent = value;
                NotifyOfPropertyChange(() => GenevaCurrent);
            }
        }
        public float VacuumCurrent
        {
            get
            {
                return _model.VacuumCurrent;
            }
            set
            {
                _model.VacuumCurrent = value;
                NotifyOfPropertyChange(() => VacuumCurrent);
            }
        }
        public float FluidPumpCurrent1
        {
            get
            {
                return _model.FluidPumpCurrent1;
            }
            set
            {
                _model.FluidPumpCurrent1 = value;
                NotifyOfPropertyChange(() => FluidPumpCurrent1);
            }
        }
        public float FluidPumpCurrent2
        {
            get
            {
                return _model.FluidPumpCurrent2;
            }
            set
            {
                _model.FluidPumpCurrent2 = value;
                NotifyOfPropertyChange(() => FluidPumpCurrent2);
            }
        }
        public float FluidPumpCurrent3
        {
            get
            {
                return _model.FluidPumpCurrent3;
            }
            set
            {
                _model.FluidPumpCurrent3 = value;
                NotifyOfPropertyChange(() => FluidPumpCurrent3);
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
        public SciencePowerViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new SciencePowerModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;


        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "SciencePowerCurrents":
                    WOActuationCurrent1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 6)) / 1000.0);
                    WOActuationCurrent2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 8)) / 1000.0);
                    GenevaCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 10)) / 1000.0);
                    VacuumCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 12)) / 1000.0);
                    FluidPumpCurrent1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 14)) / 1000.0);
                    FluidPumpCurrent2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 16)) / 1000.0);
                    FluidPumpCurrent3 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 18)) / 1000.0);
                    break;

                case "SciencePowerBusStatus":
                    Status = new BitArray(packet.Data);
                    break;


                case "BaseTwistCurrent": WOActuationCurrent1 = BitConverter.ToSingle(packet.Data, 0); break;
                case "BaseTiltCurrent": WOActuationCurrent2 = BitConverter.ToSingle(packet.Data, 0); break;
                case "ElbowTiltCurrent": GenevaCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "ElbowTwistCurrent": VacuumCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "WristTiltCurrent": FluidPumpCurrent1 = BitConverter.ToSingle(packet.Data, 0); break;
                case "WristTwistCurrent": FluidPumpCurrent2 = BitConverter.ToSingle(packet.Data, 0); break;
                case "GripperCurrent": FluidPumpCurrent3 = BitConverter.ToSingle(packet.Data, 0); break;

                case "SciencePowerBusOverCurrentNotification":
                    _log.Log($"Overcurrent notification from SciencePowerboard from Bus Index {packet.Data[0]}");
                    break;
            }
            if (LogFile != null)
            {
                switch (packet.Name)
                {
                    case "SciencePowerBusOverCurrentNotification":
                        LogFile.WriteLine("{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}, {1}, Power Overcurrent: Bus {2}", DateTime.Now, packet.Name, packet.Data[0]);
                        break;
                    default:
                        if (packet.Data.Length != 4) break;
                        LogFile.WriteLine("{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}, {1}, {2}", DateTime.Now, packet.Name, BitConverter.ToSingle(packet.Data, 0));
                        break;
                }
                LogFile.Flush();
            }
        }
        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
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

            _rovecomm.SendCommand(new Packet("SciencePowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));

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

            _rovecomm.SendCommand(new Packet("SciencePowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
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
            _rovecomm.SendCommand(new Packet("SciencePowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
        }

        public void SaveFile(bool state)

        {
            if (state)
            {
                LogFile = File.AppendText("REDSciencePowerData" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".log");
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
