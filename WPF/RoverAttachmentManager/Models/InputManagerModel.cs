using Core.Interfaces;
using Core.Interfaces.Input;
using Core.ViewModels.Input;
using RoverAttachmentManager.ViewModels;
using System.Collections.ObjectModel;

namespace RoverAttachmentManager.Models
{
    internal class InputManagerModel
    {
        internal ObservableCollection<IInputDevice> Devices;
        internal ObservableCollection<MappingViewModel> Mappings;
        internal ObservableCollection<IInputMode> Modes;
        internal ObservableCollection<InputSelectorViewModel> Selectors;
    }
}
