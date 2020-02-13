using Caliburn.Micro;
using Core.Configurations;
using Core.Interfaces;
using Core.Interfaces.Input;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
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
    public class ScienceViewModel : PropertyChangedBase, IInputMode
    {

        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public string Name { get; }
        public string ModeType { get; }
        private const int ScrewSpeedScale = 1000;
        private const int XYSpeedScale = 1000;
        private bool screwIncrementPressed = false;
        private byte ChemOne = 0;
        private byte ChemTwo = 0;
        private byte ChemThree = 0;

        private readonly ScienceModel _model;   
 
        public ScienceGraphViewModel ScienceGraph
        {
            get
            {
                return _model._scienceGraph;
            }
            set
            {
                _model._scienceGraph = value;
                NotifyOfPropertyChange(() => ScienceGraph);
            }
        }
        public SiteManagmentViewModel SiteManagment
        {
            get
            {
                return _model._siteManagment;
            }
            set
            {
                _model._siteManagment = value;
                NotifyOfPropertyChange(() => SiteManagment);
            }
        }
        public ScienceGenevaViewModel ScienceGeneva
        {
            get
            {
                return _model._scienceGeneva;
            }
            set
            {
                _model._scienceGeneva = value;
                NotifyOfPropertyChange(() => ScienceGeneva);
            }
        }
        public ScienceActuationViewModel ScienceActuation
        {
            get
            {
                return _model._scienceActuation;
            }
            set
            {
                _model._scienceActuation = value;
                NotifyOfPropertyChange(() => ScienceActuation);
            }
        }
        public SpectrometerViewModel Spectrometer
        {
            get
            {
                return _model._spectrometer;
            }
            set
            {
                _model._spectrometer = value;
                NotifyOfPropertyChange(() => Spectrometer);
            }
        }
        public ScienceSensorsViewModel ScienceSensors
        {
            get
            {
                return _model._scienceSensors;
            }
            set
            {
                _model._scienceSensors = value;
                NotifyOfPropertyChange(() => ScienceSensors);
            }
        }
        public SciencePowerViewModel SciencePower
        {
            get
            {
                return _model._sciencePower;
            }
            set
            {
                _model._sciencePower = value;
                NotifyOfPropertyChange(() => SciencePower);
            }
        }
        public InputManagerViewModel InputManager
        {
            get
            {
                return _model.InputManager;
            }
            set
            {
                _model.InputManager = value;
                NotifyOfPropertyChange(() => InputManager);
            }
        }
        public XMLConfigManager ConfigManager
        {
            get
            {
                return _model._configManager;
            }
            set
            {
                _model._configManager = value;
                NotifyOfPropertyChange();
            }
        }
        public XboxControllerInputViewModel XboxController1
        {
            get
            {
                return _model._xboxController1;
            }
            set
            {
                _model._xboxController1 = value;
                NotifyOfPropertyChange(() => XboxController1);
            }
        }
        public XboxControllerInputViewModel XboxController2
        {
            get
            {
                return _model._xboxController2;
            }
            set
            {
                _model._xboxController2 = value;
                NotifyOfPropertyChange(() => XboxController2);
            }
        }
        public XboxControllerInputViewModel XboxController3
        {
            get
            {
                return _model._xboxController3;
            }
            set
            {
                _model._xboxController3 = value;
                NotifyOfPropertyChange(() => XboxController3);
            }
        }

        public ScienceViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceModel();
            SiteManagment = new SiteManagmentViewModel(networkMessenger, idResolver, log, this);
            ScienceGeneva = new ScienceGenevaViewModel(networkMessenger, idResolver, log);
            ScienceGraph = new ScienceGraphViewModel(networkMessenger, idResolver, log);
            ScienceActuation = new ScienceActuationViewModel(networkMessenger, idResolver, log);
            Spectrometer = new SpectrometerViewModel(networkMessenger, idResolver, log, this);
            ScienceSensors = new ScienceSensorsViewModel(networkMessenger, idResolver, log, this);
            SciencePower = new SciencePowerViewModel(networkMessenger, idResolver, log);

            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            ConfigManager = new XMLConfigManager(log);

            XboxController1 = new XboxControllerInputViewModel(1);
            XboxController2 = new XboxControllerInputViewModel(2);
            XboxController3 = new XboxControllerInputViewModel(3);

            // Programatic instanciation of InputManager view, vs static like everything else in a xaml 
            InputManager = new InputManagerViewModel(log, ConfigManager,
                new IInputDevice[] { XboxController1, XboxController2, XboxController3 },
                new MappingViewModel[0],
                new IInputMode[] { this });

            Name = "Science/Controls";

            Name = Name.Replace("/", System.Environment.NewLine);

            ModeType = "ScienceControls";
        }

        public void StartMode() { }
        public void SetValues(Dictionary<string, float> values)
        {
            /* needs to be updated from screw pos to Zactuation
            if ((values["ScrewPosUp"] == 1 || values["ScrewPosDown"] == 1) && !screwIncrementPressed)
            {
                byte screwPosIncrement = (byte)(values["ScrewPosUp"] == 1 ? 1 : values["ScrewPosDown"] == 1 ? -1 : 0);
                _rovecomm.SendCommand(Packet.Create("ScrewRelativeSetPosition", screwPosIncrement));
                screwIncrementPressed = true;
            }
            else if (values["ScrewPosUp"] == 0 && values["ScrewPosDown"] == 0)
            {
                screwIncrementPressed = false;
            }
            
            Int16[] screwValue = { (Int16)(values["Screw"] * ScrewSpeedScale) };
            _rovecomm.SendCommand(Packet.Create("Screw", screwValue));
            */

            
            if (values["VacuumPulse"] == 1)
            {
                _rovecomm.SendCommand(Packet.Create("Vacuum", (byte)1));
            }else if(values["ValuePulse"] == 0)
            {
                _rovecomm.SendCommand(Packet.Create("Vacuum", (byte)0));
            }

            if (values["Chem1"] == 1)
            {
                ChemOne = 1;
            }else if(values["Chem1"] == 0)
            {
                ChemOne = 0;
            }

            if (values["Chem2"] == 1)
            {
                ChemTwo = 1;
            }
            else if (values["Chem2"] == 0)
            {
                ChemTwo = 0;
            }

            if (values["Chem3"] == 1)
            {
                ChemThree = 1;
            }
            else if (values["Chem3"] == 0)
            {
                ChemThree = 0;
            }
            byte[] chemicals = { ChemOne, ChemTwo, ChemThree };
            _rovecomm.SendCommand(Packet.Create("Chemicals", chemicals));
        }

        public void StopMode()
        {
            //_rovecomm.SendCommand(Packet.Create("Screw", (Int16)(0)), true);
        }

    }
}
