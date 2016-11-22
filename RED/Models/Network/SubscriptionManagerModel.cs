using RED.Addons;
using System.Collections.Generic;

namespace RED.Models.Network
{
    internal class SubscriptionManagerModel
    {
        internal Dictionary<ushort, SubscriptionRecord> _subscriptions = new Dictionary<ushort, SubscriptionRecord>();
    }
}