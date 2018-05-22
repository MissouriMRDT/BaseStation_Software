using System.Collections.ObjectModel;
using ArmPositionViewModel = RED.ViewModels.Modules.ArmViewModel.ArmPositionViewModel;

namespace RED.Models.Modules
{
    internal class ArmModel
    {
        internal float AngleJ1;
        internal float AngleJ2;
        internal float AngleJ3;
        internal float AngleJ4;
        internal float AngleJ5;
        internal float AngleJ6;
        internal float CurrentMain;
        internal int EndeffectorSpeedLimit = 500;
        internal ObservableCollection<ArmPositionViewModel> Positions = new ObservableCollection<ArmPositionViewModel>();
        internal ArmPositionViewModel SelectedPosition;
        internal float CoordinateX;
        internal float CoordinateY;
        internal float CoordinateZ;
        internal float Yaw;
        internal float Pitch;
        internal float Roll;
        internal string ControlState;
        internal float OpX;
        internal float OpY;
        internal float OpZ;

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
