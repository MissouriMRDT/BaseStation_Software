using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverNetworkManager.Networking;

namespace RoverNetworkManager.Models
{
    internal class MainWindowModel
    {
        public static void LoadMetadata()
        {
            MetadataSaveContext meta = MetadataManagerConfig.DefaultMetadata;
            meta.ToString();

            Dictionary<string, List<MetadataRecordContext>> commands = new Dictionary<string, List<MetadataRecordContext>>();
            foreach (MetadataServerContext ctx in meta.Servers)
            {
                commands.Add(ctx.Name, ctx.Commands.ToList());

                // TODO: implement?
                // commands.Add(ctx.Name, ctx.Telemetry.ToList());
            }
        }
    }
}
