namespace RED.Models
{
    using Interfaces;
    using System.Collections.Generic;

    public class DataRouterModel
    {
        internal Dictionary<byte, List<ISubscribe>> _registrations = new Dictionary<byte, List<ISubscribe>>();
    }
}
