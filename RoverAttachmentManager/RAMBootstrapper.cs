using Caliburn.Micro;
using RoverAttachmentManager.ViewModels;
using System.Windows;

namespace RoverAttachmentManager
{
    public class RAMBootstrapper : BootstrapperBase
    {
        public RAMBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }
    }
}
