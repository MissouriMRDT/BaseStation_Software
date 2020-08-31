using System.Collections.Generic;
using Core.Interfaces;

namespace RED.Models.Network
{
    internal class NetworkManagerModel
    {
        internal Dictionary<ushort, List<IRovecommReceiver>> _registrations = new Dictionary<ushort, List<IRovecommReceiver>>();
    }
}
