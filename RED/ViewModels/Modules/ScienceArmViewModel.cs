using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Modules
{
    public class ScienceArmViewModel : PropertyChangedBase, IInputMode
    {
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

        private const int ArmSpeedScale = 1000;
        private const int DrillSpeedScale = 1000;

        public string Name { get; private set; }
        public string ModeType { get; private set; }

        public ScienceArmViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log)
        {
            _router = router;
            _idResolver = idResolver;
            _log = log;

            Name = "Science Arm";
            ModeType = "ScienceArm";
        }

        public void StartMode()
        {

        }

        public void SetValues(Dictionary<string, float> values)
        {
            float armMovement = values["Arm"];
            float drillSpeed = values["Drill"];

            _router.Send(_idResolver.GetId("ScienceArmDrive"), (Int16)(armMovement * ArmSpeedScale));
            _router.Send(_idResolver.GetId("Drill"), (Int16)(drillSpeed * DrillSpeedScale));
        }

        public void StopMode()
        {
            _router.Send(_idResolver.GetId("ScienceArmDrive"), (Int16)(0), true);
            _router.Send(_idResolver.GetId("Drill"), (Int16)(0), true);
        }
    }
}
