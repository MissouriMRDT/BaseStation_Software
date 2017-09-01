using System;
using System.Collections.ObjectModel;
using System.Net;
using ServerLog = RED.ViewModels.Tools.TelemetryLogToolViewModel.ServerLog;

namespace RED.Models.Tools
{
    internal class TelemetryLogToolModel
    {
        internal ObservableCollection<ServerLog> TelemetryLog;

        internal class ServerLogModel
        {
            internal string Name;
            internal IPAddress Address;
            internal DateTime Timestamp;
        }
    }
}
