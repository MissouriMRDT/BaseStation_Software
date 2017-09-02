using RED.Interfaces.Input;
using RED.ViewModels.Input;
using System.Collections.ObjectModel;

namespace RED.Models.Input
{
    internal class InputManagerModel
    {
        internal ObservableCollection<IInputDevice> Devices;
        internal ObservableCollection<MappingViewModel> Mappings;
        internal ObservableCollection<IInputMode> Modes;
        internal ObservableCollection<InputSelectorViewModel> Selectors;
    }
}
