using RED.ViewModels.ControlCenter;
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
    }
}