using RoverNetworkManager.Models;
using RoverNetworkManager.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Controls;

namespace RoverNetworkManager.ViewModels
{
    public class RoveCommCustomPacketViewModel : PropertyChangedBase
    {
        private readonly RoveCommCustomPacketModel _model;

        public List<string> Commands
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

		public List<ushort> CommandIDs {
			get {
				return _model.CommandIDs;
			}
			set {
				_model.CommandIDs = value;
				NotifyOfPropertyChange(() => CommandIDs);
			}
		}

		public ushort SelectedCommand {
			get {
				return _model.SelectedCommand;
			}
			set {
				_model.SelectedCommand = CommandIDs[value];
				NotifyOfPropertyChange(() => SelectedCommand);
			}
		}

		public byte Data {
			get {
				return _model.Data;
			}
			set {
				_model.Data = value;
				NotifyOfPropertyChange(() => Data);
			}
		}

		int last = 0;
		MetadataSaveContext meta = MetadataManagerConfig.DefaultMetadata;

		public void LoadMetadata()
        {
			Commands.Add("Custom command");
			CommandIDs.Add(0);
            
            foreach (MetadataServerContext ctx in meta.Servers)
            {
				Commands.Add("== " + ctx.Name + " ==");
				Commands.AddRange(ctx.Commands.ToList().ConvertAll(i => i.ToString()));

				int len = Commands.Count - last;
				System.Diagnostics.Debugger.Log(0, "", $"length is {len}\n");
				last = Commands.Count;

				CommandIDs.Add(0);
				CommandIDs.AddRange(ctx.Commands.ToList().ConvertAll(i => i.Id));
			}
        }

		public MetadataServerContext FindContextByCommand(ushort commandID) {
			foreach (MetadataServerContext ctx in meta.Servers) {
				Commands.Add("== " + ctx.Name + " ==");
				Commands.AddRange(ctx.Commands.ToList().ConvertAll(i => i.ToString()));
				
				foreach(MetadataRecordContext record in ctx.Commands) {
					if (record.Id == commandID) return ctx;
				}
			}

			return null;
		}

        public RoveCommCustomPacketViewModel()
        {
            _model = new RoveCommCustomPacketModel();
			LoadMetadata();
		}
	}
}
