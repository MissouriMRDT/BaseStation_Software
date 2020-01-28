using RoverAttachmentManager.ViewModels.Science;
﻿using RoverAttachmentManager.ViewModels;
using System;
using System.IO;
using Core.ViewModels.Input.Controllers;
using Core.Configurations;
using RoverAttachmentManager.ViewModels.Science;


namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceModel
    {
        internal ScienceGraphViewModel _scienceGraph;
        internal SiteManagmentViewModel _siteManagment;
        internal ScienceSensorsViewModel _scienceSensors;
        internal ScienceActuationViewModel _scienceActuation;
        internal SpectrometerViewModel _spectrometer;
        
        internal InputManagerViewModel InputManager;
        internal XMLConfigManager _configManager;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;
    }
}