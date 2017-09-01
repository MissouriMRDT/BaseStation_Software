using RED.Addons.Network;
using System.Collections.Generic;

namespace RED.Models.Network
{
    internal class SubscriptionManagerModel
    {
        internal Dictionary<ushort, SubscriptionRecord> Subscriptions = new Dictionary<ushort, SubscriptionRecord>();
    }
}