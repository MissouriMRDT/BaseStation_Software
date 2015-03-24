using REDUpdater.Models;
using Caliburn.Micro;

namespace REDUpdater.ViewModels
{
    class MainWindowViewModel : PropertyChangedBase
    {
        private MainWindowModel Model;

        public ConsoleViewModel Console
        {
            get
            {
                return Model.console;
            }
            set
            {
                Model.console = value;
                NotifyOfPropertyChange(() => Model.console);
            }
        }

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

        public MainWindowViewModel()
        {
            Model = new MainWindowModel();
            Model.console = new ConsoleViewModel(this);
            Model.percent = 0;
        }
    }
}
