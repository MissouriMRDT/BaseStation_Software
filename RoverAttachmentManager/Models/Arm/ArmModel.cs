
using System.Collections.ObjectModel;
using RoverAttachmentManager.ViewModels.Arm;
using ArmPositionViewModel = RoverAttachmentManager.ViewModels.Arm.ArmViewModel.ArmPositionViewModel;

namespace RoverAttachmentManager.Models.ArmModels
{
    internal class ArmModel
    {
        internal ControlMultipliersViewModel _controlMultipliers;
        internal ControlFeaturesViewModel _controlFeatures;
        internal AngularControlViewModel _angularControl;
        internal float AngleJ1;
        internal float AngleJ2;
        internal float AngleJ3;
        internal float AngleJ4;
        internal float AngleJ5;
        internal float AngleJ6;
        internal int EndeffectorSpeedLimit = 500;
        internal int IKRangeFactor = 1000;
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
        internal ArmPowerViewModel ArmPower;

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
