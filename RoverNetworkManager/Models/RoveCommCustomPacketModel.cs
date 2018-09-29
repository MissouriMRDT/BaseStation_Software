using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverNetworkManager.Networking;

namespace RoverNetworkManager.Models
{
    class RoveCommCustomPacketModel
    {
        public Dictionary<string, List<MetadataRecordContext>> Commands = new Dictionary<string, List<MetadataRecordContext>>();
    }
}
