using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.Models.Arm
{
    internal class ArmPowerModel
    {
        internal bool AutoStartLog;

        internal float BaseTwistCurrent;
        internal float BaseTiltCurrent;
        internal float ElbowTiltCurrent;
        internal float ElbowTwistCurrent;
        internal float WristTiltCurrent;
        internal float WristTwistCurrent;
        internal float GripperCurrent;

        internal BitArray Status;
    }
}
