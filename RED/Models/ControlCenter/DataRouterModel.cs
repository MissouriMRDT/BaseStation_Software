namespace RED.Models.ControlCenter
{
    using Interfaces;
    using System.Collections.Generic;

    public class DataRouterModel
    {
        internal Dictionary<int, List<ISubscribe>> Registrations = new Dictionary<int, List<ISubscribe>>();
    }
}
