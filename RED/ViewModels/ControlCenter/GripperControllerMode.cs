using RED.Interfaces;
using RED.ViewModels.ControlCenter;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class GripperControllerMode : IControllerMode
    {
        private readonly ControlCenterViewModel _controlCenter;

        public string Name { get; set; }
        public InputViewModel InputVM { get; set; }

        public enum GripperAction
        {
            Open = 0,
            Close = 1
        }

        public GripperControllerMode(InputViewModel inputVM, ControlCenterViewModel cc)
        {
            InputVM = inputVM;
            _controlCenter = cc;
            Name = "Gripper";
        }

        public void EnterMode()
        {

        }

        public void EvaluateMode()
        {
            Controller c = InputVM.ControllerOne;
            if (c != null && !c.IsConnected) return;

            if (InputVM.ButtonB)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("AuxGripper"), GripperAction.Close);
            else if (InputVM.ButtonX)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("AuxGripper"), GripperAction.Open);
        }

        public void ExitMode()
        {

        }
    }
}
