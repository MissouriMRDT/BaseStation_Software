namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using System.Collections.Generic;

    public class DataRouter : PropertyChangedBase
    {
        private readonly DataRouterModel _model;

        private Dictionary<int, List<ISubscribe>> Registrations
        {
            get
            {
                return _model._registrations;
            }
            set
            {
                _model._registrations = value;
                NotifyOfPropertyChange(() => Registrations);
            }
        }

        public DataRouter()
        {
            _model = new DataRouterModel();
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
                if (!existingRegistrations.Contains(subscriber))
                    existingRegistrations.Add(subscriber);
            }
            else
                Registrations.Add(dataCode, new List<ISubscribe> { subscriber });
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