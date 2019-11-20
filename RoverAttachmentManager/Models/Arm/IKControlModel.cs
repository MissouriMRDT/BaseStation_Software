using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RoverAttachmentManager.ViewModels.Arm;
using ArmPositionViewModel = RoverAttachmentManager.ViewModels.Arm.ArmViewModel.ArmPositionViewModel;

namespace RoverAttachmentManager.Models.Arm
{
    class IKControlModel
    {
        internal float CoordinateX;
        internal float CoordinateY;
        internal float CoordinateZ;
        internal float Yaw;
        internal float Pitch;
        internal float Roll;
        internal int IKRangeFactor = 1000;
        internal float OpX;
        internal float OpY;
        internal float OpZ;
    }
}
