using RoverAttachmentManager.ViewModels.Science;
﻿using RoverAttachmentManager.ViewModels;
using System;
using System.IO;
using Core.ViewModels.Input.Controllers;
using Core.Configurations;


namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceModel
    {
        internal ScienceGraphViewModel _scienceGraph;
        internal SiteManagmentViewModel _siteManagment;
        internal ScienceGenevaViewModel _scienceGeneva;
        internal ScienceSensorsViewModel _scienceSensors;
        internal SpectrometerViewModel _spectrometer;
        internal SciencePowerViewModel _sciencePower;
        internal InputManagerViewModel InputManager;
        internal XMLConfigManager _configManager;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;
    }
}