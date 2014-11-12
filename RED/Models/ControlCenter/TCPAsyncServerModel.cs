using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models.ControlCenter
{
    public class TCPAsyncServerModel
    {
        internal string LocalMachineName;
        internal string LocalSoftwareName;

        internal bool IsListening;
        internal short ListeningPort;
    }
}
