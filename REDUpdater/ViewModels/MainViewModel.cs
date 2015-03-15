using REDUpdater.Models;
using Caliburn.Micro;

namespace REDUpdater.ViewModels
{
    class MainViewModel : PropertyChangedBase
    {
        MainModel Model = new MainModel();

        public short percent
        {
            get
            {
                return Model.percent;
            }
            set
            {
                Model.percent = value;
                NotifyOfPropertyChange(() => Model.percent);
            }
        }
    }
}
