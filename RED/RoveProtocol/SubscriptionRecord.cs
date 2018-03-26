using RED.ViewModels.Network;
using System;
using System.Net;

namespace RED.Roveprotocol
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