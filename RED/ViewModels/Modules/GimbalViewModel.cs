using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using RED.ViewModels.Input;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class GimbalViewModel : PropertyChangedBase, IInputMode
    {
        private const float MaxZoomSpeed = 1000;
        private readonly GimbalModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public string Name { get; }
        public string ModeType { get; }
        
        public int PanIncrement
        {
            get
            {
                return _model.PanIncrement;
            }
            set
            {
                _model.PanIncrement = value;
                NotifyOfPropertyChange(() => PanIncrement);
            }
        }

        public int RollIncrement
        {
            get
            {
                return _model.RollIncrement;
            }
            set
            {
                _model.RollIncrement = value;
                NotifyOfPropertyChange(() => RollIncrement);
            }
        }

        public int TiltIncrement
        {
            get
            {
                return _model.TiltIncrement;
            }
            set
            {
                _model.TiltIncrement = value;
                NotifyOfPropertyChange(() => TiltIncrement);
            }
        }

        public GimbalViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new GimbalModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            Name = "Main Gimbal";
            ModeType = "Gimbal";
        }

        public void StartMode()
        { }

        public void SetValues(Dictionary<string, float> values)
        {
            short pan, tilt;
            pan = (Int16)(values["Pan"] * PanIncrement);
            tilt = (Int16)(values["Tilt"] * TiltIncrement);

            short[] openVals = { pan, tilt, 0, 0, 0 };
            byte[] data = new byte[openVals.Length * sizeof(Int16)];
            Buffer.BlockCopy(openVals, 0, data, 0, data.Length);
            _rovecomm.SendCommand(_idResolver.GetId("GimbalOpenValues"), data);
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalOpenValues"), new byte[]{ 0, 0, 0, 0, 0 }, true);
        }

        public void Snapshot()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalRecord"), (byte)GimbalRecordValues.Snapshot, true);
        }

        public void RecordStart()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalRecord"), (byte)GimbalRecordValues.Start, true);
        }

        public void RecordStop()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalRecord"), (byte)GimbalRecordValues.Stop, true);
        }

        private enum GimbalRecordValues
        {
            Stop = 0,
            Start = 1,
            Snapshot = 2
        }
    }
}