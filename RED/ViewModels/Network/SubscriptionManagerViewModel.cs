using RED.Addons;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Network;
using RED.Models.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RED.ViewModels.Network
{
    public class SubscriptionManagerViewModel
    {
        public const byte SubscriptionDataId = 3;
        public const byte UnSubscribeDataId = 4;

        private SubscriptionManagerModel _model;
        private ILogger _log;
        private IIPAddressProvider _ipProvider;
        private NetworkManagerViewModel _networkManager;

        public Dictionary<ushort, SubscriptionRecord> Subscriptions
        {
            get
            {
                return _model._subscriptions;
            }
        }

        public SubscriptionManagerViewModel(ILogger log, IIPAddressProvider ipProvider, NetworkManagerViewModel networkManager)
        {
            _model = new SubscriptionManagerModel();
            _log = log;
            _ipProvider = ipProvider;
            _networkManager = networkManager;
        }

        public void SendInitialSubscriptions(MetadataRecordContext[] telemetry)
        {
            foreach (var t in telemetry)
                Subscribe(t.Id);
            _log.Log("Telemetry Subscriptions Sent");
        }

        public void Subscribe(ushort dataId)
        {
            var ip = _ipProvider.GetIPAddress(dataId);
            _networkManager.SendPacket(SubscriptionDataId, BitConverter.GetBytes(dataId), ip, false);
            Subscriptions.Add(dataId, new SubscriptionRecord(SubscriptionStatus.Subscribed, ip, DateTime.Now));
        }

        public void Unsubscribe(ushort dataId)
        {
            _networkManager.SendPacket(UnSubscribeDataId, BitConverter.GetBytes(dataId), Subscriptions[dataId].HostIP, false);
            Subscriptions[dataId].Status = SubscriptionStatus.Unsubscribed;
            Subscriptions[dataId].Timestamp = DateTime.Now;
        }

        public void Resubscribe(ushort dataId)
        {
            var ip = _ipProvider.GetIPAddress(dataId);
            _networkManager.SendPacket(SubscriptionDataId, BitConverter.GetBytes(dataId), ip, false);
            Subscriptions[dataId].Timestamp = DateTime.Now;
        }

        public void Resubscribe(IPAddress ip)
        {
            foreach (var sub in Subscriptions.Where(x => x.Value.HostIP == ip))
                Resubscribe(sub.Key);
        }

        public void ResubscribeAll()
        {
            foreach (var sub in Subscriptions)
                Resubscribe(sub.Key);
            _log.Log("Telemetry Subscriptions Sent");
        }
    }

    public enum SubscriptionStatus
    {
        Unsubscribed = 0,
        Subscribed = 1
    }
}