using RED.ViewModels.Input;
using System.Collections.ObjectModel;

namespace RED.Models.Input
{
    internal class MappingModel
    {
        internal ObservableCollection<MappingChannelViewModel> Channels;
        internal string DeviceType;
        internal string ModeType;
        internal uint UpdatePeriod;
        internal bool IsActive;
    }
}
