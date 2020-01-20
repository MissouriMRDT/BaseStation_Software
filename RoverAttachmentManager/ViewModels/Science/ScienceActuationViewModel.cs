using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using RoverAttachmentManager.Models.Science;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RoverAttachmentManager.ViewModels.Science
{
    public class ScienceActuationViewModel : PropertyChangedBase, IRovecommReceiver
    {

        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private const int ScrewSpeedScale = 1000;
        private const int XYSpeedScale = 1000;
        private bool screwIncrementPressed = false;

        private readonly ScienceActuationModel _model;

        public int ScrewPosition
        {
            get
            {
                return _model.ScrewPosition;
            }
            set
            {
                _model.ScrewPosition = value;
                NotifyOfPropertyChange(() => ScrewPosition);
            }
        }

        public ScienceActuationViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceActuationModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            _rovecomm.NotifyWhenMessageReceived(this, "ScrewAtPos");
        }

        public void SetScrewPosition(byte index)
        {
            _rovecomm.SendCommand(new Packet("ScrewAbsoluteSetPosition", index));
        }

        public void CenterX()
        {
            _rovecomm.SendCommand(new Packet("CenterX"));
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ScrewAtPos":
                    ScrewPosition = packet.Data[0];
                    break;
                default:
                    break;
            }
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
    }
}
