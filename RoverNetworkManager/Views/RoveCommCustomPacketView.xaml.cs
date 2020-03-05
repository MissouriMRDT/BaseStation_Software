using RoverOverviewNetwork.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RoverOverviewNetwork.Views
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
			((RoveCommCustomPacketViewModel)DataContext).SendCommand();
		}

		public void btnSubscribe_Click(object sender, RoutedEventArgs e) {
			((RoveCommCustomPacketViewModel)DataContext).SubscribeID();
		}
    }
}
