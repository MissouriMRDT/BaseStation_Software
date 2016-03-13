using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface IIPAddressProvider
    {
        IPAddress GetIPAddress(byte dataId);
        byte[] GetAllDataIds(IPAddress ip);
    }
}