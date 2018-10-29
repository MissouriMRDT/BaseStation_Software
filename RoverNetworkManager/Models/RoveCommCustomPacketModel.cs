using System.Collections.Generic;

namespace RoverNetworkManager.Models
{
    internal class RoveCommCustomPacketModel
    {
        internal List<string> Commands = new List<string>();
		internal List<ushort> CommandIDs = new List<ushort>();
		internal ushort SelectedCommand;
		internal byte Data;

	}
}
