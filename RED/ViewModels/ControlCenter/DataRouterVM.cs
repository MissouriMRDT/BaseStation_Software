using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.ControlCenter;
using System.Collections.Generic;

namespace RED.ViewModels.ControlCenter
{
    public class DataRouterVM : PropertyChangedBase
    {
        private DataRouterModel Model;

        private Dictionary<int, List<ISubscribe>> Registrations
        {
            get
            {
                return Model.Registrations;
            }
            set
            {
                Model.Registrations = value;
                NotifyOfPropertyChange(() => Registrations);
            }
        }

        public DataRouterVM()
        {
            Model = new DataRouterModel();
        }

        public void Send(int dataCode, byte[] data)
        {
            List<ISubscribe> registered;
            if (Registrations.TryGetValue(dataCode, out registered))
                foreach (ISubscribe subscription in registered)
                    subscription.Receive(dataCode, data);
        }

        public void Subscribe(ISubscribe subscriber, int dataCode)
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

        public void UnSubscribe(ISubscribe subscriber)
        {
            foreach (KeyValuePair<int, List<ISubscribe>> kvp in Registrations)
                UnSubscribe(subscriber, kvp.Key);
        }
        public void UnSubscribe(ISubscribe subscriber, int dataCode)
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