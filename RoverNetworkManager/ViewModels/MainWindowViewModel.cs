using Caliburn.Micro;
using RoverNetworkManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverNetworkManager.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly MainWindowModel _model;

        public MainWindowViewModel()
        {
            base.DisplayName = "Rover Network Manager";
            _model = new MainWindowModel();
        }
    }
}
