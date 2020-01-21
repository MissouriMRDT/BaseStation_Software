using RoverAttachmentManager.ViewModels.Arm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RoverAttachmentManager.ViewModels.Arm.ArmViewModel;

namespace RoverAttachmentManager.Models.Arm
{
    internal class AngularControlModel
    {
        internal float AngleJ1;
        internal float AngleJ2;
        internal float AngleJ3;
        internal float AngleJ4;
        internal float AngleJ5;
        internal float AngleJ6;
        internal ObservableCollection<ArmPositionViewModel> Positions = new ObservableCollection<ArmPositionViewModel>();
        internal ArmPositionViewModel SelectedPosition;
        internal ArmViewModel Arm;

    }
}
