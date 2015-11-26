using RED.Addons;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class SubscriptionManagerViewModel
    {
        public const byte SubscriptionDataId = 2;

        private SubscriptionManagerModel _model;
        private ControlCenterViewModel _cc;
        private IIPAddressProvider ipProvider;

        public Dictionary<byte, SubscriptionRecord> Subscriptions
        {
            get
            {
                return _model._subscriptions;
            }
        }

        public SubscriptionManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new SubscriptionManagerModel();
            _cc = cc;
            ipProvider = _cc.MetadataManager;
        }

        public void Subscribe(byte dataId)
        {
            var ip = ipProvider.GetIPAddress(dataId);
            _cc.NetworkManager.SendPacket(SubscriptionDataId, BitConverter.GetBytes(dataId), ip);
            Subscriptions.Add(dataId, new SubscriptionRecord(SubscriptionStatus.Subscribed, ip, DateTime.Now));
        }

        public void Unsubscribe(byte dataId)
        {
            _cc.NetworkManager.SendPacket(SubscriptionDataId, BitConverter.GetBytes(dataId), Subscriptions[dataId].HostIP);
            Subscriptions[dataId].Status = SubscriptionStatus.Unsubscribed;
            Subscriptions[dataId].Timestamp = DateTime.Now;
        }

        public void Resubscribe(byte dataId)
        {
            var ip = ipProvider.GetIPAddress(dataId);
            _cc.NetworkManager.SendPacket(SubscriptionDataId, BitConverter.GetBytes(dataId), ip);
            Subscriptions[dataId].Timestamp = DateTime.Now;
        }

        public void Resubscribe(IPAddress ip)
        {
            foreach (var sub in Subscriptions.Where(x => x.Value.HostIP == ip))
                Resubscribe(sub.Key);
        }
    }

    public enum SubscriptionStatus
    {
        Unsubscribed = 0,
        Subscribed = 1
    }
}