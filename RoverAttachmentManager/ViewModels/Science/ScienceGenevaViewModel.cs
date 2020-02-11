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
    public class ScienceGenevaViewModel : PropertyChangedBase
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private readonly ScienceGenevaModel _model;

        public int TestTube
        {
            get
            {
                return _model.TestTube;
            }
            set
            {
                _model.TestTube = value;
                NotifyOfPropertyChange(() => TestTube);
            }
        }

        public ScienceGenevaViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceGenevaModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;


        }

        public void RotateLeft()
        {

            TestTube--;
            if (TestTube == 0)
            {
                TestTube = 8;
            }
        }

        public void RotateRight()
        {
            TestTube++;
            if (TestTube == 9)
            {
                TestTube = 1;
            }
        }
    }
}
