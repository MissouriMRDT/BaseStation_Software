using Caliburn.Micro;
using RED.ViewModels;
using System;
using System.Windows;
using RED.Models.Modules;

namespace RED
{
    public class RedBootstrapper : BootstrapperBase
    {
        private readonly GPSModel _model;
        public RedBootstrapper()
        {
            _model = new GPSModel();
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ControlCenterViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
           
            System.IO.File.WriteAllText(System.IO.Path.GetFullPath("RoverMetrics.txt"), _model.RoverDistanceTravelled.ToString());
        }
    }
}
