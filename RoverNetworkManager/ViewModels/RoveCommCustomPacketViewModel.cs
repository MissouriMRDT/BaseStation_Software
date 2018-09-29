using RoverNetworkManager.Models;
using RoverNetworkManager.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RoverNetworkManager.ViewModels
{
    public class RoveCommCustomPacketViewModel : PropertyChangedBase
    {
        private readonly RoveCommCustomPacketModel _model;

        public Dictionary<string, List<MetadataRecordContext>> Commands
        {
            get
            {
                return _model.Commands;
            }
            set
            {
                _model.Commands = value;
                NotifyOfPropertyChange(() => Commands);
            }
        }

        public void LoadMetadata()
        {
            MetadataSaveContext meta = MetadataManagerConfig.DefaultMetadata;
            
            foreach (MetadataServerContext ctx in meta.Servers)
            {
                Commands.Add(ctx.Name, ctx.Commands.ToList());

                // TODO: implement?
                // Commands.Add(ctx.Name, ctx.Telemetry.ToList());
            }
        }

        public RoveCommCustomPacketViewModel()
        {
            _model = new RoveCommCustomPacketModel();
        }
    }
}
