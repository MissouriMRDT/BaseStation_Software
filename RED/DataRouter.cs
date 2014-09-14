using RED.Interfaces;
using System.Collections.Generic;

namespace RED
{
    public class DataRouter
    {
        private Dictionary<int, List<ISubscribe>> Registrations = new Dictionary<int, List<ISubscribe>>();

        public void Send(int dataCode, byte[] data)
        {
            List<ISubscribe> registered;
            if (Registrations.TryGetValue(dataCode, out registered))
                foreach (ISubscribe subscription in registered)
                    subscription.Receive(dataCode, data);
        }

        public void Register(ISubscribe subscriber, int dataCode)
        {
            List<ISubscribe> existingRegistrations;
            if (Registrations.TryGetValue(dataCode, out existingRegistrations))
            {
                if (existingRegistrations.Contains(subscriber))
                    existingRegistrations.Add(subscriber);
            }
            else
                Registrations.Add(dataCode, new List<ISubscribe>() { subscriber });
        }

        public void UnRegister(ISubscribe subscriber)
        {
            foreach (KeyValuePair<int, List<ISubscribe>> kvp in Registrations)
                UnRegister(subscriber, kvp.Key);
        }
        public void UnRegister(ISubscribe subscriber, int dataCode)
        {
            List<ISubscribe> existingRegistrations;
            if (Registrations.TryGetValue(dataCode, out existingRegistrations))
            {
                existingRegistrations.Remove(subscriber);
                if (existingRegistrations.Count == 0)
                    Registrations.Remove(dataCode);
            }
        }
    }
}