using Caliburn.Micro;

namespace RED.ViewModels.Modules
{
    public class DropBaysViewModel : PropertyChangedBase
    {
        private ControlCenterViewModel _cc;

        public DropBaysViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;
        }

        public void OpenBay(byte index)
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("DropBayOpen"), index);
        }
    }
}