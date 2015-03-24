namespace RED
{
    using Caliburn.Micro;
    using System.Windows;
    using ViewModels;

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
