using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.RoveProtocol
{
    public class PingManager : IRovecommReceiver {

        private HashSet<PendingPing> pendingPings = new HashSet<PendingPing>();
        private readonly IRovecomm _rovecomm;

        private class PendingPing
        {
            public ushort SeqNum;
            public DateTime Timestamp;
            public System.Threading.SemaphoreSlim Semaphore;
            public TimeSpan RoundtripTime;
        }

        public PingManager(IRovecomm rovecomm)
        {
            _rovecomm = rovecomm;
            _rovecomm.NotifyWhenMessageReceived(this, "PingReply");
        }

        /// <summary>
        /// attempt to ping a device on the network using rovecomm.
        /// </summary>
        /// <param name="ip">ip of the device to ping</param>
        /// <param name="timeout">how many milliseconds to wait before timing out</param>
        /// <returns>how long it took to reply to the ping</returns>
        public async Task<TimeSpan> SendPing(IPAddress ip, TimeSpan timeout)
        {
            PendingPing ping = new PendingPing()
            {
                Timestamp = DateTime.Now,
                SeqNum = 0,
                Semaphore = new System.Threading.SemaphoreSlim(0)
            };
            pendingPings.Add(ping);

            _rovecomm.SendCommand(new Packet("Ping"), true, ip);
            await ping.Semaphore.WaitAsync(timeout);
            return ping.RoundtripTime;
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            DateTime now = DateTime.Now;
            PendingPing ping = pendingPings.First();
            if (ping == null) return;

            ping.RoundtripTime = now - ping.Timestamp;
            ping.Semaphore.Release();
        }
    }
}
