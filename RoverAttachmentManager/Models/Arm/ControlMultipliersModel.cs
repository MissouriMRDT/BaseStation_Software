using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.Models.Arm
{
    class ControlMultipliersModel
    {
        internal int BaseRangeFactor = 500;
        internal int ElbowRangeFactor = 500;
        internal int WristRangeFactor = 1000;
        internal int GripperRangeFactor = 1000;
        internal int Gripper2RangeFactor = 1000;
    }
}
