using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class ScienceArmViewModel : PropertyChangedBase, IInputMode
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private const int ScrewSpeedScale = 1000;
        private const int DrillSpeedScale = 1000;

        public string Name { get; }
        public string ModeType { get; }

        public ScienceArmViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            Name = "Science Arm";
            ModeType = "ScienceArm";
        }

        public void StartMode()
        { }

        public void SetValues(Dictionary<string, float> values)
        {
            float screwMovement = values["Screw"];
            float drillSpeed = values["Drill"];

            _rovecomm.SendCommand(_idResolver.GetId("Screw"), (Int16)(screwMovement * ScrewSpeedScale));
            _rovecomm.SendCommand(_idResolver.GetId("Drill"), (Int16)(drillSpeed * DrillSpeedScale));
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("Screw"), (Int16)(0), true);
            _rovecomm.SendCommand(_idResolver.GetId("Drill"), (Int16)(0), true);
        }
    }
}
