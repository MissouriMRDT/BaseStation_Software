using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace RED.Models.ControlCenter
{
    internal class TCPConnectionModel
    {
        internal TcpClient Client;
        internal string RemoteName;
        internal string RemoteSoftware;
    }
}
