using RoverAttachmentManager.ViewModels.Arm;
using System.Collections.ObjectModel;
using static RoverAttachmentManager.ViewModels.Arm.AngularControlViewModel;

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

        internal class ArmPositionModel
        {
            internal string Name;
            internal float J1;
            internal float J2;
            internal float J3;
            internal float J4;
            internal float J5;
            internal float J6;
        }
    }
}
