using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RoverAttachmentManager.Models.Arm;
using System;
using System.Collections;
using System.IO;
using System.Net;

namespace RoverAttachmentManager.ViewModels.Arm
{
    public class ArmPowerViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly ArmPowerModel _model; // reads the model for ArmPower, containing all of the variables needed for this function
        private readonly IRovecomm _rovecomm; 
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private TextWriter LogFile;
        
        // logs the values if the current is over or underdrawn
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

        //auto updates the variables (so we can get the current the motor is right now)
        public float BaseTwistCurrent
        {
            get
            {
                return _model.BaseTwistCurrent;
            }
            set
            {
                _model.BaseTwistCurrent = value;
                NotifyOfPropertyChange(() => BaseTwistCurrent);
            }
        }    
        public float BaseTiltCurrent
        {
            get
            {
                return _model.BaseTiltCurrent;
            }
            set
            {
                _model.BaseTiltCurrent = value;
                NotifyOfPropertyChange(() => BaseTiltCurrent);
            }
        }
        public float ElbowTiltCurrent
        {
            get
            {
                return _model.ElbowTiltCurrent;
            }
            set
            {
                _model.ElbowTiltCurrent = value;
                NotifyOfPropertyChange(() => ElbowTiltCurrent);
            }
        }
        public float ElbowTwistCurrent
        {
            get
            {
                return _model.ElbowTwistCurrent;
            }
            set
            {
                _model.ElbowTwistCurrent = value;
                NotifyOfPropertyChange(() => ElbowTwistCurrent);
            }
        }
        public float WristTiltCurrent
        {
            get
            {
                return _model.WristTiltCurrent;
            }
            set
            {
                _model.WristTiltCurrent = value;
                NotifyOfPropertyChange(() => WristTiltCurrent);
            }
        }
        public float WristTwistCurrent
        {
            get
            {
                return _model.WristTwistCurrent;
            }
            set
            {
                _model.WristTwistCurrent = value;
                NotifyOfPropertyChange(() => WristTwistCurrent);
            }
        }
        public float GripperCurrent
        {
            get
            {
                return _model.GripperCurrent;
            }
            set
            {
                _model.GripperCurrent = value;
                NotifyOfPropertyChange(() => GripperCurrent);
            }
        }

        //updates the information within the array so that the table is the most updated it can be
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
        //creating the method of ArmPowerViewModel using the inputs inside the parenthesis
        public ArmPowerViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ArmPowerModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log; 
        }
        

        //sending the packets (in a bit array) to the rover and getting it back  
        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ArmPowerCurrents":
                    BaseTwistCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 6)) / 1000.0);
                    BaseTiltCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 8)) / 1000.0);
                    ElbowTiltCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 10)) / 1000.0);
                    ElbowTwistCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 12)) / 1000.0);
                    WristTiltCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 14)) / 1000.0);
                    WristTwistCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 16)) / 1000.0);
                    GripperCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 18)) / 1000.0);
                    break;

                case "ArmPowerBusStatus":
                    Status = new BitArray(packet.Data);
                    break;


                case "BaseTwistCurrent": BaseTwistCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "BaseTiltCurrent": BaseTiltCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "ElbowTiltCurrent": ElbowTiltCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "ElbowTwistCurrent": ElbowTwistCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "WristTiltCurrent": WristTiltCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "WristTwistCurrent": WristTwistCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                case "GripperCurrent": GripperCurrent = BitConverter.ToSingle(packet.Data, 0); break;
                
                //Overcurrent --> logged 
                case "ArmPowerBusOverCurrentNotification":
                    _log.Log($"Overcurrent notification from ArmPowerboard from Bus Index {packet.Data[0]}");
                    break;
            }
            if (LogFile != null)
            {
                switch (packet.Name)
                {
                    //Overcurrent --> saved in a file.
                    case "ArmPowerBusOverCurrentNotification":
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

        // making the bit array so that the rover can receive the information in the correct amount of bytes/bits 
        public void EnableBus(byte index)
        {

            BitArray bits = new BitArray(16);
            bits.Set(index + 1, true);
            byte[] thebits = new byte[2];

            bits.CopyTo(thebits, 0);

            byte[] bytes = new byte[3];
            thebits.CopyTo(bytes, 0);

            bytes[2] = 1;

            _rovecomm.SendCommand(new Packet("ArmPowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));

        }

        // I don't get this sorry
        public void DisableBus(byte index)
        {

            BitArray bits = new BitArray(16);
            bits.Set(index + 1, true);
            byte[] thebits = new byte[2];

            bits.CopyTo(thebits, 0);

            byte[] bytes = new byte[3];
            thebits.CopyTo(bytes, 0);

            bytes[2] = 0;

            _rovecomm.SendCommand(new Packet("ArmPowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
        }

        //if a motor is enabled or disabled, it goes through here
        public void MotorBusses(bool state)
        {
            BitArray bits = new BitArray(16);
            bits.Set(9, true);
            bits.Set(10, true);
            bits.Set(11, true);

            byte[] bytes = new byte[3];
            bits.CopyTo(bytes, 0);

            bytes[2] = (state) ? (byte)1 : (byte)0;
            _rovecomm.SendCommand(new Packet("ArmPowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
        }

        //saves the data about the ArmPowerData
        public void SaveFile(bool state)

        {
            if (state)
            {
                 LogFile = File.AppendText("REDArmPowerData" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".log");
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
