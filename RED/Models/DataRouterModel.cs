using RED.Interfaces;
using System.Collections.Generic;

namespace RED.Models
{
    internal class DataRouterModel
    {
        internal Dictionary<ushort, List<INetworkSubscriber>> _registrations = new Dictionary<ushort, List<INetworkSubscriber>>();
    }
}
