using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Interfaces.Network;
using System;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class ScienceArmViewModel : PropertyChangedBase, IInputMode
    {
        private readonly INetworkMessenger _networkMessenger;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private const int ArmSpeedScale = 1000;
        private const int DrillSpeedScale = 1000;

        public string Name { get; }
        public string ModeType { get; }

        public ScienceArmViewModel(INetworkMessenger networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _networkMessenger = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            Name = "Science Arm";
            ModeType = "ScienceArm";
        }

        public void StartMode()
        { }

        public void SetValues(Dictionary<string, float> values)
        {
            float armMovement = values["Arm"];
            float drillSpeed = values["Drill"];

            _networkMessenger.SendOverNetwork(_idResolver.GetId("ScienceArmDrive"), (Int16)(armMovement * ArmSpeedScale));
            _networkMessenger.SendOverNetwork(_idResolver.GetId("Drill"), (Int16)(drillSpeed * DrillSpeedScale));
        }

        public void StopMode()
        {
            _networkMessenger.SendOverNetwork(_idResolver.GetId("ScienceArmDrive"), (Int16)(0), true);
            _networkMessenger.SendOverNetwork(_idResolver.GetId("Drill"), (Int16)(0), true);
        }
    }
}
