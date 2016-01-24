using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface IInputDevice
    {
        bool ModeNext { get; set; }
        bool ModePrev { get; set; }
        bool DrillCounterClockwise { get; set; }
        bool DrillClockwise { get; set; }
        bool BaseCounterClockwise { get; set; }
        bool BaseClockwise { get; set; }
        bool ActuatorForward { get; set; }
        bool ActuatorBackward { get; set; }
        bool ArmReset { get; set; }
        bool DebouncedArmReset { get; set; }
        bool ToolNext { get; set; }
        bool DebouncedToolNext { get; set; }
        bool ToolPrev { get; set; }
        bool DebouncedToolPrev { get; set; }
        float WheelsLeft { get; set; }
        float WheelsRight { get; set; }
        float WristBend { get; set; }
        float WristTwist { get; set; }
        float ElbowBend { get; set; }
        float ElbowTwist { get; set; }
        float GripperOpen { get; set; }
        float GripperClose { get; set; }

        void Start();
        int SerialReadSpeed { get; set; }
        bool AutoDeadzone { get; set; }
        int ManualDeadzone { get; set; }
        ObservableCollection<IControllerMode> ControllerModes { get; }
    }
}
