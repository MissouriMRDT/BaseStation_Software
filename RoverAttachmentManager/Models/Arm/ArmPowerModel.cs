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

        internal float ArmBaseCurrent1;
        internal float ArmBaseCurrent2;
        internal float ElbowCurrent1;
        internal float ElbowCurrent2;
        internal float WristCurrent1;
        internal float WristCurrent2;
        internal float GripperCurrent;

        internal BitArray Status;
    }
}
