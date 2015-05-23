namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using System.Collections.Generic;

    public class DataRouter : PropertyChangedBase
    {
        private readonly DataRouterModel _model;

        public Dictionary<byte, List<ISubscribe>> Registrations
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

        public void Send(byte dataCode, byte[] data)
        {
            if (dataCode == 0) return;
            List<ISubscribe> registered;
            if (Registrations.TryGetValue(dataCode, out registered))
                foreach (ISubscribe subscription in registered)
                    subscription.ReceiveFromRouter(dataCode, data);
        }
        public void Send(byte dataCode, byte obj)
        {
            Send(dataCode, new byte[] { obj });
        }
        public void Send(byte dataCode, dynamic obj)
        {
            Send(dataCode, System.BitConverter.GetBytes(obj));
        }

        public void Subscribe(ISubscribe subscriber, byte dataCode)
        {
            if (dataCode == 0) return;
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
            var registrationCopy = new Dictionary<byte, List<ISubscribe>>(Registrations); //Use a copy because we may modify it while removing stuff and that breaks the foreach
            foreach (KeyValuePair<byte, List<ISubscribe>> kvp in registrationCopy)
                UnSubscribe(subscriber, kvp.Key);
        }
        public void UnSubscribe(ISubscribe subscriber, byte dataCode)
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