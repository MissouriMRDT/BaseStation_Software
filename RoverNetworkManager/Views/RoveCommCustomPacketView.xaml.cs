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

		private void btnSend_Click(object sender, RoutedEventArgs e) {
			RoveCommCustomPacketViewModel vm = (RoveCommCustomPacketViewModel)DataContext;
			vm.SendCommand();
		}
	}
}
