using Caliburn.Micro;
using RED.Models;
using RED.ViewModels.Network;

namespace RED.ViewModels
{
    public class StateViewModel : PropertyChangedBase
    {
        private readonly StateModel _model = new StateModel();
        private readonly SubscriptionManagerViewModel _subscriptionManager;

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public string CurrentControlMode
        {
            get
            {
                return _model._currentControlMode;
            }
            set
            {
                _model._currentControlMode = value;
                NotifyOfPropertyChange(() => CurrentControlMode);
            }
        }
        public bool NetworkHasConnection
        {
            get
            {
                return _model._networkHasConnection;
            }
            set
            {
                _model._networkHasConnection = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => NetworkConnectionStatus);
            }
        }
        public string NetworkConnectionStatus
        {
            get
            {
                return NetworkHasConnection ? "Connected" : "Disconnected";
            }
        }
        public bool ControllerIsConnected
        {
            get
            {
                return _model._controllerIsConnected;
            }
            set
            {
                _model._controllerIsConnected = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => ControllerConnectionStatus);
            }
        }
        public string ControllerConnectionStatus
        {
            get
            {
                return ControllerIsConnected ? "Connected" : "Disconnected";
            }
        }

        public StateViewModel(SubscriptionManagerViewModel subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
            CurrentControlMode = "";
        }

        public void ResubscribeAll()
        {
            _subscriptionManager.ResubscribeAll();
        }
    }
}
