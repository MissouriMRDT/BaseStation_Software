using RED.ViewModels.Input;
using System.Collections.ObjectModel;

namespace RED.Models.Input
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
