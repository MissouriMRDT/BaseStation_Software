using System.Collections.Generic;

namespace RoverNetworkManager.Models
{
    internal class RoveCommCustomPacketModel
    {
        internal List<string> Commands = new List<string>();
		internal List<ushort> CommandIDs = new List<ushort>();
		internal Dictionary<string, string> Addresses = new Dictionary<string, string>();
		internal ushort SelectedCommand;
		internal string Data;
		internal string ID;
		internal string IP;
	}
}
