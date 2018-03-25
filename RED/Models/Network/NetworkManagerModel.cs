using System.Collections.Generic;
using RED.Interfaces;

namespace RED.Models.Network
{
    internal class NetworkManagerModel
    {
        internal bool EnableReliablePackets;
        internal Dictionary<ushort, List<ISubscribe>> _registrations = new Dictionary<ushort, List<ISubscribe>>();
    }
}
