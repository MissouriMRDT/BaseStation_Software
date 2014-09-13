using RED.Interfaces;
using System.Collections.Generic;

namespace RED
{
    public class DataRouter
    {
        private Dictionary<int, List<iSubscribe>> Registrations = new Dictionary<int, List<iSubscribe>>();

        public void Send(int dataCode, byte[] data)
        {
            List<iSubscribe> registered;
            if (Registrations.TryGetValue(dataCode, out registered))
                foreach (iSubscribe subscription in registered)
                    subscription.Receive(dataCode, data);
        }

        public void Register(iSubscribe subscriber, int dataCode)
        {
            List<iSubscribe> existingRegistrations;
            if (Registrations.TryGetValue(dataCode, out existingRegistrations))
            {
                if (existingRegistrations.Contains(subscriber))
                    existingRegistrations.Add(subscriber);
            }
            else
                Registrations.Add(dataCode, new List<iSubscribe>() { subscriber });
        }

        public void UnRegister(iSubscribe subscriber)
        {
            foreach (KeyValuePair<int, List<iSubscribe>> kvp in Registrations)
                UnRegister(subscriber, kvp.Key);
        }
        public void UnRegister(iSubscribe subscriber, int dataCode)
        {
            List<iSubscribe> existingRegistrations;
            if (Registrations.TryGetValue(dataCode, out existingRegistrations))
            {
                existingRegistrations.Remove(subscriber);
                if (existingRegistrations.Count == 0)
                    Registrations.Remove(dataCode);
            }
        }
    }
}