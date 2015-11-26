using RED.Models;
using RED.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class SubscriptionManagerViewModel
    {
        private SubscriptionManagerModel _model;
        private ControlCenterViewModel _cc;

        public SubscriptionManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new SubscriptionManagerModel();
            _cc = cc;
        }
    }
}