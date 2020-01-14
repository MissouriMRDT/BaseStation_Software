using RoverAttachmentManager.ViewModels.Science;
﻿using RoverAttachmentManager.ViewModels;
using System;
using System.IO;
using Core.ViewModels.Input.Controllers;

namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceModel
    {
        internal float Sensor0Value;
        internal float Sensor1Value;
        internal float Sensor2Value;
        internal float Sensor3Value;
        internal float Sensor4Value;
        internal int ScrewPosition;
        internal int RunCount = 100;
        internal int SiteNumber = 1;
        internal SciencePowerViewModel SciencePower;
        internal InputManagerViewModel InputManager;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;
        internal System.Net.IPAddress SpectrometerIPAddress;
        internal ushort SpectrometerPortNumber = 11001;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";

        internal Stream SensorDataFile;
    }
}