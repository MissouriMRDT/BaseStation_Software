using System;
using System.Net;

namespace Core.RoveProtocol
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