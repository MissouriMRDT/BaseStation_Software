namespace RED.Models.Modules
{
    using System;
    using System.Net.Sockets;

    internal class NetworkingModel
    {
        internal string Title = "Networking System";
        internal bool InUse = false;
        internal bool IsManageable = true;

        internal string ConsoleText;
        internal string EndOfMessageDelimiter = "\0"; // Denotes the end of a transmission
        internal bool CanListen = true;
        internal bool CanDisconnect;
        internal bool CanSend;

        internal const int SocketBacklog = 100;
        internal Socket currentSocket;
        internal bool isConnected;
        internal bool stopListening;
        internal string[] responses;

        internal int IdToSend = 0;
        internal string ValueToSend = String.Empty;
    }
}
