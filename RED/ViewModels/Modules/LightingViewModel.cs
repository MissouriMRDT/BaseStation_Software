using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;

namespace RED.ViewModels.Modules
{
    public class LightingViewModel : PropertyChangedBase
    {
        private readonly LightingModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public bool Enabled
        {
            get
            {
                return _model.Enabled;
            }
            set
            {
                _model.Enabled = value;
                NotifyOfPropertyChange(() => Enabled);
                if (value)
                    SendColors();
                else
                    TurnOff();
            }
        }
        public bool HeadlightsEnabled
        {
            get
            {
                return _model.HeadlightsEnabled;
            }
            set
            {
                _model.HeadlightsEnabled = value;
                NotifyOfPropertyChange(() => HeadlightsEnabled);
                if (value)
                {
                
                    _log.Log("Doin our thing");
                    TurnOnHeadlights();
                }
                else
                    TurnOffHeadlights();
            }
        }
        public byte Red
        {
            get
            {
                return _model.Red;
            }
            set
            {
                _model.Red = value;
                NotifyOfPropertyChange(() => Red);
                SendColors();
            }
        }
        public byte Green
        {
            get
            {
                return _model.Green;
            }
            set
            {
                _model.Green = value;
                NotifyOfPropertyChange(() => Green);
                SendColors();
            }
        }
        public byte Blue
        {
            get
            {
                return _model.Blue;
            }
            set
            {
                _model.Blue = value;
                NotifyOfPropertyChange(() => Blue);
                SendColors();
            }
        }

        public LightingViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new LightingModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
        }

        private void SendColors()
        {
            if (Enabled)
                _rovecomm.SendCommand(_idResolver.GetId("UnderglowColor"), new byte[] { Red, Green, Blue });
        }

        private void TurnOff()
        {
            _rovecomm.SendCommand(_idResolver.GetId("UnderglowColor"), new byte[] { 0, 0, 0 }, true);
        }

        private void TurnOnHeadlights()
        {
            _rovecomm.SendCommand(_idResolver.GetId("Headlights"), new byte[] { 1 }, false);
        }

        private void TurnOffHeadlights()
        {
            _rovecomm.SendCommand(_idResolver.GetId("Headlights"), new byte[] { 0 }, false);
        }
    }
}
