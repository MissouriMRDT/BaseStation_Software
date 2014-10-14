using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models.ControlCenter
{
    public class DataRouterModel
    {
        internal Dictionary<int, List<ISubscribe>> Registrations = new Dictionary<int, List<ISubscribe>>();
    }
}
