using Caliburn.Micro;
using RED.ViewModels;
using System;
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

        protected override void OnExit(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(System.IO.Path.GetFullPath("RoverMetrics.txt"), "end");
        }
    }
}
