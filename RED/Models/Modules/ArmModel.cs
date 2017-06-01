using RED.Contexts;
using RED.ViewModels.Input;
using System.Collections.ObjectModel;
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
        internal ObservableCollection<ArmPositionContext> Positions = new ObservableCollection<ArmPositionContext>();
        internal ArmPositionContext SelectedPosition;
    }
}
