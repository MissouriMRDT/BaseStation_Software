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
    }
}
