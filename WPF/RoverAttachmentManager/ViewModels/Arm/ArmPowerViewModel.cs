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
            _rovecomm.NotifyWhenMessageReceived(this, "ArmCurrents");
        }


        //sending the packets (in a bit array) to the rover and getting it back  
        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ArmCurrents":
                    float[] armCurrents = packet.GetDataArray<float>();
                    BaseTwistCurrent = (float)(armCurrents[0]);
                    BaseTiltCurrent = (float)(armCurrents[1]);
                    ElbowTiltCurrent = (float)(armCurrents[2]);
                    ElbowTwistCurrent = (float)(armCurrents[3]);
                    WristTiltCurrent = (float)(armCurrents[4]);
                    WristTwistCurrent = (float)(armCurrents[5]);
                    GripperCurrent = (float)(armCurrents[6]);
                    break;

                /*New error system not implemented
                //Overcurrent --> logged 
                case "ArmPowerBusOverCurrentNotification":
                    _log.Log($"Overcurrent notification from ArmPowerboard from Bus Index {packet.Data[0]}");
                    break;
                */

                default:
                    break;
            }
            /* new error system not implemented
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
            */
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

            //_rovecomm.SendCommand(new Packet("ArmPowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));

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

            //_rovecomm.SendCommand(new Packet("ArmPowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
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
            //_rovecomm.SendCommand(new Packet("ArmPowerBusEnableDisable", bytes, 3, (byte)DataTypes.UINT8_T));
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
