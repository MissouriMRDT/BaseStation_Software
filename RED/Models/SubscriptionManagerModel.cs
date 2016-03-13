using RED.Addons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models
{
    public class SubscriptionManagerModel
    {
        internal Dictionary<ushort, SubscriptionRecord> _subscriptions = new Dictionary<ushort, SubscriptionRecord>();
    }
}