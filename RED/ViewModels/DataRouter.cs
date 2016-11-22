using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using System.Collections.Generic;

namespace RED.ViewModels
{
    public class DataRouter : PropertyChangedBase, IDataRouter
    {
        private readonly DataRouterModel _model;

        public Dictionary<ushort, List<ISubscribe>> Registrations
        {
            get
            {
                return _model._registrations;
            }
        }

        public DataRouter()
        {
            _model = new DataRouterModel();
        }

        public void Send(ushort dataId, byte[] data)
        {
            if (dataId == 0) return;
            List<ISubscribe> registered;
            if (Registrations.TryGetValue(dataId, out registered))
                foreach (ISubscribe subscription in registered)
                    subscription.ReceiveFromRouter(dataId, data);
        }
        public void Send(ushort dataId, byte obj)
        {
            Send(dataId, new byte[] { obj });
        }
        public void Send(ushort dataId, dynamic obj)
        {
            Send(dataId, System.BitConverter.GetBytes(obj));
        }

        public void Subscribe(ISubscribe subscriber, ushort dataId)
        {
            if (dataId == 0) return;
            List<ISubscribe> existingRegistrations;
            if (Registrations.TryGetValue(dataId, out existingRegistrations))
            {
                if (!existingRegistrations.Contains(subscriber))
                    existingRegistrations.Add(subscriber);
            }
            else
                Registrations.Add(dataId, new List<ISubscribe> { subscriber });
        }

        public void UnSubscribe(ISubscribe subscriber)
        {
            var registrationCopy = new Dictionary<ushort, List<ISubscribe>>(Registrations); //Use a copy because we may modify it while removing stuff and that breaks the foreach
            foreach (KeyValuePair<ushort, List<ISubscribe>> kvp in registrationCopy)
                UnSubscribe(subscriber, kvp.Key);
        }
        public void UnSubscribe(ISubscribe subscriber, ushort dataId)
        {
            List<ISubscribe> existingRegistrations;
            if (Registrations.TryGetValue(dataId, out existingRegistrations))
            {
                existingRegistrations.Remove(subscriber);
                if (existingRegistrations.Count == 0)
                    Registrations.Remove(dataId);
            }
        }
    }
}