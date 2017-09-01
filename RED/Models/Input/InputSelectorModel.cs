using RED.Interfaces.Input;
using RED.ViewModels.Input;
using System.Collections.ObjectModel;

namespace RED.Models.Input
{
    internal class InputSelectorModel
    {
        internal IInputMode Mode;
        internal ObservableCollection<IInputDevice> Devices;
        internal ObservableCollection<MappingViewModel> Mappings;

        internal IInputDevice SelectedDevice;
        internal MappingViewModel SelectedMapping;

        internal bool Enabled;
        internal bool IsRunning;
    }
}
