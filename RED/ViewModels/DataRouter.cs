using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using System.Collections.Generic;

namespace RED.ViewModels
{
    public class DataRouter : PropertyChangedBase, IDataRouter
    {
        private readonly DataRouterModel _model;
        private readonly ILogger _log;

        public Dictionary<ushort, List<ISubscribe>> Registrations
        {
            get
            {
                return _model._registrations;
            }
        }

        public DataRouter(ILogger log)
        {
            _model = new DataRouterModel();
            _log = log;
        }

        public void Send(ushort dataId, byte[] data, bool reliable = false)
        {
            if (dataId == 0) return;
            if (Registrations.TryGetValue(dataId, out List<ISubscribe> registered))
                foreach (ISubscribe subscription in registered)
                {
                    try
                    {
                        subscription.ReceiveFromRouter(dataId, data, reliable);
                    }
                    catch (System.Exception e)
                    {
                        _log.Log("Error parsing packet with dataid={0}{1}{2}", dataId, System.Environment.NewLine, e);
                    }
                }
        }
        public void Send(ushort dataId, byte obj, bool reliable = false)
        {
            Send(dataId, new byte[] { obj }, reliable);
        }
        public void Send(ushort dataId, dynamic obj, bool reliable = false)
        {
            Send(dataId, System.BitConverter.GetBytes(obj), reliable);
        }

        public void Subscribe(ISubscribe subscriber, ushort dataId)
        {
            if (dataId == 0) return;
            if (Registrations.TryGetValue(dataId, out List<ISubscribe> existingRegistrations))
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
            if (Registrations.TryGetValue(dataId, out List<ISubscribe> existingRegistrations))
            {
                existingRegistrations.Remove(subscriber);
                if (existingRegistrations.Count == 0)
                    Registrations.Remove(dataId);
            }
        }
    }
}