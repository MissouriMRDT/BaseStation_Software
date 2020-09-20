using Caliburn.Micro;
using RoverOverviewNetwork.ViewModels;
using System.Windows;

namespace RoverOverviewNetwork
{
    public class RONBootstrapper : BootstrapperBase
    {
        public RONBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

		public void DisplayNetworkManager() {
			OnStartup(null, null);
		}
    }
}
