using Core.ViewModels.Input;
using System.Collections.ObjectModel;

namespace Core.Models.Input
{
    internal class MappingModel
    {
        internal string Name;
        internal ObservableCollection<MappingChannelViewModel> Channels;
        internal string DeviceType;
        internal string ModeType;
        internal int UpdatePeriod;
    }
}
