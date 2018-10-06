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
			MessageBox.Show($"Sending command of {txtID.Text} with data of {d} to {txtIP.Text}");
		}
	}
}
