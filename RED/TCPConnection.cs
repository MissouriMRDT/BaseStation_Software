using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using RED.Interfaces;

namespace RED
{
    public class TCPConnection : ISubscribe
    {
        public TcpClient Client { get; set; }
        public IPAddress Ip
        {
            get
            {
                return ((IPEndPoint)(Client.Client.RemoteEndPoint)).Address;
            }
        }
        public string RemoteName { get; set; }
        public string RemoteSoftware { get; set; }

        public TCPConnection(TcpClient client)
        {
            this.Client = client;
            throw new NotImplementedException();
            //Talk to the client and get the name and software
        }

        public void Close()
        {
            Client.Close();
        }

        public void Receive(int dataId, byte[] data)
        {
            throw new NotImplementedException();
            //Send packet to client
        }
    }
}
