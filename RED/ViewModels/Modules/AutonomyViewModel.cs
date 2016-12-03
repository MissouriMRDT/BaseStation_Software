using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Modules
{
    public class AutonomyViewModel : PropertyChangedBase
    {
        private AutonomyModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        public AutonomyViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new AutonomyModel();

            _router = router;
            _idResolver = idResolver;
        }
    }
}
