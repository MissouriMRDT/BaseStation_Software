using REDUpdater.Models;
using Caliburn.Micro;

namespace REDUpdater.ViewModels
{
    class ConsoleViewModel : PropertyChangedBase
    {
        ConsoleModel Model = new ConsoleModel();

        public string Text
        {
            get
            {
                return Model.text;
            }
            set
            {
                Model.text = value;
                NotifyOfPropertyChange(() => Model.text);
            }
        }

        public void Print(string str)
        {
            Text += str + System.Environment.NewLine;
            return;
        }

    }
}
