using Caliburn.Micro;
using RED.Configurations;
using RED.Contexts;
using RED.Interfaces;
using RoverNetworkManager.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RoverNetworkManager.ViewModels {
	public class RoveCommCustomPacketViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly RoveCommCustomPacketModel _model;

		private readonly IRovecomm _networkManager;

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

		public Dictionary<string, string> Addresses {
			get {
				return _model.Addresses;
			}
			set {
				_model.Addresses = value;
				NotifyOfPropertyChange(() => Addresses);
			}
		}

		public int SelectedCommand {
			get {
				return _model.SelectedCommand;
			}
			set {
				_model.SelectedCommand = value;
				NotifyOfPropertyChange(() => SelectedCommand);
			}
		}

		public string Data {
			get {
				return _model.Data;
			}
			set {
				_model.Data = value;
				NotifyOfPropertyChange(() => Data);
			}
		}

		public string ID {
			get {
				return _model.ID;
			}

			set {
				_model.ID = value;
				NotifyOfPropertyChange(() => ID);
			}
		}

		public string IP {
			get {
				return _model.IP;
			}

			set {
				_model.IP = value;
				NotifyOfPropertyChange(() => IP);
			}
		}

		public string PacketLog {
			get {
				return _model.PacketLog;
			}

			set {
				_model.PacketLog = value;
				NotifyOfPropertyChange(() => PacketLog);
			}
		}

		MetadataSaveContext meta = MetadataManagerConfig.DefaultMetadata;

		public void LoadMetadata()
        {
			Commands.Add("Custom command");
			CommandIDs.Add(0);
			Addresses.Add("Custom command", "");
            
            foreach (MetadataServerContext ctx in meta.Servers)
            {
				List<MetadataRecordContext> l = ctx.Commands.ToList();
				List<string> strings = l.ConvertAll(i => i.ToString());
				string sep = "== " + ctx.Name + " ==";

				Commands.Add(sep);
				Commands.AddRange(strings);

				CommandIDs.Add(0);
				CommandIDs.AddRange(l.ConvertAll(i => i.Id));

				Addresses.Add(sep, "");
				foreach(string s in strings) Addresses.Add(s, ctx.Address);
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

		private void UpdateTextboxes() {
			// clear the textboxes if no item is selected or a seperator is selected
			if (SelectedCommand == -1 || CommandIDs[SelectedCommand] == 0) {
				ID = "";
				IP = "";
				return;
			}

			ID = CommandIDs[SelectedCommand].ToString();
			IP = Addresses[Commands[SelectedCommand]];
		}

		// TODO: reimplement with LINQ
		byte[] StringToByteArray(string raw) {
			List<byte> ret = new List<byte>();
			if (Data != "") {
				foreach (string s in raw.Split(',')) {
					byte conv;
					if (byte.TryParse(s, out conv)) ret.Add(conv);
				}
			}
			else {
				ret.Add(0);
			}

			return ret.ToArray();
		}

		internal void SendCommand() {
			byte[] data = StringToByteArray(Data);

			ushort id;
			if (ushort.TryParse(ID, out id)) {
				_networkManager.SendPacket(id, data.ToArray(), System.Net.IPAddress.Parse(IP), false);
			}
		}

		public RoveCommCustomPacketViewModel(IRovecomm network, IConfigurationManager config)
        {
            _model = new RoveCommCustomPacketModel();
			_networkManager = network;

			_networkManager.SubscribeMyPCToAllDevices();

			LoadMetadata();
			PropertyChanged += RoveCommCustomPacketViewModel_PropertyChanged;
		}

		private void RoveCommCustomPacketViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			string n = e.PropertyName;
			if (e.PropertyName == "SelectedCommand") UpdateTextboxes();
		}

		public void SubscribeID() {
			_networkManager.NotifyWhenMessageReceived(this, ushort.Parse(ID));
		}

		public void ReceivedRovecommMessageCallback(ushort dataId, byte[] data, bool reliable) {
			string d = "";
			foreach (byte b in data.ToArray()) { d += b.ToString() + ","; }
			d = d.Remove(d.Length - 1, 1);

			PacketLog += $"{dataId}: {d}\r\n";
		}
	}
}
