using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces.Network
{
    public interface INetworkMessenger
    {
        void SendOverNetwork(ushort dataId, dynamic obj, bool reliable = false);
        void SendOverNetwork(ushort dataId, byte obj, bool reliable = false);
        void SendOverNetwork(ushort dataId, byte[] data, bool reliable = false);

        void Subscribe(ISubscribe subscriber, ushort dataId);
        void UnSubscribe(ISubscribe subscriber);
        void UnSubscribe(ISubscribe subscriber, ushort dataId);
    }
}
