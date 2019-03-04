using Caliburn.Micro;
using RoverNetworkManager.ViewModels;
using System.Windows;

namespace RoverNetworkManager
{
    public class RNMBootstrapper : BootstrapperBase
    {
        public RNMBootstrapper()
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
