using Core.Interfaces;
using Core.Interfaces.Input;
using Core.ViewModels.Input;
using System.Collections.ObjectModel;

namespace Core.Models.Input
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
