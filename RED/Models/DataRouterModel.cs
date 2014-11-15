namespace RED.Models
{
    using Interfaces;
    using System.Collections.Generic;

    public class DataRouterModel
    {
        internal Dictionary<int, List<ISubscribe>> _registrations = new Dictionary<int, List<ISubscribe>>();
    }
}
