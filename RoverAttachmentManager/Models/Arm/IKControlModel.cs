using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RoverAttachmentManager.ViewModels.Arm;

namespace RoverAttachmentManager.Models.Arm
{
    class IKControlModel
    {
        internal ArmViewModel _arm;
        internal float CoordinateX;
        internal float CoordinateY;
        internal float CoordinateZ;
        internal float Yaw;
        internal float Pitch;
        internal float Roll;
        internal float OpX;
        internal float OpY;
        internal float OpZ;

    }
}
