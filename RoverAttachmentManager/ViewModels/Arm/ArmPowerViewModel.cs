﻿using Caliburn.Micro;
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
        public float ArmBaseCurrent1
        {
            get
            {
                return _model.ArmBaseCurrent1;
            }
            set
            {
                _model.ArmBaseCurrent1 = value;
                NotifyOfPropertyChange(() => ArmBaseCurrent1);
            }
        }    
        public float ArmBaseCurrent2
        {
            get
            {
                return _model.ArmBaseCurrent2;
            }
            set
            {
                _model.ArmBaseCurrent2 = value;
                NotifyOfPropertyChange(() => ArmBaseCurrent2);
            }
        }
        public float ElbowCurrent1
        {
            get
            {
                return _model.ElbowCurrent1;
            }
            set
            {
                _model.ElbowCurrent1 = value;
                NotifyOfPropertyChange(() => ElbowCurrent1);
            }
        }
        public float ElbowCurrent2
        {
            get
            {
                return _model.ElbowCurrent2;
            }
            set
            {
                _model.ElbowCurrent2 = value;
                NotifyOfPropertyChange(() => ElbowCurrent2);
            }
        }
        public float WristCurrent1
        {
            get
            {
                return _model.WristCurrent1;
            }
            set
            {
                _model.WristCurrent1 = value;
                NotifyOfPropertyChange(() => WristCurrent1);
            }
        }
        public float WristCurrent2
        {
            get
            {
                return _model.WristCurrent2;
            }
            set
            {
                _model.WristCurrent2 = value;
                NotifyOfPropertyChange(() => WristCurrent2);
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
                    ArmBaseCurrent1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 6)) / 1000.0);
                    ArmBaseCurrent2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 8)) / 1000.0);
                    ElbowCurrent1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 10)) / 1000.0);
                    ElbowCurrent2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 12)) / 1000.0);
                    WristCurrent1 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 14)) / 1000.0);
                    WristCurrent2 = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 16)) / 1000.0);
                    GripperCurrent = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 18)) / 1000.0);
                    break;

                case "ArmPowerBusStatus":
                    Status = new BitArray(packet.Data);
                    break;


                case "ArmBaseCurrent1": ArmBaseCurrent1 = BitConverter.ToSingle(packet.Data, 0); break;
                case "ArmBaseCurrent2": ArmBaseCurrent2 = BitConverter.ToSingle(packet.Data, 0); break;
                case "ElbowCurrent1": ElbowCurrent1 = BitConverter.ToSingle(packet.Data, 0); break;
                case "ElbowCurrent2": ElbowCurrent2 = BitConverter.ToSingle(packet.Data, 0); break;
                case "WristCurrent1": WristCurrent1 = BitConverter.ToSingle(packet.Data, 0); break;
                case "WristCurrent2": WristCurrent2 = BitConverter.ToSingle(packet.Data, 0); break;
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
        
        //making sure the rover got the message 
        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
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