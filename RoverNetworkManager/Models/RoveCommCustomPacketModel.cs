using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverNetworkManager.Networking;

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
