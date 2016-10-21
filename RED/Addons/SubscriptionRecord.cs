using RED.ViewModels.ControlCenter;
using RED.ViewModels.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.Addons
{
    public class SubscriptionRecord
    {
        public SubscriptionStatus Status;
        public IPAddress HostIP;
        public DateTime Timestamp;

        public SubscriptionRecord(SubscriptionStatus status, IPAddress hostIP, DateTime timestamp)
        {
            Status = status;
            HostIP = hostIP;
            Timestamp = timestamp;
        }
    }
}