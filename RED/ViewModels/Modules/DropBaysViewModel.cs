using Caliburn.Micro;
    public class DropBaysViewModel : PropertyChangedBase
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public DropBaysViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
        }

        public void OpenBay(byte index)
        {
            _rovecomm.SendCommand(new Packet("DropBayOpen", index), true);
            _log.Log("Drop bay #{0} opened", index + 1);
        }

        public void CloseBay(byte index)
        {
            _rovecomm.SendCommand(new Packet("DropBayClose", index), true);
            _log.Log("Drop bay #{0} closed", index + 1);
        }
    }
}