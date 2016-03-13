namespace RED.Models
{
    using Interfaces;
    using System.Collections.Generic;

    public class DataRouterModel
    {
        internal Dictionary<ushort, List<ISubscribe>> _registrations = new Dictionary<ushort, List<ISubscribe>>();
    }
}
