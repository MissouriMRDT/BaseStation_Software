using RoverNetworkManager.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RoverNetworkManager.Views
{
    /// <summary>
    /// Interaction logic for RoveCommCustomPacket.xaml
    /// </summary>
    public partial class RoveCommCustomPacketView : UserControl
    {
		private Networking.Rovecomm rc = new Networking.Rovecomm();

		public RoveCommCustomPacketView() {
			InitializeComponent();
		}

		private void cbxBoard_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			RoveCommCustomPacketViewModel vm = (RoveCommCustomPacketViewModel)DataContext;

			ushort index = (ushort)((ComboBox)(sender)).SelectedIndex;

			vm.SelectedCommand = index;

			// can't be set through the index directly because the SelectedCommand setter
			// updates itself to the command's id
			txtID.Text = vm.SelectedCommand.ToString();

			Networking.MetadataServerContext ctx = vm.FindContextByCommand(vm.SelectedCommand);
			if (ctx != null) txtIP.Text = ctx.Address;

			// category separators and custom commands have the "id" of 0
			if (txtID.Text == "0") {
				txtID.Text = "";
				txtIP.Text = "";
			}

			else {
				txtData.Text = "";
			}
		}

		private void btnSend_Click(object sender, System.Windows.RoutedEventArgs e) {
			RoveCommCustomPacketViewModel vm = (RoveCommCustomPacketViewModel)DataContext;

			List<byte> data = new List<byte>();
			if(txtData.Text != "") {
				foreach(string s in txtData.Text.Split(',')) {
					byte conv;
					if (byte.TryParse(s, out conv)) data.Add(conv);
				}
			}
			else {
				data.Add(0);
			}

			string d = "";
			foreach(byte b in data.ToArray()) { d += b.ToString() + ","; }
			d = d.Remove(d.Length - 1, 1);

			ushort id;
			if(ushort.TryParse(txtID.Text, out id)) {
				rc.SendPacket(id, data.ToArray(), System.Net.IPAddress.Parse(txtIP.Text), 0);
				MessageBox.Show($"Sent command {id} with data of {d} to {txtIP.Text}");
			}
		}
	}
}
