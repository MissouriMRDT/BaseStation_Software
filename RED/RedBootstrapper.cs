using Caliburn.Micro;
using RED.ViewModels;
using System.Windows;

namespace RED
{
    public class RedBootstrapper : BootstrapperBase
    {
        public RedBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ControlCenterViewModel>();
        }
    }
}
