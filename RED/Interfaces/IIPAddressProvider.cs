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
        IPAddress GetIPAddress(ushort dataId);
        ushort[] GetAllDataIds(IPAddress ip);
    }
}