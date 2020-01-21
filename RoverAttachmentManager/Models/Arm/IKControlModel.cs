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
        internal ArmViewModel _arm;
        internal ControlMultipliersViewModel _controlMultipliers;
        internal float AngleJ1;
        internal float AngleJ2;
        internal float AngleJ3;
        internal float AngleJ4;
        internal float AngleJ5;
        internal float AngleJ6;
        internal float CoordinateX;
        internal float CoordinateY;
        internal float CoordinateZ;
        internal float Yaw;
        internal float Pitch;
        internal float Roll;
        internal int IKRangeFactor = 1000;
        internal ObservableCollection<ArmPositionViewModel> Positions = new ObservableCollection<ArmPositionViewModel>();
        internal float OpX;
        internal float OpY;
        internal float OpZ;
        internal byte SelectedTool;

    }
}
